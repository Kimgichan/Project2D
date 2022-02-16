using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidingState : IState
{
    //������������������������������������������������������������������������������������������������//
    //                                         ����                                           
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

            // �� ��ġ - ��� ��ġ = ��밡 �� ���� ����
            Vector3 dir = myCharacter.transform.position - myCharacter.GetotherColliderVector;

            if (dir.x >= 0.0f) nextVector.x = 1;
            else nextVector.x = -1;

            if (dir.y > 0.0f) nextVector.y = 1;
            else nextVector.y = -1;
        }    

        // Ÿ�̸Ӱ� ?�� �̻��� ���
        if(timer >= 3.0f)
        {
            avoidingStop = false;
            timer = 0.0f;
        }

        // �̵�
        myCharacter.myRigidbody.AddForce(nextVector * speed);

        // �ִϸ��̼�
        myCharacter.GetSpumPrefabs.PlayAnimation(5);
    }

    private Vector3[] directionPos =
{
       new Vector3(0,2,0),     //  ��
       new Vector3(2,0,0),     //  ��
       new Vector3(0,-2,0),    // ��
       new Vector3(-2,0,0),    // ��

       new Vector3(1,1,0),     //  ��
       new Vector3(1,-1,0),    //  ��
       new Vector3(-1,-1,0),   //  ��
       new Vector3(-1,1,0),    //  ��
    };
}
