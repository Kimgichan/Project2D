using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ���� ������ ��� �����, ���÷� MinDamage���� �� ���� �������� ���� �� �ְ�, MaxDamage���� �� ���� �������� �������� �ִ�. �׳� �⺻ ����ġ�� �̷��ٰ� �����ϸ� �� ��.
/// 
/// ���� ���� Ŭ������ �����Ǹ� ������ ����.
/// </summary>
[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [ReadOnly] [SerializeField] private int hp;

    [ReadOnly] [SerializeField] private Enums.Effect baseAttack;
    /// <summary>
    /// �ӵ� ����<br/>
    /// unitSpeed * speed
    /// </summary>
    [ReadOnly] [SerializeField] private float speed;
    [ReadOnly] [SerializeField] private int minDamage;
    [ReadOnly] [SerializeField] private int maxDamage;

    /// <summary>
    /// ���� ����<br/>
    /// ������ �����ϴ� ������ �ƴ϶� ���� ������ ũ�⸦<br/>
    /// ��������� �� �������� �����ϴ� ����<br/>
    /// unitRange * range
    /// </summary>
    [ReadOnly] [SerializeField] private float attackRange;

    /// <summary>
    /// �о�� ������ ����<br/>
    /// unitPush * push
    /// </summary>
    [ReadOnly] [SerializeField] private float push;


    /// <summary>
    /// �������� �� ��밡 ���Ͽ� �ɸ��� ���� 1f -> 1��(s) 
    /// </summary>
    [ReadOnly] [SerializeField] private float stunTimer;

    #region ũ����(����) ���� API

    public Enums.Effect BaseAttack;

    /// <summary>
    /// HP => ����(Creature)�� MAX ü��.
    /// </summary>
    public int HP => hp;

    /// <summary>
    /// Speed => ������ �ӵ�, �� �ӵ��� �����ؼ� '�̵�(EX -> Speed*1.0f)', '�뽬(EX -> Speed*1.5f)', '���ݼӵ�(EX -> Speed*0.8f)'�� �� �����Ͻø� �� ��.
    /// </summary>
    public float Speed => speed * GameManager.Instance.GameDB.UnitValueDB.UnitSpeed;



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
    public void WriteData(Enums.Effect baseAttack, int hp, float speed, int minDamage, int maxDamage, float push, float stunTimer)
    {
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");
        this.baseAttack = baseAttack;
        this.hp = hp;
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.push = push;
        this.stunTimer = stunTimer;
    }
#endif
}
