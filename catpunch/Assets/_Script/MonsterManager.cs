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
    private GameObject GO_LastHit; //��Ʈ ��Ʈ ������Ʈ
    private int lastHitScore;

    [Space]
    public AppearanceMonster[] MonsterList;
    public MonsterStatus CurrentMonsterStatus;

    [Header("Links")]
    public GameObject GO_Player;
    public GameObject LastHitPrefab; //��Ʈ ��Ʈ ������
    public TextMeshProUGUI TMP_CurrentMonsterHp;

    [Header("Value")]
    public float MonsterDieDelay;
    public Image HealthBar; //���� ü�¹� �̹���
    public bool LastHitStart; //��Ʈ ��Ʈ ���� ����

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
        if (CurrentMonsterStatus.Hp <= 1) //���� ü���� 1�� ���
        {
            LastHitCreator(); //��Ʈ ��Ʈ ����
        }
    }

    void CurrentMonsterSetup()
    {
        CurrentMonsterStatus.Hp = GO_CurrentMonster.GetComponent<Monster>().GetHp();
        CurrentMonsterStatus.HpDefault = GO_CurrentMonster.GetComponent<Monster>().GetHp();

        HealthBar.fillAmount = 1.0f; //ü�¹� max�� �ʱ�ȭ
        TMP_CurrentMonsterHp.text = CurrentMonsterStatus.Hp.ToString() + " / " + CurrentMonsterStatus.HpDefault.ToString();
    }

    void UiMonsterHpUpdate()
    {
        TMP_CurrentMonsterHp.text = CurrentMonsterStatus.Hp.ToString() + " / " + CurrentMonsterStatus.HpDefault.ToString();
        HealthBar.fillAmount = (float)CurrentMonsterStatus.Hp / (float)CurrentMonsterStatus.HpDefault; //���� ü�¹� ����
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
        //��Ʈ ��Ʈ
        LastHitStart = false; //��Ʈ ��Ʈ ȭ��ǥ ȸ�� ����
        GO_Player.GetComponent<PlayerCharacter>().SetHitable(false);
        GO_LastHit.transform.GetChild(0).GetComponent<LastHit>().LastHitScoreDecide(); //���� ����
        GameObject LasthitRotate = GO_LastHit.transform.GetChild(0).gameObject; //��Ʈ ��Ʈ �������� Lasthit_Rotate(ȭ��ǥ) �ڽ� ��ü ��������
        lastHitScore=LasthitRotate.GetComponent<LastHit>().LastHitScore;

        Invoke("LastHitQuit", 0.2f); //0.2�� �� ��Ʈ ��Ʈ �����

        //���� ȹ��
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

        //���� ����
        Destroy(GO_CurrentMonster);
        GO_CurrentMonster = GO_NextMonster;

        if (GO_NextMonster == null)
        {
            Debug.Log("GO_NextMonster�� null�Դϴ�.");
        }
        CurrentMonsterSetup();
        if (MonsterList.GetLength(0) == 0)
        {
            Debug.Log("Monster List�� Monster�� �����ϴ�.");
        }
        else
        {
            GO_NextMonster = Instantiate(MonsterCreator(), this.transform);
            GO_NextMonster.GetComponent<Animator>().SetTrigger("OutOfScreen");
            GO_CurrentMonster.GetComponent<Animator>().SetTrigger("Spawn");   
        }
    }

    void LastHitCreator() //��Ʈ ��Ʈ ����
    {
        LastHitStart = true;
        GO_LastHit = Instantiate(LastHitPrefab, this.transform); //Moster Manager ��ü �ȿ� ��Ʈ ��Ʈ �ڽ� ��ü�� ����
        GO_LastHit.transform.position = new Vector3(-1.36f, 0.26f, 0.0f); //��Ʈ ��Ʈ ���� ��ġ ����

        GameObject LasthitBar = GO_LastHit.transform.GetChild(1).gameObject; //��Ʈ ��Ʈ �������� Lasthit_Bar(���� ��) �ڽ� ��ü ��������
        LasthitBar.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, -360.0f)); //���� �� ȸ�� ���� ����(0~360��)
    }

    void LastHitQuit() //��Ʈ ��Ʈ ����
    {
        Destroy(GO_LastHit); //��Ʈ ��Ʈ ������Ʈ ����
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

