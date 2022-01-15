using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;

public class UPlayer : UCharacter, IController
{
    private static Dictionary<string, UnityAction<UPlayer, List<string>>> actionDic = new Dictionary<string, UnityAction<UPlayer, List<string>>>()
    {
        //Idle�� valList �� State 
        { "Idle", (o, valList) => {o.ChangeState(new IdleState());  } },

        //Move�� valList �� State/InputX/InputY 
        { "Move", (o, valList) => {o.ChangeState(new MoveState()); } }


        //�� ������ ����


        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

        //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

        //���� ���� �� valList ����_State/�����ð�

        //��Ÿ���
    };

    public bool DoMove()
    {
        if (moveVector.x == 0f && moveVector.y == 0f) return false;
        else return true;
    }

    List<string> ReturnTheStateList(string _state)
    {
        List<string> action = new List<string>();

        string actionStr = _state;
        action.Add(actionStr);

        return action;
    }

    public void OrderAction(List<string> actionList)
    {
        foreach (var action in actionList)
        {
            var valList = new List<string>(action.Split('/'));
            if (actionDic.TryGetValue(valList[0], out UnityAction<UPlayer, List<string>> actionFunc))
            {
                actionFunc(this, valList);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();

        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if(DoMove())
        {
            // ���⿡ ���� ĳ���� �� ���� ���� ����
            if (moveVector.x > 0f)      spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if(moveVector.x < 0f)  spumPrefabs.transform.localScale = new Vector3(1, 1, 1);

            //ChangeState(new UMoveState());
            OrderAction(ReturnTheStateList("Move"));
        }
        else
        {
            //ChangeState(new UIdleState());
            OrderAction(ReturnTheStateList("Idle"));
        }

        base.Update();
    }
}
