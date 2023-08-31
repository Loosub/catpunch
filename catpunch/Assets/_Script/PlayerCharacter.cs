using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class Jewel
{
    private int Amount;
    [SerializeField] private TextMeshProUGUI TMPro;
    public void add(int addAmount)
    {
        Amount += addAmount;
        TMPro.text = Amount.ToString();
    }
    public void subtract(int subtractAmount)
    {
        Amount -= subtractAmount;
        TMPro.text = Amount.ToString();
    }
}

public class PlayerCharacter : MonoBehaviour
{
    public int Level;
    public int Exp;
    public int Damage;
    public float Critical;
    public float JewelryDropFactor;
    public float AutoHitStartDelay;
    public float AutoHitSpeed;

    public GameObject GO_MonsterManager;
    public Animator animator;
    private bool Hitable;

    private float AutoHitTimer;
    private float NoAttackTimer;

    public Jewel Silver; //현재 실버 개수
    public Jewel Gold; //현재 골드 개수
    public Jewel Emerald; //현재 에메랄드 개수
    public Jewel Diamond; //현재 다이아몬드 개수



    void Start()
    {
        animator = this.GetComponent<Animator>();
        Hitable = true;
    }

    void Update()
    {
        NoAttackTimer += Time.deltaTime;
        AutoHitTimer += Time.deltaTime;

        if (NoAttackTimer >= AutoHitStartDelay)
        {
            if (AutoHitTimer >= AutoHitSpeed)
            {
                Attack();
            }
        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) //UI클릭 막기
        {
            if (Hitable)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        NoAttackTimer = 0;

        animator.SetTrigger("Hit");
        GO_MonsterManager.GetComponent<MonsterManager>().Attacked(Damage, Critical);
    }

    public void SetHitable(bool State)
    {
        Hitable = State;
    }

}
