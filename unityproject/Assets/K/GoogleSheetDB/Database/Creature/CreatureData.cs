using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP => ����(Creature)�� MAX ü��.
/// Speed => ������ �ӵ�, �� �ӵ��� �����ؼ� '�̵�(EX -> Speed*1.0f)', '�뽬(EX -> Speed*1.5f)', '���ݼӵ�(EX -> Speed*0.8f)'�� �� �����Ͻø� �� ��.
/// MinDamage => ������ �⺻ �ּ� ���� ���ġ.
/// MaxDamage => ������ �⺻ �ִ� ���� ���ġ.
/// ���� ������ ��� �����, ���÷� MinDamage���� �� ���� �������� ���� �� �ְ�, MaxDamage���� �� ���� �������� �������� �ִ�. �׳� �⺻ ����ġ�� �̷��ٰ� �����ϸ� �� ��.
/// 
/// ���� ���� Ŭ������ �����Ǹ� ������ ����.
/// </summary>
[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [SerializeField] private int hp;
    public int HP => hp;

    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    public int MinDamage => minDamage;
    public int MaxDamage => maxDamage;

    public void WriteData(int hp, float speed, int minDamage, int maxDamage)
    {
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");
        this.hp = hp;
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }
}
