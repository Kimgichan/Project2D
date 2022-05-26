using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using OrderTitle = IController.OrderTitle;
using Order = IController.Order;

public class CreaturController : BaseController
{
    #region 클래스 INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerTrigger> hitTriggers;
    [SerializeField] protected List<ControllerTrigger> attackTriggers;

    protected OrderTitle animState;
    protected HashSet<OrderTitle> buffStates;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    private int currentHP;

    protected Vector2 inputDir;
    #endregion

    #region 성능 수치 집계 방식 상속(변경) 가능
    public virtual int CurrentHP => currentHP;
    public virtual float Speed => info.Speed;

    public virtual int MinDamage => info.MinDamage;
    public virtual int MaxDamage => info.MaxDamage;
    public virtual int RangeDamage => Random.Range(MinDamage, MaxDamage + 1);
    #endregion

    #region 초기화, 셋팅
    protected virtual void Start()
    {
        InitColliders();

        //ready 값이 true여야 주문(order)을 수행함
        ready = true;

        CreatureReset();
    }
    
    protected virtual void OnEnable()
    {
        if (ready)
        {
            CreatureReset();
        }
    }

    private void CreatureReset()
    {
        currentHP = info.HP;
        OrderIdle(new OrderParameters_Idle() { compulsion = true });
    }


    protected void InitColliders()
    {
        for(int i = 0, icount = hitTriggers.Count; i<icount; i++)
        {
            hitTriggers[i].controller = this;
        }

        for(int i = 0, icount = attackTriggers.Count; i<icount; i++)
        {
            attackTriggers[i].controller = this;
        }   
    }
    #endregion

    #region 주문 테이블에 들어있는 함수 구현부
    protected override void OrderIdle(OrderParameters_Idle parameter)
    {
        inputDir = Vector2.zero;
        if (!parameter.compulsion)
        {
            if (animState == OrderTitle.AttackDash || animState == OrderTitle.Dash ||
                animState == OrderTitle.Attack)
            {
                return;
            }
        }
        rigid2D.velocity = Vector2.zero;
        animState = OrderTitle.Idle;
    }
    protected override void OrderMove(OrderParameters_Move parameter)
    {
        inputDir = parameter.inputXY;
        if(animState != OrderTitle.Idle && animState != OrderTitle.Move)
        {
            return;
        }

        rigid2D.velocity = inputDir * Speed;
        if(inputDir.x > 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if(inputDir.x < 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    protected override void OrderAttack()
    {
        
    }

    protected override void OrderAttackStop()
    {
        
    }

    protected override void OrderPushed(OrderParameters_Pushed parameter)
    {
        
    }

    protected override void OrderShock(OrderParameters_Shock parameter)
    {
        
    }

    protected override void OrderDamage(OrderParameters_Damage parameter)
    {
        
    }

    protected override void OrderSuper(OrderParameters_Super parameter)
    {

    }
    #endregion
}
