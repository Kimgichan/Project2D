using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Pathfinding;
using DG.Tweening;

public class CreatureController : ObjectController
{
    #region 클래스 INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerCollision> hitRanges;

    protected Enums.CreatureState animState;
    protected HashSet<Enums.CreatureState> buffStats;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    protected int currentHP;

    protected Vector2 inputDir;

    #region AI 관련 변수

    /// <summary>
    /// 타겟과의 거리를 어느정도 유지할 것인지.<br/>
    /// 배율 단위임. 즉, targetDist * unitScale로 거리를 정함<br/>
    /// 0보다 작은 값은 이 변수를 사용하지 않는다는 의미
    /// </summary>
    protected float targetDist;

    /// <summary>
    /// 타겟과 어느정도 거리에서 공격을 할 것인지.<br/>
    /// 배율 단위임. 즉, attackDist * unitScale로 거리를 정함<br/>
    /// 0보다 작은 값은 이 변수를 사용하지 않는다는 의미
    /// </summary>
    protected float attackDist;

    /// <summary>
    /// 직역 : 호전성<br/>
    /// 값은 0~1f<br/>
    /// 값이 클수록 공격 시도를 더 많이함.<br/>
    /// 값이 작을수록 공격 대신 좀 더 거리를 벌리는 시도를 많이함.<br/>
    /// 값이 0 이하이면 0과 같음, 값 1 이상은 1과 똑같이 처리됨.
    /// </summary>
    protected float bellicosity;
    #region 패스파인딩(Pathfinding) 관련 변수
    /// <summary>
    /// 가야할 곳의 가장 빠른 경로를 탐색해주는 객체.<br/>
    /// 사용되는 메소드 StartPath
    /// </summary>
    [SerializeField] protected Seeker seeker;

    protected float nextWaypointDistance = 1f;

    protected Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    #endregion

    #endregion


    #endregion


    #region 코루틴 관리 관련 변수
    protected IEnumerator attackCoolTimeCor;
    protected IEnumerator aiCor;
    protected IEnumerator pathUpdateCor;
    protected IEnumerator dashCor;
    #endregion

    [SerializeField] protected Transform showTr;


    #region 성능 수치 집계 방식 상속(변경) 가능
    public virtual int OriginalHP
    {
        get
        {
            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                return Info.HP + (decorator as EquipmentDecorator).AddOriginalHP;
            }
            return Info.HP;
        }
    }
    public virtual int CurrentHP 
    {
        get
        {
            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                return currentHP + (decorator as EquipmentDecorator).AddCurrentHP;
            }
            return currentHP;
        }
        set
        {
            if(value > OriginalHP)
            {
                currentHP = OriginalHP;
            }
            else if(value < 0)
            {
                currentHP = 0;
            }
            else
            {
                currentHP = value;
            }
        }
    }
    public virtual float MoveSpeed => info.MoveSpeed;
    public virtual float AttackSpeed
    {
        get
        {
            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                return info.AttackSpeed * (decorator as EquipmentDecorator).AttackSpeed;
            }

            return info.AttackSpeed;
        }
    }
    //public virtual int MinDamage => info.MinDamage;
    //public virtual int MaxDamage => info.MaxDamage;
    //public virtual int RandomDamage => Random.Range(MinDamage, MaxDamage + 1);

    public virtual int MinDamage
    {
        get
        {
            var val = 0;

            if (TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                var equip = decorator as EquipmentDecorator;
                val = equip.MinDamage;
            }
            else
            {
                val = Info.MinDamage;
            }

            return val;
        }
    }

    public virtual int MaxDamage
    {
        get
        {
            var val = 0;

            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                var equip = decorator as EquipmentDecorator;
                val = equip.MaxDamage;
            }
            else
            {
                val = Info.MaxDamage;
            }

            return val;
        }
    }

    public virtual int RandomDamage
    {
        get
        {
            var val = 0;

            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                var equip = decorator as EquipmentDecorator;
                val = equip.RandomDamage;
            }
            else
            {
                val = Info.RandomDamage;
            }

            return val;
        }
    }

    public virtual float PushEnergy
    {
        get
        {
            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                return info.PushEnergy * (decorator as EquipmentDecorator).PushEnergy;
            }

            return info.PushEnergy;
        }
    }

    public virtual float AttackRange
    {
        get
        {
            if(TryGetDecorator(Enums.Decorator.Equipment, out Component decorator))
            {
                return info.AttackRange * (decorator as EquipmentDecorator).AttackRange;
            }

            return info.AttackRange;
        }
    }

    public virtual float Dash
    {
        get => 4.5f;
    }
    #endregion


    #region 초기화, 셋팅
    protected override void Start()
    {
        buffStats = new HashSet<Enums.CreatureState>();
        CurrentHP = OriginalHP;
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
    protected IEnumerator StartCor()
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

    protected void CreatureReset()
    {
        animState = Enums.CreatureState.Idle;
        currentHP = info.HP;
        buffStats.Clear();

        if(attackCoolTimeCor != null)
        {
            StopCoroutine(attackCoolTimeCor);
        }
        attackCoolTimeCor = null;

        StopAI();

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

            if (animState == Enums.CreatureState.Push)
            {
                return;
            }
        }
        else
        {
            StopAI();
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

        if(animState == Enums.CreatureState.Push || animState == Enums.CreatureState.Dash)
        {
            return;
        }

        Move(dir);
    }

    public override void OrderAttack(Vector2 dir)
    {
        if(animState == Enums.CreatureState.Dash || animState == Enums.CreatureState.Shock || animState == Enums.CreatureState.Push)
        {
            OrderAttackStop();
            return;
        }

        if(animState == Enums.CreatureState.Attack || buffStats.Contains(Enums.CreatureState.PathFind))
        {
            return;
        }

        Attack(dir);
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
        if(animState == Enums.CreatureState.Push || animState == Enums.CreatureState.Dash || animState == Enums.CreatureState.Attack)
        {
            return;
        }


        if (inputDir == Vector2.zero) return;

        animState = Enums.CreatureState.Dash;

        if(dashCor != null)
        {
            StopCoroutine(dashCor);
        }
        dashCor = DashCor(GameManager.Instance.GameDB.UnitValueDB.UnitDashTime, inputDir);
        StartCoroutine(dashCor);
    }

    public override void OrderPushed(Vector2 force)
    {
        if(dashCor != null)
        {
            StopCoroutine(dashCor);
            dashCor = null;
        }

        rigid2D.velocity = Vector2.zero;

        animState = Enums.CreatureState.Push;
        StartCoroutine(PushCor(1f));
        rigid2D.AddForce(force);
    }

    public override void OrderShock()
    {
        
    }

    public override void OrderDamage(int damage)
    {
        DamageUpdate(damage);
    }

    public override void OrderSuper()
    {

    }

    public override void OrderDestroy()
    {

    }

    public override void OrderXFlip(bool flip)
    {
        if (flip)
        {
            showTr.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            showTr.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    /// <summary>
    /// AI가 작동한다.(길찾기만)<br/>
    /// 조작이 불가능하다. (이동, 공격 등등)<br/>
    /// AI를 중단하려면 OrderPathFindStop을 호출할 것.
    /// </summary>
    /// <param name="targetTr"></param>
    public override void OrderPathFind(Transform targetTr)
    {

        // 이 명령(메소드)은 길찾기만 제공하므로 그 외에 역할을 수행하도록 하는 변수는 전부 off(= -1)함.
        attackDist = -1f;
        targetDist = -1f;
        bellicosity = -1f;

        PlayAI(targetTr);
    }

    public override void OrderPathFindStop()
    {
        StopAI();
    }


    public override void OrderSetAI_Style(float attackDist, float targetDist, float bellicosity)
    {
        this.attackDist = attackDist;
        this.targetDist = targetDist;
        this.bellicosity = bellicosity;
    }

    public override void OrderPlayAI(Transform targetTr, float attackDist, float targetDist, float bellicosity)
    {
        OrderSetAI_Style(attackDist, targetDist, bellicosity);

        PlayAI(targetTr);
    }

    public override void OrderStopAI()
    {
        StopAI();
    }
    #endregion


    #region Order를 서포트하는 로직 조각

    #region 공격 로직 조각

    protected void Attack(Vector2 dir)
    {
        animState = Enums.CreatureState.Attack;


        // 도구를 이용한 공격을 진행할 수 있는지
        bool equipAttack = false;
        if(TryGetDecorator(Enums.Decorator.Equipment, out Component value))
        {
            var equipDecorator = value as EquipmentDecorator;
            equipAttack = equipDecorator.Attack(this, dir);
        }
        if (!equipAttack)
        {
            // 베이스 공격도 확장성을 위해 외부로 분리
            GameManager.Instance.ControllerManager.CreatureEffectPlay(this, dir);

            if (attackCoolTimeCor == null)
            {
                attackCoolTimeCor = AttackCoolTimeCor(1f / AttackSpeed);
            }
            StartCoroutine(attackCoolTimeCor);
        }

        //var shock = GameManager.Instance.EffectManager.Pop(Enums.Effect.Shock_Base) as Shock;
        //shock.gameObject.SetActive(true);
        //shock.Play(this, new Vector3(rigid2D.position.x, rigid2D.position.y, -10f), 1.2f, 1f, (hitTarget) =>
        //{
        //    hitTarget.OrderPushed(dir.normalized * Info.PushEnergy);
        //});
    }

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


    #region 이동 로직 조각
    protected void Move(Vector2 inputDir)
    {
        animState = Enums.CreatureState.Move;
        rigid2D.velocity = inputDir * MoveSpeed;
        if (inputDir.x > 0.01f)
        {
            //transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            OrderXFlip(false);
        }
        else if (inputDir.x < -0.01f)
        {
            //transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            OrderXFlip(true);
        }
    }
    #endregion


    #region AI 로직 조각

    protected void PlayAI(Transform targetTr)
    {
        if (!buffStats.Contains(Enums.CreatureState.PathFind))
        {
            buffStats.Add(Enums.CreatureState.PathFind);
        }

        if (aiCor != null)
        {
            StopCoroutine(aiCor);
        }
        aiCor = AICor(targetTr);
        StartCoroutine(aiCor);

        if (pathUpdateCor != null)
        {
            StopCoroutine(pathUpdateCor);
        }
        pathUpdateCor = PathUpdateCor(0.5f, targetTr);
        StartCoroutine(pathUpdateCor);
    }


    /// <summary>
    /// 패스 파인더 코루틴을 중단시킴.<br/>
    /// buffStats에서 패스 파인더 제거함.
    /// </summary>
    protected void StopAI()
    {
        // 패스 파인더 Cor 중단 시켜야함
        if (aiCor != null)
        {
            StopCoroutine(aiCor);
            aiCor = null;
        }

        if (pathUpdateCor != null)
        {
            StopCoroutine(pathUpdateCor);
            pathUpdateCor = null;
        }

        if (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            buffStats.Remove(Enums.CreatureState.PathFind);
        }
    }

    protected IEnumerator AICor(Transform targetTr)
    {
        while (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            yield return null;

            if (animState == Enums.CreatureState.Push)
            {
                continue;
            }

            var dist = Vector2.Distance(rigid2D.position, targetTr.position);


            // 상대가 공격 범위에 닿음
            if (attackDist >= 0f && attackDist * GameManager.Instance.GameDB.UnitValueDB.UnitDist > dist)
            {
                
                var attackPossbility = Random.Range(0f, 1f);

                //공격
                if(bellicosity >= attackPossbility) 
                {
                    if (animState == Enums.CreatureState.Dash || animState == Enums.CreatureState.Shock)
                    {
                        OrderAttackStop();
                        continue;
                    }

                    if (animState == Enums.CreatureState.Attack)
                    {
                        continue;
                    }


                    Attack(((Vector2)targetTr.position - rigid2D.position) / dist);
                }
                else // 이동
                {
                    ChoiceMoveStyle(targetTr, dist);
                }
            }
            else
            {
                ChoiceMoveStyle(targetTr, dist);
            }
        }

        StopAI();
    }


    /// <summary>
    /// 메인 상태(animState)가 Idle, Move가 아닐 경우 중단<br/>
    /// AICor 코루틴 함수에서 사용됌
    /// </summary>
    /// <param name="targetTr"></param>
    /// <param name="dist"></param>
    protected void ChoiceMoveStyle(Transform targetTr, float dist)
    {
        if (animState != Enums.CreatureState.Idle && animState != Enums.CreatureState.Move) return;


        if (targetDist >= 0f && targetDist * GameManager.Instance.GameDB.UnitValueDB.UnitDist > dist)
        {
            //타겟과 반대 방향으로 이동
            //벡터 크기를 1로 만들고 Move 호출
            Move((rigid2D.position - (Vector2)targetTr.position) / dist);
        }
        else
        {
            PathFinderMove();
        }
    }

    #region 패스파인더가 사용되는 로직 조각. 추적 액션에 사용.
    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    protected IEnumerator PathUpdateCor(float cooltime, Transform targetTr)
    {
        var wait = new WaitForSeconds(cooltime);
        UpdatePath(targetTr);
        while (buffStats.Contains(Enums.CreatureState.PathFind))
        {
            yield return wait;
            UpdatePath(targetTr);
        }

        StopAI();
    }


    /// <summary>
    /// 최적의 경로 탐색<br/>
    /// targetTr : 쫒는 타겟
    /// </summary>
    /// <param name="targetTr">쫒는 타겟</param>
    protected void UpdatePath(Transform targetTr)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigid2D.position, targetTr.position, OnPathComplete);
        }
    }

    protected void PathFinderMove()
    {
        // AI 이동 관련 로직
        if ((object)path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 force = ((Vector2)path.vectorPath[currentWaypoint] - rigid2D.position).normalized;
        Vector2 dir = force.normalized;
        float distance = Vector2.Distance(path.vectorPath[currentWaypoint], rigid2D.position);

        Move(dir);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }



    #endregion




    #endregion


    #region 푸쉬 로직 조각(밀쳐짐을 당하는 경우)

    protected IEnumerator PushCor(float timer)
    {
        yield return new WaitForSeconds(timer);
        animState = Enums.CreatureState.Idle;
    }

    #endregion

    #region 대쉬 로직 조각(대쉬와 푸쉬의 차이. 푸쉬는 당하는 것)

    protected IEnumerator DashCor(float timer, Vector2 inputDir)
    {
        yield return null;

        var dir = inputDir.normalized;

        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            rigid2D.velocity = dir * MoveSpeed * Dash;
        }

        animState = Enums.CreatureState.Idle;
        dashCor = null;
    }

    #endregion

    #region 데미지 로직
    protected void DamageUpdate(int damage)
    {
        if (damage <= 0) return;

        if(TryGetDecorator(Enums.Decorator.Equipment, out Component decoratorEquip))
        {
            var equipment = decoratorEquip as EquipmentDecorator;

            var addHP = equipment.AddCurrentHP;
            equipment.AddCurrentHP -= damage;
            damage -= addHP;
        }

        if (damage > 0)
        {
            CurrentHP -= damage;
        }

        if(TryGetDecorator(Enums.Decorator.HUD, out Component decoratorHUD))
        {
            var hud = decoratorHUD as HUDDecorator;
            hud.ShowHP(this);
        }

        var damageText = GameManager.Instance.EffectManager.Pop(Enums.Effect.DamageText) as DamageText;
        damageText.gameObject.SetActive(true);
        damageText.Play(transform.position, damage);
    }
    #endregion

    #endregion
}

