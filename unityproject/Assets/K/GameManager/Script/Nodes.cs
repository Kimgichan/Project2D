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


    [System.Serializable]
    public struct ItemInfo
    {
        public string itemName;
        public Enums.ItemKind kind;
        public int count;
        public string content;

        public ItemInfo(string itemName, 
            Enums.ItemKind kind, 
            int count, string content)
        {
            this.itemName = itemName;
            this.kind = kind;
            this.count = count;
            this.content = content;
        }

        public void From(InterfaceList.Item item)
        {
            if(item == null)
            {
                content = "";
                count = -1;
                kind = Enums.ItemKind.Empty;
                itemName = "Null";
                return;
            }

            content = item.Content;
            count = item.CurrentCount;
            itemName = item.Name;
            kind = item.Kind;
        }

        public static ItemInfo Empty => new ItemInfo() { itemName = "Null", content = "", count = -1, kind = Enums.ItemKind.Empty };

        public static ItemInfo Cast(InterfaceList.Item item)
        {
            if (item == null)
                return Empty;
            else
                return new ItemInfo()
                {
                    content = item.Content,
                    count = item.CurrentCount,
                    itemName = item.Name,
                    kind = item.Kind
                };
        } 
    }
}
