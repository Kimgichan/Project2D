using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EquipKind = Enums.EquipKind;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Object/Equip/Weapon", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private EquipKind kind;
    public EquipKind Kind
    {
        get
        {
            return kind;
        }
        set
        {
            Debug.LogError("경고: 인게임에서는 수치를 변경하면 안 되는 값(WeaponData.Kind)입니다.");
            kind = value;
        }
    }

    [SerializeField] private Vector2 damage; // x는 최소값, y는 최대값
    public float MinDamage => damage.x;
    public float MaxDamage => damage.y;
    public float ResultDamage => Random.Range(damage.x, damage.y);
    public Vector2 Damage
    {
        get => damage;
        set
        {
            Debug.LogError("경고: 인게임에서는 수치를 변경하면 안 되는 값(WeaponData.Damage)입니다.");
            damage = value;
        }
    }

    [SerializeField] private int reinforceMaxCount;
    public int ReinforceMaxCount
    {
        get => reinforceMaxCount;
        set
        {
            Debug.LogError("경고: 인게임에서는 수치를 변경하면 안 되는 값(WeaponData.ReinforceMaxCount)입니다.");
            reinforceMaxCount = value;
        }
    }


    //특성 언락하기 위한 최소 강화 갯수
    [SerializeField] private List<int> unlockAttributeCounts;
    public List<int> UnlockAttributeCounts_Warning
    {
        get
        {
            Debug.LogError("경고: 인게임에서는 수치를 변경하면 안 되는 값(WeaponData.UnlockAttributeCounts_Warning)입니다.");
            return unlockAttributeCounts;
        }
    }
    public int AttributeCount => unlockAttributeCounts.Count;
    public int UnlockRequireReinforceCount(int indx) => unlockAttributeCounts[indx];

    [SerializeField] private int baseRequireReinforceCount;
    public int BaseRequireReinforceCount
    {
        get => baseRequireReinforceCount;
        set
        {
            Debug.LogError("경고: 인게임에서는 수치를 변경하면 안 되는 값(WeaponData.BaseRequireReinforceCount)입니다.");
            baseRequireReinforceCount = value;
        }
    }
}
