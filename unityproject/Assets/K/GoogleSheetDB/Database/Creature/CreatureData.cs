using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ���� ������ ��� �����, ���÷� MinDamage���� �� ���� �������� ���� �� �ְ�, MaxDamage���� �� ���� �������� �������� �ִ�. �׳� �⺻ ����ġ�� �̷��ٰ� �����ϸ� �� ��.
/// 
/// ���� ���� Ŭ������ �����Ǹ� ������ ����.
/// </summary>
[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Controller/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [SerializeField] private int hp;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Enums.Effect baseAttack;

    [SerializeField] private float attackSpeed;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;

    /// <summary>
    /// ���� ����<br/>
    /// ������ �����ϴ� ������ �ƴ϶� ���� ������ ũ�⸦<br/>
    /// ��������� �� �������� �����ϴ� ����<br/>
    /// unitRange * range
    /// </summary>
    [SerializeField] private float attackRange;

    /// <summary>
    /// �о�� ������ ����<br/>
    /// unitPush * push
    /// </summary>
    [SerializeField] private float push;


    /// <summary>
    /// �������� �� ��밡 ���Ͽ� �ɸ��� ���� 1f -> 1��(s) 
    /// </summary>
    [SerializeField] private float stunTimer;

    #region ũ����(����) ���� API

    public Enums.Effect BaseAttack => baseAttack;

    /// <summary>
    /// HP => ����(Creature)�� MAX ü��.
    /// </summary>
    public int HP => hp;

    /// <summary>
    /// Speed => ������ �ӵ�, �� �ӵ��� �����ؼ� '�̵�(EX -> Speed*1.0f)', '�뽬(EX -> Speed*1.5f)', '���ݼӵ�(EX -> Speed*0.8f)'�� �� �����Ͻø� �� ��.
    /// </summary>
    public float MoveSpeed => moveSpeed * GameManager.Instance.GameDB.UnitValueDB.UnitSpeed;



    public float AttackSpeed => attackSpeed;

    /// <summary>
    /// MinDamage => ������ �⺻ �ּ� ���� ���ġ.
    /// </summary>
    public int MinDamage => minDamage;


    /// <summary>
    /// MaxDamage => ������ �⺻ �ִ� ���� ���ġ.
    /// </summary>
    public int MaxDamage => maxDamage;

    public int RandomDamage => Random.Range(MinDamage, MaxDamage + 1);


    /// <summary>
    /// �������� ����
    /// </summary>
    public float PushEnergy => push * GameManager.Instance.GameDB.UnitValueDB.UnitPush;

    public float AttackRange => attackRange * GameManager.Instance.GameDB.UnitValueDB.UnitRange;
    #endregion



#if UNITY_EDITOR
    /// <summary>
    /// �� ����(����) �߿��� ������� �� ��, �� �� �ݵ�� #if UNITY_EDITOR�� �����ٰ�.<br/>
    /// ������ �� ����<br/>
    /// 
    /// baseAttack : �ƹ��� ���Ⱑ ���� �� �ϴ� �⺻ ����
    /// </summary>
    /// <param name="baseAttack">��� ���ڷ������� ���Ⱑ �������� �ʾ��� ��<br/> 
    /// �⺻ ���� ���</param>
    /// <param name="hp"></param>
    /// <param name="speed"></param>
    /// <param name="minDamage"></param>
    /// <param name="maxDamage"></param>
    public void WriteData(Enums.Effect baseAttack, int hp, float moveSpeed, int minDamage, int maxDamage, float push, float stunTimer, float attackSpeed, float attackRange)
    {
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");
        this.baseAttack = baseAttack;
        this.hp = hp;
        this.moveSpeed = moveSpeed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.push = push;
        this.stunTimer = stunTimer;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }
#endif
}
