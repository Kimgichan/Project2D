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

    //Ư�� ����ϱ� ���� �ּ� ��ȭ ����
    [SerializeField] private List<int> unlockAttributeCounts;
    public int AttributeCount => unlockAttributeCounts.Count;
    public int UnlockRequireReinforceCount(int indx) => unlockAttributeCounts[indx];



    // �̰� �̸��ٲ� �ʿ� �־��
    [SerializeField] private int baseRequireReinforceCount;
    public int BaseRequireReinforceCount => baseRequireReinforceCount;


#if UNITY_EDITOR
    /// <summary>
    /// �� ����(����) �߿��� ������� �� ��, �� �� �ݵ�� #if UNITY_EDITOR�� �����ٰ�.
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
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");
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
