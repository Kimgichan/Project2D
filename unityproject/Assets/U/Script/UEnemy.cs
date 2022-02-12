using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;

public class UEnemy : UCharacter, IController
{
    private static Dictionary<string, UnityAction<UEnemy, List<object>>> actionDic = new Dictionary<string, UnityAction<UEnemy, List<object>>>()
    {
        //Idle�� valList �� State 
        { "Idle", (o, valList) => {o.ChangeState(new IdleState());  } },

        //Move�� valList �� State/InputX/InputY 
        { "Move", (o, valList) => {o.ChangeState(new MoveState()); } },

        { "Follow", (o, valList) => {o.ChangeState(new FollowState()); } },

        { "Attack", (o, valList) => {o.ChangeState(new AttackState()); } }

        //�� ������ ����


        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

        //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

        //���� ���� �� valList ����_State/�����ð�

        //��Ÿ���
    };

    List<string> ReturnTheStateList(string _state)
    {
        List<string> action = new List<string>();

        string actionStr = _state;
        action.Add(actionStr);

        return action;
    }

    public void OrderAction(params object[] orders)
    {
        foreach (var order in orders as IController.Order[])
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemy, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    protected override void Start()
    {

        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();

        ChangeState(new IdleState());
    }

    // �浹 ���� �ȿ� ���� ����
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            boxColliderVector = other.transform.position;
            float dis = Vector3.Distance(boxColliderVector, transform.position);

            // ������ �Ÿ�
            if (dis < 1.0f) OrderAction(ReturnTheStateList("Attack"));
            else            OrderAction(ReturnTheStateList("Follow"));
        }
    }
    // �浹 �������� ���� ����
    public void OnTriggerExit2D(Collider2D other)
    {
        OrderAction(ReturnTheStateList("Idle"));
    }

    protected override void Update()
    {
        base.Update();
    }
}
