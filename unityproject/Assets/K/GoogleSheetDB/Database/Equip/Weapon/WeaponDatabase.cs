using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Object/Equip/WeaponDatabase", order = int.MaxValue)]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField] private List<WeaponDataPrefabNode> weaponDataPrefabList;
    private Dictionary<WeaponData, GameObject> toPrefabDic;
    private Dictionary<string, WeaponData> weaponTable;
    public int WeaponTableCount => weaponTable.Count;
    public WeaponData GetWeaponData(string title) => weaponTable[title];

    public IWeaponPrefab ToPrefab(WeaponData data) => toPrefabDic [data].GetComponent<IWeaponPrefab>();

    [System.Serializable]
    public class WeaponDataPrefabNode
    {
        public WeaponData key_weapon;
        public GameObject value_prefab;
    }


    [SerializeField] private List<EquipAttributePresetData> equipAttributeList;
    private Dictionary<Enums.EquipKind, Dictionary<string, EquipAttributePresetData>> equipAttributeTable;
    public int EquipAttributePresetCount(Enums.EquipKind kind) => equipAttributeTable[kind].Count;
    public EquipAttributePresetData GetEquipAttributePresetData(Enums.EquipKind kind, string title) => equipAttributeTable[kind][title];


    private void OnEnable()
    {
        if(weaponDataPrefabList != null)
        {
            toPrefabDic = new Dictionary<WeaponData, GameObject>();
            weaponTable = new Dictionary<string, WeaponData>();
            for(int i = 0, icount = weaponDataPrefabList.Count; i < icount; i++)
            {
                toPrefabDic.Add(weaponDataPrefabList[i].key_weapon, weaponDataPrefabList[i].value_prefab);
                weaponTable.Add(weaponDataPrefabList[i].key_weapon.name, weaponDataPrefabList[i].key_weapon);
            }
        }

        if(equipAttributeList != null)
        {
            equipAttributeTable = new Dictionary<Enums.EquipKind, Dictionary<string, EquipAttributePresetData>>();
            for(int i = 0, icount = equipAttributeList.Count; i<icount; i++)
            {
                if(equipAttributeTable.TryGetValue(equipAttributeList[i].Kind, out Dictionary<string, EquipAttributePresetData> val))
                {
                    val.Add(equipAttributeList[i].name, equipAttributeList[i]);
                }
                else
                {
                    equipAttributeTable.Add(equipAttributeList[i].Kind, new Dictionary<string, EquipAttributePresetData>() { { equipAttributeList[i].name, equipAttributeList[i] } });
                }
            }
        }
    }

    public WeaponItem CreateWeaponItem(WeaponData weaponData)
    {
        var weaponItem = new WeaponItem();
        var attributePresetList = new List<EquipAttributePresetData>(equipAttributeTable[weaponData.Kind].Values);


        weaponItem.Data = new WeaponItem.WeaponSetting()
        {
            weaponTitle = weaponData.name,
            reinforceCount = 0,
            attributeTitle = attributePresetList[Random.Range(0, attributePresetList.Count)].name,
            statTitles = new List<string>(),
            statSteps = new List<int>()
        };

        return weaponItem;
    }
}
