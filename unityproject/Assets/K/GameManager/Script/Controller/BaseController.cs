using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using OrderTitle = IController.OrderTitle;
using Order = IController.Order;

/// <summary>
/// ���� �⺻ Controller. 
/// 
/// ���ο� OrderAction�� �߰��ϰ� �ʹٸ� orderTable�� �߰��� ������Ѵ�.
/// orderTable�� BaseController������ ���� �����ϴ�.
/// </summary>
public class BaseController : MonoBehaviour, IController
{
    /// <summary>
    /// �� ������ ���� false�� OrderAction�Լ��� ������� ����
    /// </summary>
    protected bool ready;

    private static Dictionary<OrderTitle, UnityAction<BaseController, object>> orderTable = new Dictionary<OrderTitle, UnityAction<BaseController, object>>()
    {
        { OrderTitle.Idle, (con, parameter) => {con.OrderIdle((OrderParameters_Idle)parameter); } },
        { OrderTitle.Move, (con, parameter) => { con.OrderMove((OrderParameters_Move)parameter); } },
        { OrderTitle.Attack, (con, parameter) => { con.OrderAttack(); } },
        { OrderTitle.AttackStop, (con, parameter) => { con.OrderAttackStop(); } },
        { OrderTitle.Pushed, (con, parameter) => { con.OrderPushed((OrderParameters_Pushed)parameter); } },
        { OrderTitle.Shock, (con, parameter) => { con.OrderShock((OrderParameters_Shock)parameter); } },
        { OrderTitle.Damage, (con, parameter) => { con.OrderDamage((OrderParameters_Damage)parameter); } },
        { OrderTitle.Super, (con, parameter) => { con.OrderSuper((OrderParameters_Super)parameter); } },
    };

    public void Awake()
    {
        ready = false;
    }

    #region �������̽� API ������
    public void OrderAction(params Order[] orders)
    {
        if (!ready) return;


        for(int i = 0, icount = orders.Length; i<icount; i++)
        {
            if(orderTable.TryGetValue(orders[i].orderTitle, out UnityAction<BaseController, object> orderFunc))
            {
                orderFunc(this, orders[i].parameter);
            }
        }
    }
    public void OrderAction(List<Order> orders)
    {
        if (!ready) return;


        for (int i = 0, icount = orders.Count; i < icount; i++)
        {
            if (orderTable.TryGetValue(orders[i].orderTitle, out UnityAction<BaseController, object> orderFunc))
            {
                orderFunc(this, orders[i].parameter);
            }
        }
    }

    public GameObject GetGameObject() => gameObject;
    public Transform GetTransform() => transform;
    #endregion

    #region �ֹ� ���̺� ����ִ� �Լ� ������

    protected virtual void OrderIdle(OrderParameters_Idle parameter) {}

    protected virtual void OrderMove(OrderParameters_Move parameter) {}

    protected virtual void OrderAttack() {}

    protected virtual void OrderAttackStop() {}

    protected virtual void OrderPushed(OrderParameters_Pushed parameter) {}

    protected virtual void OrderShock(OrderParameters_Shock parameter) {}

    protected virtual void OrderDamage(OrderParameters_Damage parameter) {}
    
    protected virtual void OrderSuper(OrderParameters_Super parameter) {}
    #endregion

    #region Order �Ķ���� ����
    public struct OrderParameters_Move
    {
        //���� 1���Ϸ� ���� �� ��
        public Vector2 inputXY;
    }
    public struct OrderParameters_Idle
    {
        //������ Idle ���·� ���� ������
        //true�� � ���µ� ������ Idle�� ����� ������ ������ ��
        public bool compulsion;
    }
    public struct OrderParameters_Pushed
    {
        public Vector2 force;
    }
    public struct OrderParameters_Shock
    {
        public float timer;
    }
    public struct OrderParameters_Damage
    {
        public int damage;
    }
    public struct OrderParameters_Super
    {
        public float timer;
    }

    #endregion
}
