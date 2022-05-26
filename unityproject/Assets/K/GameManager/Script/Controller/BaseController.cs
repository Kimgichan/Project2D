using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using OrderTitle = IController.OrderTitle;
using Order = IController.Order;

/// <summary>
/// 가장 기본 Controller. 
/// 
/// 새로운 OrderAction을 추가하고 싶다면 orderTable에 추가를 해줘야한다.
/// orderTable은 BaseController에서만 접근 가능하다.
/// </summary>
public class BaseController : MonoBehaviour, IController
{
    /// <summary>
    /// 이 변수의 값이 false면 OrderAction함수가 실행되지 않음
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

    #region 인터페이스 API 구현부
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

    #region 주문 테이블에 들어있는 함수 구현부

    protected virtual void OrderIdle(OrderParameters_Idle parameter) {}

    protected virtual void OrderMove(OrderParameters_Move parameter) {}

    protected virtual void OrderAttack() {}

    protected virtual void OrderAttackStop() {}

    protected virtual void OrderPushed(OrderParameters_Pushed parameter) {}

    protected virtual void OrderShock(OrderParameters_Shock parameter) {}

    protected virtual void OrderDamage(OrderParameters_Damage parameter) {}
    
    protected virtual void OrderSuper(OrderParameters_Super parameter) {}
    #endregion

    #region Order 파라미터 종류
    public struct OrderParameters_Move
    {
        //길이 1이하로 넘지 말 것
        public Vector2 inputXY;
    }
    public struct OrderParameters_Idle
    {
        //강제로 Idle 상태로 만들 것인지
        //true면 어떤 상태든 강제로 Idle로 만드는 로직을 구성할 것
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
