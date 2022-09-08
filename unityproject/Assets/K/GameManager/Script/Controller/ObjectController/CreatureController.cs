using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Pathfinding;
using DG.Tweening;

public class CreatureController : ObjectController
{
    #region Ŭ���� INFO
    [SerializeField] protected Rigidbody2D rigid2D;
    [SerializeField] protected List<ControllerCollision> hitRanges;

    protected Enums.CreatureState animState;
    protected HashSet<Enums.CreatureState> buffStats;

    [SerializeField] private CreatureData info;
    public CreatureData Info => info;

    protected int currentHP;

    protected Vector2 inputDir;

    #region AI ���� ����

    /// <summary>
    /// Ÿ�ٰ��� �Ÿ��� ������� ������ ������.<br/>
    /// ���� ������. ��, targetDist * unitScale�� �Ÿ��� ����<br/>
    /// 0���� ���� ���� �� ������ ������� �ʴ´ٴ� �ǹ�
    /// </summary>
    protected float targetDist;

    /// <summary>
    /// Ÿ�ٰ� ������� �Ÿ����� ������ �� ������.<br/>
    /// ���� ������. ��, attackDist * unitScale�� �Ÿ��� ����<br/>
    /// 0���� ���� ���� �� ������ ������� �ʴ´ٴ� �ǹ�
    /// </summary>
    protected float attackDist;

    /// <summary>
    /// ���� : ȣ����<br/>
    /// ���� 0~1f<br/>
    /// ���� Ŭ���� ���� �õ��� �� ������.<br/>
    /// ���� �������� ���� ��� �� �� �Ÿ��� ������ �õ��� ������.<br/>
    /// ���� 0 �����̸� 0�� ����, �� 1 �̻��� 1�� �Ȱ��� ó����.
    /// </summary>
    protected float bellicosity;
    #region �н����ε�(Pathfinding) ���� ����
    /// <summary>
    /// ������ ���� ���� ���� ��θ� Ž�����ִ� ��ü.<br/>
    /// ���Ǵ� �޼ҵ� StartPath
    /// </summary>
    [SerializeField] protected Seeker seeker;

    protected float nextWaypointDistance = 1f;

    protected Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    #endregion

    #endregion


    #endregion


    #region �ڷ�ƾ ���� ���� ����
    protected IEnumerator attackCoolTimeCor;
    protected IEnumerator aiCor;
    protected IEnumerator pathUpdateCor;
    protected IEnumerator dashCor;
    #endregion

    [SerializeField] protected Transform showTr;


    #region ���� ��ġ ���� ��� ���(����) ����
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


    #region �ʱ�ȭ, ����
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
    /// AI�� �۵��Ѵ�.(��ã�⸸)<br/>
    /// ������ �Ұ����ϴ�. (�̵�, ���� ���)<br/>
    /// AI�� �ߴ��Ϸ��� OrderPathFindStop�� ȣ���� ��.
    /// </summary>
    /// <param name="targetTr"></param>
    public override void OrderPathFind(Transform targetTr)
    {

        // �� ���(�޼ҵ�)�� ��ã�⸸ �����ϹǷ� �� �ܿ� ������ �����ϵ��� �ϴ� ������ ���� off(= -1)��.
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


    #region Order�� ����Ʈ�ϴ� ���� ����

    #region ���� ���� ����

    protected void Attack(Vector2 dir)
    {
        animState = Enums.CreatureState.Attack;


        // ������ �̿��� ������ ������ �� �ִ���
        bool equipAttack = false;
        if(TryGetDecorator(Enums.Decorator.Equipment, out Component value))
        {
            var equipDecorator = value as EquipmentDecorator;
            equipAttack = equipDecorator.Attack(this, dir);
        }
        if (!equipAttack)
        {
            // ���̽� ���ݵ� Ȯ�强�� ���� �ܺη� �и�
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


    #region �̵� ���� ����
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


    #region AI ���� ����

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
    /// �н� ���δ� �ڷ�ƾ�� �ߴܽ�Ŵ.<br/>
    /// buffStats���� �н� ���δ� ������.
    /// </summary>
    protected void StopAI()
    {
        // �н� ���δ� Cor �ߴ� ���Ѿ���
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


            // ��밡 ���� ������ ����
            if (attackDist >= 0f && attackDist * GameManager.Instance.GameDB.UnitValueDB.UnitDist > dist)
            {
                
                var attackPossbility = Random.Range(0f, 1f);

                //����
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
                else // �̵�
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
    /// ���� ����(animState)�� Idle, Move�� �ƴ� ��� �ߴ�<br/>
    /// AICor �ڷ�ƾ �Լ����� ����
    /// </summary>
    /// <param name="targetTr"></param>
    /// <param name="dist"></param>
    protected void ChoiceMoveStyle(Transform targetTr, float dist)
    {
        if (animState != Enums.CreatureState.Idle && animState != Enums.CreatureState.Move) return;


        if (targetDist >= 0f && targetDist * GameManager.Instance.GameDB.UnitValueDB.UnitDist > dist)
        {
            //Ÿ�ٰ� �ݴ� �������� �̵�
            //���� ũ�⸦ 1�� ����� Move ȣ��
            Move((rigid2D.position - (Vector2)targetTr.position) / dist);
        }
        else
        {
            PathFinderMove();
        }
    }

    #region �н����δ��� ���Ǵ� ���� ����. ���� �׼ǿ� ���.
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
    /// ������ ��� Ž��<br/>
    /// targetTr : �i�� Ÿ��
    /// </summary>
    /// <param name="targetTr">�i�� Ÿ��</param>
    protected void UpdatePath(Transform targetTr)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigid2D.position, targetTr.position, OnPathComplete);
        }
    }

    protected void PathFinderMove()
    {
        // AI �̵� ���� ����
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


    #region Ǫ�� ���� ����(�������� ���ϴ� ���)

    protected IEnumerator PushCor(float timer)
    {
        yield return new WaitForSeconds(timer);
        animState = Enums.CreatureState.Idle;
    }

    #endregion

    #region �뽬 ���� ����(�뽬�� Ǫ���� ����. Ǫ���� ���ϴ� ��)

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

    #region ������ ����
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

