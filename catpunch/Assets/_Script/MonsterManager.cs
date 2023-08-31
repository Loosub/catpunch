using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class AppearanceMonster
{
    [SerializeField] private GameObject Prefab_Monster;
    [SerializeField] private int SpawnNumber;
    [ReadOnly][SerializeField] private float SpawnRate;

    public int GetSpawnNumber()
    {
        return SpawnNumber;
    }
    public GameObject GetPrefabMonster()
    {
        return Prefab_Monster;
    }
    public void SetSpawnSpawnRate(float Value)
    {
        SpawnRate = Value;
    }

}
[System.Serializable]
public struct MonsterStatus
{
    [ReadOnly] public int HpDefault;
    [ReadOnly] public int Hp;
    [ReadOnly] public float JewelryDropRate;
}

public class MonsterManager : MonoBehaviour
{

    private int FullSpawnNumber;
    private GameObject GO_CurrentMonster;
    private GameObject GO_NextMonster;
    private RectTransform GO_NextMonster_Move;
    private GameObject GO_LastHit; //라스트 히트 오브젝트
    private int lastHitScore;

    [Space]
    public AppearanceMonster[] MonsterList;
    public MonsterStatus CurrentMonsterStatus;

    [Header("Links")]
    public GameObject GO_Player;
    public GameObject LastHitPrefab; //라스트 히트 프리팹
    public TextMeshProUGUI TMP_CurrentMonsterHp;

    [Header("Value")]
    public float MonsterDieDelay;
    public Image HealthBar; //몬스터 체력바 이미지
    public bool LastHitStart; //라스트 히트 실행 여부

    ////////////////
    /// FUNCTION ///
    ////////////////

    void Start()
    {
        GO_CurrentMonster = Instantiate(MonsterCreator(), this.transform);
        CurrentMonsterSetup();
        GO_CurrentMonster.GetComponent<Animator>().Play("Default_Position");
        GO_NextMonster = Instantiate(MonsterCreator(), this.transform);
        foreach (var ml in MonsterList)
        {
            FullSpawnNumber += ml.GetSpawnNumber();
        }
        foreach (var ml in MonsterList)
        {
            ml.SetSpawnSpawnRate((float)ml.GetSpawnNumber() / FullSpawnNumber);
        }

        LastHitStart = false;
        lastHitScore = 0;
    }

    void Update()
    {
        
    }

    public void Attacked(int Damage, float critical)
    {
        CurrentMonsterStatus.Hp = CurrentMonsterStatus.Hp - Damage;

        UiMonsterHpUpdate();

        if (CurrentMonsterStatus.Hp <= 0)
        {
            MonsterDie();
        }
        if (CurrentMonsterStatus.Hp <= 1) //몬스터 체력이 1일 경우
        {
            LastHitCreator(); //라스트 히트 실행
        }
    }

    void CurrentMonsterSetup()
    {
        CurrentMonsterStatus.Hp = GO_CurrentMonster.GetComponent<Monster>().GetHp();
        CurrentMonsterStatus.HpDefault = GO_CurrentMonster.GetComponent<Monster>().GetHp();

        HealthBar.fillAmount = 1.0f; //체력바 max로 초기화
        TMP_CurrentMonsterHp.text = CurrentMonsterStatus.Hp.ToString() + " / " + CurrentMonsterStatus.HpDefault.ToString();
    }

    void UiMonsterHpUpdate()
    {
        TMP_CurrentMonsterHp.text = CurrentMonsterStatus.Hp.ToString() + " / " + CurrentMonsterStatus.HpDefault.ToString();
        HealthBar.fillAmount = (float)CurrentMonsterStatus.Hp / (float)CurrentMonsterStatus.HpDefault; //몬스터 체력바 감소
    }

    void FindCorrectDropAmount(ref int resultAmount,ref JewelType resultDropJewel)
    {
        resultAmount = 0;
        List<DropInfomation> currentMonsterDropInfo;
        currentMonsterDropInfo = GO_CurrentMonster.GetComponent<Monster>().GetDropInfo();
        int randomPoint = Random.Range(0, 101);
        int point = randomPoint + lastHitScore;
        foreach (DropInfomation dropInfo in currentMonsterDropInfo)
        {
            if (point <= dropInfo.GetPoint())
            {
                Debug.Log("POINT :" + point + " DROPPOINT :" + dropInfo.GetPoint() + " LASTHITSCORE :" + lastHitScore + " DROPCOUNT :" + resultAmount + " DROPJEWEL :" + resultDropJewel);

                return;
            }
            resultAmount = dropInfo.GetDropAmount();
            resultDropJewel = dropInfo.GetDropJewel();
        }
        Debug.Log("POINT :" + point + " LastHitScore :" + lastHitScore + " DROPCOUNT :" + resultAmount + " DROPJEWEL :" + resultDropJewel);
        return ;
    }

    void MonsterDie()
    {
        //라스트 히트
        LastHitStart = false; //라스트 히트 화살표 회전 종료
        GO_Player.GetComponent<PlayerCharacter>().SetHitable(false);
        GO_LastHit.transform.GetChild(0).GetComponent<LastHit>().LastHitScoreDecide(); //점수 측정
        GameObject LasthitRotate = GO_LastHit.transform.GetChild(0).gameObject; //라스트 히트 프리팹의 Lasthit_Rotate(화살표) 자식 객체 가져오기
        lastHitScore=LasthitRotate.GetComponent<LastHit>().LastHitScore;

        Invoke("LastHitQuit", 0.2f); //0.2초 후 라스트 히트 사라짐

        //보석 획득
        int dropAmount = 0;
        JewelType dropJewel = 0;
        FindCorrectDropAmount(ref dropAmount, ref dropJewel);
        switch (dropJewel)
        {
            case JewelType.Silver:
                GO_Player.GetComponent<PlayerCharacter>().Silver.add(dropAmount);
                break;
            case JewelType.Gold:
                GO_Player.GetComponent<PlayerCharacter>().Gold.add(dropAmount);
                break;
            case JewelType.Emerald:
                GO_Player.GetComponent<PlayerCharacter>().Emerald.add(dropAmount);
                break;
            case JewelType.Diamond:
                GO_Player.GetComponent<PlayerCharacter>().Diamond.add(dropAmount);
                break;
        }

        //몬스터 정리
        Destroy(GO_CurrentMonster);
        GO_CurrentMonster = GO_NextMonster;

        if (GO_NextMonster == null)
        {
            Debug.Log("GO_NextMonster가 null입니다.");
        }
        CurrentMonsterSetup();
        if (MonsterList.GetLength(0) == 0)
        {
            Debug.Log("Monster List에 Monster가 없습니다.");
        }
        else
        {
            GO_NextMonster = Instantiate(MonsterCreator(), this.transform);
            GO_NextMonster.GetComponent<Animator>().SetTrigger("OutOfScreen");
            GO_CurrentMonster.GetComponent<Animator>().SetTrigger("Spawn");   
        }
    }

    void LastHitCreator() //라스트 히트 생성
    {
        LastHitStart = true;
        GO_LastHit = Instantiate(LastHitPrefab, this.transform); //Moster Manager 객체 안에 라스트 히트 자식 객체로 생성
        GO_LastHit.transform.position = new Vector3(-1.36f, 0.26f, 0.0f); //라스트 히트 생성 위치 고정

        GameObject LasthitBar = GO_LastHit.transform.GetChild(1).gameObject; //라스트 히트 프리팹의 Lasthit_Bar(원형 바) 자식 객체 가져오기
        LasthitBar.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, -360.0f)); //원형 바 회전 각도 설정(0~360도)
    }

    void LastHitQuit() //라스트 히트 종료
    {
        Destroy(GO_LastHit); //라스트 히트 오브젝트 삭제
    }

    GameObject MonsterCreator()
    {
        int r = Random.Range(0, MonsterList.GetLength(0));
        int randomNumber = Random.Range(1, FullSpawnNumber);

        GameObject result = MonsterList[0].GetPrefabMonster();
        int compareNumber = 0;
        foreach (var ml in MonsterList)
        {
            compareNumber += ml.GetSpawnNumber();
            if (randomNumber <= compareNumber)
            {
                result = ml.GetPrefabMonster();
                break;
            }
        }
        Debug.Log(randomNumber + "<=" + compareNumber);
        return result;
    }
}

