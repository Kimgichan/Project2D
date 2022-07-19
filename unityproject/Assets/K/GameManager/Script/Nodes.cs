using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 서브 클래스(데이타 참고용 클래스), 구조체 모음
/// </summary>
public class Nodes
{
    [System.Serializable]
    public struct WeaponSetting
    {
        public string weaponData;
        public int reinforceCount;
        public string attributePreset;
        public List<string> statNames;
        public List<int> statLevels;
    }

    [System.Serializable]
    public struct EquipAttributePercent
    {
        public Enums.EquipAttribute kind;
        public float percent;
    }

    [System.Serializable]
    public struct EquipAttributeLevel
    {
        [SerializeField] private EquipAttributeData equipAttributeData;
        [SerializeField] private int level;

        public EquipAttributeData EquipAttributeData
        {
            get
            {
                return equipAttributeData;
            }
        }

        public int Level
        {
            get => level;
            set
            {
                if(value < 0)
                {
                    level = 0;
                }

                level = value;
            }
        }

        public EquipAttributeLevel(Enums.EquipAttribute kind, int level)
        {
            //this.kind = kind;
            equipAttributeData = GameManager.Instance.EquipAttributeManager.EquipAttributeDB
                .GetData(kind);
            this.level = level;
        }
    }
}
