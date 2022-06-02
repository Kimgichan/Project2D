using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CreatureController : ObjectController
{
    #region Ŭ���� INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerCollision> hitRanges;

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
    protected override void Start()
    {
        StartCoroutine(StartCor());
    }
    
    protected override void OnEnable()
    {
        if (kind != Enums.ControllerKind.None)
        {
            CreatureReset();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCor()
    {
        for (int i = 0, icount = hitRanges.Count; i < icount; i++) 
        {
            while(hitRanges[i].GetCollider() == null)
            {
                yield return null;
            }

            hitRanges[i].controller = this;
            if (i > 0)
            {
                Physics2D.IgnoreCollision(hitRanges[0].GetCollider(), hitRanges[i].GetCollider());
            }
        }

        CreatureReset();

        kind = Enums.ControllerKind.Creature;
    }

    private void CreatureReset()
    {
        animState = Enums.CreatureState.Idle;
        currentHP = info.HP;

        if(attackCoolTimeCor != null)
        {
            StopCoroutine(attackCoolTimeCor);
        }
        attackCoolTimeCor = null;

        OrderIdle(true);
    }
    #endregion


    #region ũ���İ� �����ϴ� �׼� �Լ� ���

    /// <summary>
    /// �⺻(Idle) ���� ���<para/>
    /// compulsion: true = ������ Idle�� ����
    /// </summary>
    /// <param name="compulsion">true = ������ Idle�� ����</param>
    public override void OrderIdle(bool compulsion)
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
    public override void OrderMove(Vector2 dir)
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

    public override void OrderAttack(Vector2 dir)
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


        HashSet<ObjectController> hitTarget = new HashSet<ObjectController>();
        for(int i = 0, icount = hitRanges.Count; i<icount; i++)
        {
            var colls = hitRanges[i].CurrentCheckTrigger();
            for(int j = 0, jcount = colls.Length; j<jcount; j++)
            {
                var controllerColl = colls[j].GetComponent<ControllerCollision>();
                if (controllerColl == null || hitTarget.Contains(controllerColl.controller) || controllerColl.controller == this)
                {
                    continue;
                }

                var pushEnergy = Info.PushEnergy;
                hitTarget.Add(controllerColl.controller);
                controllerColl.controller.OrderPushed(((Vector2)controllerColl.controller.transform.position - (Vector2)transform.position).normalized * pushEnergy);
            }
        }

        if(attackCoolTimeCor == null)
        {
            attackCoolTimeCor = AttackCoolTimeCor();
        }
        StartCoroutine(attackCoolTimeCor);
    }

    public override void OrderAttackStop()
    {
        if(attackCoolTimeCor != null)
        {
            StopCoroutine(attackCoolTimeCor);
            attackCoolTimeCor = null;
        }

        if (animState == Enums.CreatureState.Attack)
        {
            animState = Enums.CreatureState.Idle;
            OrderIdle(false);
        }
    }

    public override void OrderDash()
    {
        if(animState != Enums.CreatureState.Move)
        {
            return;
        }

        animState = Enums.CreatureState.Dash;
    }

    public override void OrderPushed(Vector2 force)
    {
        rigid2D.AddForce(force);
    }

    public override void OrderShock()
    {
        
    }

    public override void OrderDamage()
    {
        
    }

    public override void OrderSuper()
    {

    }
    #endregion


    #region Order�� ����Ʈ�ϴ� ���� ����

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackCoolTimeCor()
    {
        var timer = 3f / Info.Speed;

        yield return null;

        while(animState == Enums.CreatureState.Attack && timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        OrderAttackStop();
    }
    #endregion
}

