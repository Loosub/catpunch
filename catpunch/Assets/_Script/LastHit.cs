using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LastHit : MonoBehaviour
{
    public int LastHitScore;
    public TextMeshPro STRING_LastHitScore;

    private string ColorArea; //원형 타이밍바 색상 영역

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Red")) ColorArea = "Red"; //화살표가 'Red'태그 오브젝트 영역에 있을 경우
        else if (collision.CompareTag("Orange")) ColorArea = "Orange"; //화살표가 'Orange'태그 오브젝트 영역에 있을 경우
        else if (collision.CompareTag("Yellow")) ColorArea = "Yellow"; //화살표가 'Yellow'태그 오브젝트 영역에 있을 경우
        else if (collision.CompareTag("Green")) ColorArea = "Green"; //화살표가 'Green'태그 오브젝트 영역에 있을 경우
    }

    public void LastHitScoreDecide() //라스트 히트 점수 측정
    {
        if (ColorArea == "Red") LastHitScore = 20;
        else if (ColorArea == "Orange") LastHitScore = 10;
        else if (ColorArea == "Yellow") LastHitScore = 5;
        else if (ColorArea == "Green") LastHitScore = 0;

        STRING_LastHitScore.text = LastHitScore.ToString(); //점수를 텍스트로 변환
    }

    public void LastHitArrow() //라스트 히트 화살표 회전
    {
        if (GameObject.FindWithTag("MonsterManager").GetComponent<MonsterManager>().LastHitStart) //LastHitStart가 true일 경우
        {
            transform.Rotate(Vector3.back * Time.deltaTime * 350.0f); //화살표가 Vector3.back을 기준으로 회전(회전속도 조절 가능)
        }
    }

    void Update()
    {
        LastHitArrow();
    }
}