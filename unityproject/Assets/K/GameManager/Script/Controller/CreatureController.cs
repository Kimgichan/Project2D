using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Pathfinding;


public class CreatureController : ObjectController
{
    #region Ŭ���� INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerCollision> hitRanges;

    protected Enums.CreatureState animState;
    protected HashSet<Enums.CreatureState> buffStats;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    private int currentHP;

    protected Vector2 inputDir;


    #region �н����ε�(Pathfinding) ���� ����
    /// <summary>
    /// ������ ���� ���� ���� ��θ� Ž�����ִ� ��ü.<br/>
    /// ���Ǵ� �޼ҵ� StartPath
    /// </summary>
    [SerializeField] private Seeker seeker;

    private float nextWaypointDistance = 1f;

    private Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    #endregion
    #endregion


    #region �ڷ�ƾ ���� ���� ����
    private IEnumerator attackCoolTimeCor;
    private IEnumerator pathFinderCor;
    private IEnumerator pathUpdateCor;
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
        buffStats = new HashSet<Enums.CreatureState>();
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
        buffStats.Clear();

        if(attackCoolTimeCor != null)
        {
            StopCoroutine(attackCoolTimeCor);
        }
        attackCoolTimeCor = null;

        StopPathFinder();

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
                animState == Enums.CreatureState.Attack || buffStats.Contains(Enums.CreatureState.PathFind))
            {
                return;
            }
        }
        else
        {
            StopPathFinder();
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
        if((animState != Enums.CreatureState.Idle && animState != Enums.CreatureState.Move) || buffStats.Contains(Enums.CreatureState.PathFind))
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
            attackCoolTimeCor = AttackCoolTimeCor(3f / Info.Speed);
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

    public override void OrderPathFind(Transform targetTr)
    {
        if (!buffStats.Contains(Enums.CreatureState.PathFind))
        {
            buffStats.Add(Enums.CreatureState.PathFind);
        }

        if(pathFinderCor != null)
        {
            StopCoroutine(pathFinderCor);
        }
        pathFinderCor = PathFinderCor(targetTr);
        StartCoroutine(pathFinderCor);

        if(pathUpdateCor != null)
        {
            StopCoroutine(pathUpdateCor);
        }
        pathUpdateCor = PathUpdateCor(0.5f, targetTr);
        StartCoroutine(pathUpdateCor);
    }

    public override void OrderPathFindStop()
    {
        StopPathFinder();
    }
    #endregion


    #region Order�� ����Ʈ�ϴ� ���� ����

    #region ������
    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackCoolTimeCor(float cooltime)
    {
        //var timer = 3f / Info.Speed;

        yield return null;

        while(animState == Enums.CreatureState.Attack && cooltime > 0f)
        {
            cooltime -= Time.deltaTime;
            yield return null;
        }

        OrderAttackStop();
    }
    #endregion

    #region �н����δ��� ���Ǵ� ���� ����. ���� �׼ǿ� ���.
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private IEnumerator PathFinderCor(Transform targetTr)
    {
        while (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            yield return null;
            if (animState == Enums.CreatureState.Move ||
                animState == Enums.CreatureState.Idle)
            {
                if ((object)path == null)
                {
                    continue;
                }

                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    continue;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                Vector2 force = ((Vector2)path.vectorPath[currentWaypoint] - rigid2D.position).normalized;
                Vector2 dir = force.normalized;
                float distance = Vector2.Distance(path.vectorPath[currentWaypoint], rigid2D.position);

                rigid2D.velocity = dir * Speed * 0.5f;
                //rigid2D.AddForce(dir * Speed * Time.deltaTime);

                if(dir.x > 0.01f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if(dir.x < -0.01f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
        }

        StopPathFinder();
    }

    /// <summary>
    /// �н� ���δ� �ڷ�ƾ�� �ߴܽ�Ŵ.<br/>
    /// buffStats���� �н� ���δ� ������.
    /// </summary>
    private void StopPathFinder()
    {
        // �н� ���δ� Cor �ߴ� ���Ѿ���
        if (pathFinderCor != null)
        {
            StopCoroutine(pathFinderCor);
            pathFinderCor = null;
        }

        if(pathUpdateCor != null)
        {
            StopCoroutine(pathUpdateCor);
            pathUpdateCor = null;
        }

        if (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            buffStats.Remove(Enums.CreatureState.PathFind);
        }
    }


    private IEnumerator PathUpdateCor(float cooltime, Transform targetTr)
    {
        var wait = new WaitForSeconds(cooltime);
        UpdatePath(targetTr);
        while (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            yield return wait;
            UpdatePath(targetTr);
        }

        StopPathFinder();
    }


    /// <summary>
    /// ������ ��� Ž��<br/>
    /// targetTr : �i�� Ÿ��
    /// </summary>
    /// <param name="targetTr">�i�� Ÿ��</param>
    private void UpdatePath(Transform targetTr)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigid2D.position, targetTr.position, OnPathComplete);
        }
    }

    #endregion

    #endregion
}

