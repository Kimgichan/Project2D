using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using EquipAttribute = EquipAttributePresetData.AttributeNode;

[System.Serializable]
public class WeaponItem
{
    private WeaponData weaponData;
    public WeaponData Weapon => weaponData;
    private int reinforceCount;
    public int ReinforceCount => reinforceCount;

    private EquipAttributePresetData attributeData;
    public EquipAttributePresetData AttributeState => attributeData;
    private List<EquipAttribute> stats;
    public int AttributeStatCount => stats.Count;
    public EquipAttribute GetStat(int indx) => stats[indx];


    //저장 로드할 때 오고갈 데이터
    public WeaponSetting Data
    {
        get
        {
            var icount = stats.Count;
            var result = new WeaponSetting() { weaponTitle = weaponData.name, reinforceCount = reinforceCount, attributeTitle = attributeData.name, 
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
            weaponData = GameManager.Instance.GameDB.WeaponManager.GetWeaponData(value.weaponTitle);
            reinforceCount = value.reinforceCount;
            attributeData = GameManager.Instance.GameDB.WeaponManager.GetEquipAttributePresetData(weaponData.Kind, value.attributeTitle);

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
        public string weaponTitle;
        public int reinforceCount;
        public string attributeTitle;
        public List<string> statTitles;
        public List<int> statSteps;
    }
}
