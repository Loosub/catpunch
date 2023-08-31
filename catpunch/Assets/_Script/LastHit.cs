using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastHit : MonoBehaviour
{
    public int LastHitScore;
    public TextMeshPro STRING_LastHitScore;

    private string ColorArea; //���� Ÿ�ֹ̹� ���� ����

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Red")) ColorArea = "Red"; //ȭ��ǥ�� 'Red'�±� ������Ʈ ������ ���� ���
        else if (collision.CompareTag("Orange")) ColorArea = "Orange"; //ȭ��ǥ�� 'Orange'�±� ������Ʈ ������ ���� ���
        else if (collision.CompareTag("Yellow")) ColorArea = "Yellow"; //ȭ��ǥ�� 'Yellow'�±� ������Ʈ ������ ���� ���
        else if (collision.CompareTag("Green")) ColorArea = "Green"; //ȭ��ǥ�� 'Green'�±� ������Ʈ ������ ���� ���
    }

    public void LastHitScoreDecide() //��Ʈ ��Ʈ ���� ����
    {
        if (ColorArea == "Red") LastHitScore = 20;
        else if (ColorArea == "Orange") LastHitScore = 10;
        else if (ColorArea == "Yellow") LastHitScore = 5;
        else if (ColorArea == "Green") LastHitScore = 0;

        STRING_LastHitScore.text = LastHitScore.ToString(); //������ �ؽ�Ʈ�� ��ȯ
    }

    public void LastHitArrow() //��Ʈ ��Ʈ ȭ��ǥ ȸ��
    {
        if (GameObject.FindWithTag("MonsterManager").GetComponent<MonsterManager>().LastHitStart) //LastHitStart�� true�� ���
        {
            transform.Rotate(Vector3.back * Time.deltaTime * 350.0f); //ȭ��ǥ�� Vector3.back�� �������� ȸ��(ȸ���ӵ� ���� ����)
        }
    }

    void Update()
    {
        LastHitArrow();
    }
}