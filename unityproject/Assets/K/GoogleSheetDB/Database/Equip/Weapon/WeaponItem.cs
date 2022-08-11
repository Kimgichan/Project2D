using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using NaughtyAttributes;


using EquipAttributeLevel = Nodes.EquipAttributeLevel;
using WeaponSetting = Nodes.WeaponSetting;

[System.Serializable]
public class WeaponItem : InterfaceList.Item
{
    #region ���� ���

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int reinforceCount;
    [SerializeField] private EquipAttributePresetData attributePresetData;
    [SerializeField] private List<EquipAttributeLevel> stats;

    /// <summary>
    /// ���� �ΰ��ӿ��� ����Ǵ�(��������) ��ü
    /// </summary>
    private InterfaceList.WeaponPrefab weaponObject;

    #endregion


    #region ������Ƽ ���

    public WeaponData Weapon => weaponData;
    public int ReinforceCount => reinforceCount;
    public EquipAttributePresetData AttributePreset => attributePresetData;
    public int AttributeStatCount => stats.Count;


    public int MinDamage
    {
        get
        {
            var val = 0;

            val += Weapon.MinDamage;
            val += Mathf.FloorToInt(val * Weapon.MinDamage * (float)ReinforceCount);

            return val;
        }
    }

    public int MaxDamage
    {
        get
        {
            var val = 0;

            val += Weapon.MaxDamage;
            val += Mathf.FloorToInt(val * Weapon.MaxDamage * (float)ReinforceCount);

            return val;
        }
    }


    public int RandomDamage
    {
        get
        {
            var val = 0;

            val += Weapon.RandomDamage;
            val += Mathf.FloorToInt(val * Weapon.AddDamage * (float)ReinforceCount);

            return val;
        }
    }


    //���� �ε��� �� ���� ������
    public WeaponSetting Data
    {
        get
        {
            var icount = stats.Count;
            var result = new WeaponSetting()
            {
                weaponData = weaponData.name,
                reinforceCount = reinforceCount,
                attributePreset = attributePresetData.name,
                statNames = new List<string>(icount),
                statLevels = new List<int>(icount)
            };
            for (int i = 0; i < icount; i++)
            {
                result.statNames.Add(stats[i].EquipAttributeData.EquipAttribute.ToString());
                result.statLevels.Add(stats[i].Level);
            }
            return result;
        }
        set
        {
            weaponData = GameManager.Instance.GameDB.WeaponManager.GetWeaponData(value.weaponData);
            reinforceCount = value.reinforceCount;
            attributePresetData = GameManager.Instance.GameDB.WeaponManager.GetEquipAttributePresetData(weaponData.Kind, value.attributePreset);

            var icount = value.statNames.Count;
            stats = new List<EquipAttributeLevel>(icount);
            for (int i = 0; i < icount; i++)
            {
                stats.Add(new EquipAttributeLevel((Enums.EquipAttribute)Enum.Parse(typeof(Enums.EquipAttribute), value.statNames[i]), value.statLevels[i]));
            }
        }
    }
    #endregion



    #region �Լ� ���

    public EquipAttributeLevel GetStat(int indx) => stats[indx];

    /// <summary>
    ///  EquipAttributeLevel.level ���� ������ �̺��� ����
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public EquipAttributeLevel GetStat(Enums.EquipAttribute stat)
    {
        //return stats.Find(f => f.kind.Equals(stat));
        for(int i = 0, icount = stats.Count; i<icount; i++)
        {
            if(stats[i].EquipAttributeData.EquipAttribute == stat)
            {
                return stats[i];
            }
        }

        return new EquipAttributeLevel(stat, -1);
    }

    /// <summary>
    /// ������ ������ �ø�, <br/>
    /// ���� addLevel ���� �����, �������� ���� stat�� ��� stats�� �߰���<br/>
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="addLevel"></param>
    public void AddEquipAttributeLevel(Enums.EquipAttribute stat, int addLevel)
    {
        var statLevel = GetStat(stat);

        if(statLevel.Level >= 0) //������ �����ϰ� ����
        {
            statLevel.Level += addLevel;
        }
        else if(addLevel >= 0)
        {
            stats.Add(new EquipAttributeLevel(stat, addLevel));
        }
    }

    /// <summary>
    /// weaponObject�� �Ҵ��ϰ� ����
    /// </summary>
    /// <param name="rigging">�� �ڽ����� �ٿ���</param>
    public void DrawWeaponObject(Transform rigging)
    {
        EraseWeaponObject();

        weaponObject = GameObject.Instantiate(GameManager.Instance.GameDB.WeaponManager.GetWeaponPrefab(Weapon).GetTr().gameObject, rigging)
            .GetComponent<InterfaceList.WeaponPrefab>();
    }

    /// <summary>
    /// weaponObject�� ������
    /// </summary>
    public void EraseWeaponObject()
    {
        if (weaponObject != null)
        {
            GameObject.Destroy(weaponObject.GetTr().gameObject);
            weaponObject = null;
        }
    }


    /// <summary>
    /// timer : attackEvent�� ���� ȣ���� ������, �׸��� ����� �ӵ�<br/>
    /// attackEvent : timer�� ������ ����Ǵ� ���� ����
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="attackEvent"></param>
    public void AttackAnim(float speed, UnityAction attackEvent)
    {
        if((object)weaponObject == null)
        {
            Debug.LogError("���� ������ weaponObject �� null");
            return;
        }

        weaponObject.AttackAnim(speed, attackEvent);
    }

    #endregion


    #region ������ �������̽�
    [SerializeField] private bool use;

    public string Name => Weapon.name;

    public Enums.ItemKind Kind => Enums.ItemKind.Weapon;

    public bool Use
    {
        get => use;
        set => use = value;
    }

    public int CurrentCount
    {
        get => 1;
        set { }
    }

    public int MaxCount => 1;

    public Sprite Icon => Weapon.Icon;

    public string Content => "";

    public void Destroy() {}

    public void Drop() { }

    public InterfaceList.Item Copy() => null;
    #endregion
}
