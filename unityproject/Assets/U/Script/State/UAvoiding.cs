using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidingState : IState
{
    //￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣//
    //                                         변수                                           
    private UCharacter  myCharacter;
    private Vector3     nextVector;

    private float       timer           = 0.0f;
    private bool        avoidingStop    = false;
    private float       speed           = 3.0f;
    private bool        isStateaChange  = true;

    //_______________________________________________________________________________________//

    public bool getStateChange()
    {
        return isStateaChange;
    }


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
            
            avoidingStop    = true;
            // 다른 상태로 변경하는 걸 막는 다.
            isStateaChange  = false;
            
            // 내 위치 - 상대 위치 = 상대가 날 보는 방향
            Vector3 dir = myCharacter.transform.position - myCharacter.GetotherColliderVector;

            if (dir.x >= 0.0f) nextVector.x = 1;
            else nextVector.x = -1;

            if (dir.y > 0.0f) nextVector.y = 1;
            else nextVector.y = -1;

          
            // 이동
            myCharacter.myRigidbody.AddForce(nextVector*speed, ForceMode2D.Impulse);

        }

        // 1초 이상일 경우 다른 상태변경을 허용
        if (timer >= 1.5f)
        {
            isStateaChange = true;
        }

        // 타이머가 ?초 이상일 경우
        if(timer >= 3.0f)
        {
            avoidingStop = false;
            timer = 0.0f;
        }

        
        // 애니메이션
        myCharacter.GetSpumPrefabs.PlayAnimation(3);
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
