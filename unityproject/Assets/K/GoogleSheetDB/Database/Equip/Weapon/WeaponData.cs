using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EquipKind = Enums.EquipKind;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Object/Equip/Weapon", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private EquipKind kind;
    public EquipKind Kind => kind;

    [SerializeField] private Enums.Effect attackEffect;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;


    public Enums.Effect AttackEffect => attackEffect;
    public int MinDamage => minDamage;
    public int MaxDamage => maxDamage;
    public int RandomDamage => Random.Range(minDamage, maxDamage);

    [SerializeField] private int reinforceMaxCount;
    public int ReinforceMaxCount => reinforceMaxCount;

    //특성 언락하기 위한 최소 강화 갯수
    [SerializeField] private List<int> unlockAttributeCounts;
    public int AttributeCount => unlockAttributeCounts.Count;
    public int UnlockRequireReinforceCount(int indx) => unlockAttributeCounts[indx];



    // 이거 이름바꿀 필요 있어보임
    [SerializeField] private int baseRequireReinforceCount;
    public int BaseRequireReinforceCount => baseRequireReinforceCount;


#if UNITY_EDITOR
    /// <summary>
    /// 인 게임(빌드) 중에는 사용하지 말 것, 쓸 때 반드시 #if UNITY_EDITOR로 감싸줄것.
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="minDamage"></param>
    /// <param name="maxDamage"></param>
    /// <param name="reinforceMaxCount"></param>
    /// <param name="unlockAttributeCounts"></param>
    /// <param name="baseRequireReinforceCount"></param>
    public void WriteData(EquipKind kind, Enums.Effect attackEffect, int minDamage, int maxDamage,
        int reinforceMaxCount, List<int> unlockAttributeCounts, int baseRequireReinforceCount)
    {
        Debug.LogError("경고: 인게임에서 수치를 변경하면 안 되는 값들입니다.");
        this.kind = kind;
        this.attackEffect = attackEffect;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.reinforceMaxCount = reinforceMaxCount;
        this.unlockAttributeCounts = unlockAttributeCounts;
        this.baseRequireReinforceCount = baseRequireReinforceCount;
    }
#endif
}
