using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;
using Order = IController.Order;
using OrderTitle = IController.OrderTitle;

public class UPlayer : UCharacter, IController
{
    private OrderTitle animState;
    private HashSet<OrderTitle> buffStates;

    public GameObject gunAim;

    void gunAimPlayerFollow()
    {
        gunAim.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private static Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>>()
    {
        //Idle�� valList �� State 
        { OrderTitle.Idle, (o, valList) =>       {o.GetSpumPrefabs.PlayAnimation(0); } },

        //Move�� valList �� State/InputX/InputY 
        { OrderTitle.Move, (o, valList) =>
            {
             /* o.ChangeState(new MoveState(me));*/
             // ĳ���� �̵�
             o.transform.Translate(o.GetMoveVector * o.GetSpeed * Time.deltaTime);
             // �ִϸ��̼� ���
             o.GetSpumPrefabs.PlayAnimation(1);
            }
        },

        { OrderTitle.Attack,(o, valList) =>
            {

            }
        }

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

    public void OrderAction(params Order[] orders)
    {

        foreach (var order in orders)
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    public void OrderAction(List<Order> orders)
    {

        foreach (var order in orders)
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        boxCollider2D = GetComponent<BoxCollider2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
    }

    protected override void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if (DoMove())
        {
            // ���⿡ ���� ĳ���� �� ���� ���� ����
            if (moveVector.x > 0f) spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if (moveVector.x < 0f) spumPrefabs.transform.localScale = new Vector3(1, 1, 1);

            OrderAction(new Order() { orderTitle = OrderTitle.Move });
            //OrderAction(new Order() { orderTitle = "move", parameters = new List<object>() { 1.0f, 1.0f } });
        }
        else
        {
            OrderAction(new Order() { orderTitle = OrderTitle.Idle });
        }

        //GunAim�� ĳ���� ��ġ���� ������ ������Ʈ.
        gunAimPlayerFollow();

    }
}
