using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP => 생물(Creature)의 MAX 체력.
/// Speed => 생물의 속도, 이 속도를 참고해서 '이동(EX -> Speed*1.0f)', '대쉬(EX -> Speed*1.5f)', '공격속도(EX -> Speed*0.8f)'를 잘 조절하시면 될 듯.
/// MinDamage => 생물의 기본 최소 공격 기대치.
/// MaxDamage => 생물의 기본 최대 공격 기대치.
/// 여기 변수는 모두 참고용, 예시로 MinDamage보다 더 작은 데미지를 나갈 수 있고, MaxDamage보다 더 높게 데미지를 뽑을수도 있다. 그냥 기본 성능치가 이렇다고 참고하면 될 듯.
/// 
/// 좋은 참고 클래스가 구성되면 제시할 예정.
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
        Debug.LogError("경고: 인게임에서 수치를 변경하면 안 되는 값들입니다.");
        this.hp = hp;
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }
}
