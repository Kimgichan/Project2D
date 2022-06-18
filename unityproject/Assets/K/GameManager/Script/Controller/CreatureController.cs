using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Pathfinding;


public class CreatureController : ObjectController
{
    #region 클래스 INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerCollision> hitRanges;

    protected Enums.CreatureState animState;
    protected HashSet<Enums.CreatureState> buffStats;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    private int currentHP;

    protected Vector2 inputDir;


    #region 패스파인딩(Pathfinding) 관련 변수
    /// <summary>
    /// 가야할 곳의 가장 빠른 경로를 탐색해주는 객체.<br/>
    /// 사용되는 메소드 StartPath
    /// </summary>
    [SerializeField] private Seeker seeker;

    private float nextWaypointDistance = 1f;

    private Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    #endregion
    #endregion


    #region 코루틴 관리 관련 변수
    private IEnumerator attackCoolTimeCor;
    private IEnumerator pathFinderCor;
    private IEnumerator pathUpdateCor;
    #endregion


    #region 성능 수치 집계 방식 상속(변경) 가능
    public virtual int CurrentHP => currentHP;
    public virtual float Speed => info.Speed;

    public virtual int MinDamage => info.MinDamage;
    public virtual int MaxDamage => info.MaxDamage;
    public virtual int RangeDamage => Random.Range(MinDamage, MaxDamage + 1);
    #endregion


    #region 초기화, 셋팅
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


    #region 크리쳐가 제공하는 액션 함수 목록

    /// <summary>
    /// 기본(Idle) 상태 명령<para/>
    /// compulsion: true = 강제로 Idle로 변경
    /// </summary>
    /// <param name="compulsion">true = 강제로 Idle로 변경</param>
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
    /// 이동 명령
    /// </summary>
    /// <param name="dir">어느 방향으로 이동할 지</param>
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


    #region Order를 서포트하는 로직 조각

    #region 공격쪽
    /// <summary>
    /// 공격 쿨타임
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

    #region 패스파인더가 사용되는 로직 조각. 추적 액션에 사용.
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
    /// 패스 파인더 코루틴을 중단시킴.<br/>
    /// buffStats에서 패스 파인더 제거함.
    /// </summary>
    private void StopPathFinder()
    {
        // 패스 파인더 Cor 중단 시켜야함
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
    /// 최적의 경로 탐색<br/>
    /// targetTr : 쫒는 타겟
    /// </summary>
    /// <param name="targetTr">쫒는 타겟</param>
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

