using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidingState : IState
{
    //￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣//
    //                                         변수                                           
    private UCharacter  myCharacter;
    private Vector3     nextVector;

    private float       timer = 0.0f;
    private bool        avoidingStop = false;
    private float       speed = 10.0f;
    //_______________________________________________________________________________________//


    public string getStringState()
    {
        return "Avoiding";
    }

    public AvoidingState(UCharacter _character)
    {
        SetParent(_character);
    }

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;
    }

    // Update is called once per frame
    public void Update()
    {
        timer += Time.deltaTime;

        if (!avoidingStop)
        {
            avoidingStop = true;

            // 내 위치 - 상대 위치 = 상대가 날 보는 방향
            Vector3 dir = myCharacter.transform.position - myCharacter.GetotherColliderVector;

            if (dir.x >= 0.0f) nextVector.x = 1;
            else nextVector.x = -1;

            if (dir.y > 0.0f) nextVector.y = 1;
            else nextVector.y = -1;
        }    

        // 타이머가 ?초 이상일 경우
        if(timer >= 3.0f)
        {
            avoidingStop = false;
            timer = 0.0f;
        }

        // 이동
        myCharacter.myRigidbody.AddForce(nextVector * speed);

        // 애니메이션
        myCharacter.GetSpumPrefabs.PlayAnimation(5);
    }

    private Vector3[] directionPos =
{
       new Vector3(0,2,0),     //  ↑
       new Vector3(2,0,0),     //  →
       new Vector3(0,-2,0),    // ↓
       new Vector3(-2,0,0),    // ←

       new Vector3(1,1,0),     //  ↗
       new Vector3(1,-1,0),    //  ↘
       new Vector3(-1,-1,0),   //  ↙
       new Vector3(-1,1,0),    //  ↖
    };
}
