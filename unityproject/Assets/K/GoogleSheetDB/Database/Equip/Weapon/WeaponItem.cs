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
    #region 변수 목록

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int reinforceCount;
    [SerializeField] private EquipAttributePresetData attributePresetData;
    [SerializeField] private List<EquipAttributeLevel> stats;

    /// <summary>
    /// 실제 인게임에서 노출되는(보여지는) 객체
    /// </summary>
    private InterfaceList.WeaponPrefab weaponObject;

    #endregion


    #region 프로퍼티 목록

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


    //저장 로드할 때 오고갈 데이터
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



    #region 함수 목록

    public EquipAttributeLevel GetStat(int indx) => stats[indx];

    /// <summary>
    ///  EquipAttributeLevel.level 값이 음수면 미보유 스탯
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
    /// 스탯의 레벨을 올림, <br/>
    /// 만약 addLevel 값이 양수고, 보유하지 않은 stat일 경우 stats에 추가됨<br/>
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="addLevel"></param>
    public void AddEquipAttributeLevel(Enums.EquipAttribute stat, int addLevel)
    {
        var statLevel = GetStat(stat);

        if(statLevel.Level >= 0) //스탯을 보유하고 있음
        {
            statLevel.Level += addLevel;
        }
        else if(addLevel >= 0)
        {
            stats.Add(new EquipAttributeLevel(stat, addLevel));
        }
    }

    /// <summary>
    /// weaponObject를 할당하고 붙임
    /// </summary>
    /// <param name="rigging">의 자식으로 붙여짐</param>
    public void DrawWeaponObject(Transform rigging)
    {
        EraseWeaponObject();

        weaponObject = GameObject.Instantiate(GameManager.Instance.GameDB.WeaponManager.GetWeaponPrefab(Weapon).GetTr().gameObject, rigging)
            .GetComponent<InterfaceList.WeaponPrefab>();
    }

    /// <summary>
    /// weaponObject를 삭제함
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
    /// timer : attackEvent를 언제 호출할 것인지, 그리고 모션의 속도<br/>
    /// attackEvent : timer가 끝나면 진행되는 공격 정의
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="attackEvent"></param>
    public void AttackAnim(float speed, UnityAction attackEvent)
    {
        if((object)weaponObject == null)
        {
            Debug.LogError("웨폰 아이템 weaponObject 값 null");
            return;
        }

        weaponObject.AttackAnim(speed, attackEvent);
    }

    #endregion


    #region 아이템 인터페이스
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
