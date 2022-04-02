using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using EquipAttribute = EquipAttributePresetData.AttributeNode;

[System.Serializable]
public class WeaponItem
{
    //이변수는 나중에 체계화 되어야함. 중복값이 생기지 않도록 시스템 설계.
    private int id;
    public int InstanceID => id;

    private WeaponData weaponData;
    public WeaponData Weapon => weaponData;
    private int reinforceCount;
    public int ReinforceCount => reinforceCount;

    private EquipAttributePresetData attributeData;
    public EquipAttributePresetData AttributeState => attributeData;
    private List<EquipAttribute> stats;
    public int attributeStatCount => stats.Count;
    public EquipAttribute GetStat(int indx) => stats[indx];


    //저장 로드할 때 오고갈 데이터
    public WeaponSetting Data
    {
        get
        {
            var icount = stats.Count;
            var result = new WeaponSetting() { id = id, weaponTitle = weaponData.name, reinforceCount = reinforceCount, attributeTitle = attributeData.name, 
                statTitles = new List<string>(icount), statSteps = new List<int>(icount) };
            for(int i = 0; i<icount; i++)
            {
                result.statTitles.Add(stats[i].kind.ToString());
                result.statSteps.Add(stats[i].count);
            }
            return result;
        }
        set
        {
            id = value.id;
            weaponData = GameManager.Instance.GameDB.WeaponTable[value.weaponTitle];
            reinforceCount = value.reinforceCount;
            attributeData = GameManager.Instance.GameDB.EquipAttributePresetTable[value.attributeTitle];

            var icount = value.statTitles.Count;
            stats = new List<EquipAttribute>(icount);
            for(int i = 0; i<icount; i++)
            {
                stats.Add(new EquipAttribute() { 
                    kind = (Enums.EquipAttribute)Enum.Parse(typeof(Enums.EquipAttribute), value.statTitles[i]), 
                    count = value.statSteps[i] });
            }
        }
    }

    [System.Serializable]
    public struct WeaponSetting
    {
        public int id;
        public string weaponTitle;
        public int reinforceCount;
        public string attributeTitle;
        public List<string> statTitles;
        public List<int> statSteps;
    }
}
