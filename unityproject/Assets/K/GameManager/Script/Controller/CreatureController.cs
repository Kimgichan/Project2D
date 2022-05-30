using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CreatureController : MonoBehaviour
{
    #region Ŭ���� INFO
    protected bool ready;

    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<CreatureTrigger> hitTriggers;
    [SerializeField] protected List<CreatureTrigger> attackTriggers;

    protected Enums.CreatureState animState;
    protected HashSet<Enums.CreatureState> buffStates;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    private int currentHP;

    protected Vector2 inputDir;
    #endregion

    #region �ڷ�ƾ ���� ���� ����
    private IEnumerator attackCoolTimeCor;
    #endregion

    #region ���� ��ġ ���� ��� ���(����) ����
    public virtual int CurrentHP => currentHP;
    public virtual float Speed => info.Speed;

    public virtual int MinDamage => info.MinDamage;
    public virtual int MaxDamage => info.MaxDamage;
    public virtual int RangeDamage => Random.Range(MinDamage, MaxDamage + 1);
    #endregion

    #region �ʱ�ȭ, ����
    protected virtual void Start()
    {
        InitColliders();

        //ready ���� true���� �ֹ�(order)�� ������
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

        if(attackCoolTimeCor != null)
        {
            StopCoroutine(attackCoolTimeCor);
        }
        attackCoolTimeCor = null;

        OrderIdle(true);
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
            attackTriggers[i].triggerEnter = AttackTriggerEnter;
            attackTriggers[i].gameObject.SetActive(false);
        }   
    }

    private void AttackTriggerEnter(Collider2D collision)
    {
        var col = collision.gameObject.GetComponent<CreatureTrigger>();
        if( col.Equals(this))
        {
            return;
        }

        //col.controller.OrderAction(new Order() { orderTitle =  });
    }
    #endregion

    #region ũ���İ� �����ϴ� �׼� �Լ� ���

    /// <summary>
    /// �⺻(Idle) ���� ���<para/>
    /// compulsion: true = ������ Idle�� ����
    /// </summary>
    /// <param name="compulsion">true = ������ Idle�� ����</param>
    public virtual void OrderIdle(bool compulsion)
    {
        inputDir = Vector2.zero;
        if (!compulsion)
        {
            if (animState == Enums.CreatureState.Dash ||
                animState == Enums.CreatureState.Attack)
            {
                return;
            }
        }
        rigid2D.velocity = Vector2.zero;
        animState = Enums.CreatureState.Idle;
    }

    /// <summary>
    /// �̵� ���
    /// </summary>
    /// <param name="dir">��� �������� �̵��� ��</param>
    public virtual void OrderMove(Vector2 dir)
    {
        inputDir = dir;
        if(animState != Enums.CreatureState.Idle && animState != Enums.CreatureState.Move)
        {
            return;
        }

        animState = Enums.CreatureState.Move;
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

    public virtual void OrderAttack()
    {
        if(animState == Enums.CreatureState.Dash || animState == Enums.CreatureState.Shock)
        {
            OrderAttackStop();
            return;
        }

        if(animState == Enums.CreatureState.Attack)
        {
            return;
        }

        animState = Enums.CreatureState.Attack;
        for(int i = 0, icount = attackTriggers.Count; i<icount; i++)
        {
            attackTriggers[i].gameObject.SetActive(true);
        }
    }

    public virtual void OrderAttackStop()
    {
        for (int i = 0, icount = attackTriggers.Count; i < icount; i++)
        {
            attackTriggers[i].gameObject.SetActive(false);
        }
    }

    public virtual void OrderDash()
    {
        if(animState != Enums.CreatureState.Move)
        {
            return;
        }

        animState = Enums.CreatureState.Dash;
    }

    public virtual void OrderPushed()
    {
        
    }

    public virtual void OrderShock()
    {
        
    }

    public virtual void OrderDamage()
    {
        
    }

    public virtual void OrderSuper()
    {

    }
    #endregion

    #region Order�� �����ϴ� ���� ����

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackCoolTimeCor()
    {
        var timer = 1f / Info.Speed;

        yield return null;

        while(animState == Enums.CreatureState.Attack && timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        animState = Enums.CreatureState.Idle;
        OrderAttackStop();
    }
    #endregion
}

