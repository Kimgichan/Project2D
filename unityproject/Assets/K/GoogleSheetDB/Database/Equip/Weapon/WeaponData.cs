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

    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
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

    public void WriteData(EquipKind kind, int minDamage, int maxDamage,
        int reinforceMaxCount, List<int> unlockAttributeCounts, int baseRequireReinforceCount)
    {
        Debug.LogError("경고: 인게임에서 수치를 변경하면 안 되는 값들입니다.");
        this.kind = kind;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.reinforceMaxCount = reinforceMaxCount;
        this.unlockAttributeCounts = unlockAttributeCounts;
        this.baseRequireReinforceCount = baseRequireReinforceCount;
    }
}
