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
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(WeaponData.Kind)�Դϴ�.");
            kind = value;
        }
    }

    [SerializeField] private Vector2 damage; // x�� �ּҰ�, y�� �ִ밪
    public float MinDamage => damage.x;
    public float MaxDamage => damage.y;
    public float ResultDamage => Random.Range(damage.x, damage.y);
    public Vector2 Damage
    {
        get => damage;
        set
        {
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(WeaponData.Damage)�Դϴ�.");
            damage = value;
        }
    }

    [SerializeField] private int reinforceMaxCount;
    public int ReinforceMaxCount
    {
        get => reinforceMaxCount;
        set
        {
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(WeaponData.ReinforceMaxCount)�Դϴ�.");
            reinforceMaxCount = value;
        }
    }


    //Ư�� ����ϱ� ���� �ּ� ��ȭ ����
    [SerializeField] private List<int> unlockAttributeCounts;
    public List<int> UnlockAttributeCounts_Warning
    {
        get
        {
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(WeaponData.UnlockAttributeCounts_Warning)�Դϴ�.");
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
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(WeaponData.BaseRequireReinforceCount)�Դϴ�.");
            baseRequireReinforceCount = value;
        }
    }
}
