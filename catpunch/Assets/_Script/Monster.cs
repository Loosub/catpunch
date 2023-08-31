using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JewelType
{
    Silver,Gold,Emerald,Diamond
}

[System.Serializable]
public class DropInfomation
{
    public int reachPoint;
    public int dropAmount;
    public JewelType DropJewel;

    public int GetPoint()
    {
        return reachPoint;
    }
    public int GetDropAmount()
    {
        return dropAmount;
    }
    public JewelType GetDropJewel()
    {
        return DropJewel;
    }
}

public class Monster : MonoBehaviour
{

    [SerializeField] private int Hp;
    [SerializeField] public List<DropInfomation> DropInfo;

    private GameObject GO_Player;
    public List<DropInfomation> GetDropInfo()
    {
        return DropInfo;
    }
    public int GetHp()
    {
        return Hp;
    }
    public void Animation_End_Spawn()
    {
        GO_Player.GetComponent<PlayerCharacter>().SetHitable(true);
    }

    void Start()
    {
        GO_Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        
    }
}
