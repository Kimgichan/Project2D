using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ��� �����, ���÷� MinDamage���� �� ���� �������� ���� �� �ְ�, MaxDamage���� �� ���� �������� �������� �ִ�. �׳� �⺻ ����ġ�� �̷��ٰ� �����ϸ� �� ��.
/// 
/// ���� ���� Ŭ������ �����Ǹ� ������ ����.
/// </summary>
[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [SerializeField] private int hp;

    /// <summary>
    /// �ӵ� ����<br/>
    /// unitSpeed * speed
    /// </summary>
    [SerializeField] private float speed;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;

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


    /// <summary>
    /// �������� ����
    /// </summary>
    public float PushEnergy => push * GameManager.Instance.GameDB.UnitValueDB.UnitPush;
    #endregion



#if UNITY_EDITOR
    /// <summary>
    /// �� ����(����) �߿��� ������� �� ��, �� �� �ݵ�� #if UNITY_EDITOR�� �����ٰ�.
    /// ������ �� ����
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="speed"></param>
    /// <param name="minDamage"></param>
    /// <param name="maxDamage"></param>
    public void WriteData(int hp, float speed, int minDamage, int maxDamage, float push, float stunTimer)
    {
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");
        this.hp = hp;
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.push = push;
        this.stunTimer = stunTimer;
    }
#endif
}
