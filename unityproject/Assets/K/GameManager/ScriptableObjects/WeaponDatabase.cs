using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using UnityEngine.Networking;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;
using EquipKind = Enums.EquipKind;
using EquipAttribute = Enums.EquipAttribute;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Object/Equip/WeaponDatabase", order = int.MaxValue)]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField] private List<WeaponDataPrefabNode> weaponDataPrefabList;
    private Dictionary<WeaponData, GameObject> toPrefabDic;
    private Dictionary<string, WeaponData> weaponTable;
    public int WeaponTableCount => weaponTable.Count;
    public WeaponData GetWeaponData(string title) => weaponTable[title];

    public IWeaponPrefab GetWeaponPrefab(WeaponData data) => toPrefabDic [data].GetComponent<IWeaponPrefab>();

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


        weaponItem.Data = new Nodes.WeaponSetting()
        {
            weaponTitle = weaponData.name,
            reinforceCount = 0,
            attributeTitle = attributePresetList[Random.Range(0, attributePresetList.Count)].name,
            statTitles = new List<string>(),
            statSteps = new List<int>()
        };

        return weaponItem;
    }


#if UNITY_EDITOR
    #region 데이터 로더 관련 로직


    #region 웨폰 데이터 로더
    [SerializeField] List<WeaponData> updateWeaponDatas;
    [SerializeField] private string weaponDocID;
    [SerializeField] private string weaponGID;

    [Button] public void WeaponDataUpdateStart()
    {
        CoroutineHelper.StartCoroutine(DataUpdateCor(weaponDocID, weaponGID, WeaponDataUpdate));
    }

    private void WeaponDataUpdate(string csv)
    {
        // 웨폰 데이타 로더에 존재하는 웨폰 데이타 갱신 함수 이쪽으로 옮김.
        var lines = csv.Split('\n');
        var dataCount = lines.Length - 1;

        for(int i = 0, icount = updateWeaponDatas.Count; i<icount && i <dataCount; i++)
        {
            var path = AssetDatabase.GetAssetPath(updateWeaponDatas[i].GetInstanceID());
            var values = lines[i + 1].Split(',');

            AssetDatabase.RenameAsset(path, values[0]);
            AssetDatabase.ImportAsset(path);

            var kind = (EquipKind)Enum.Parse(typeof(EquipKind), values[1]);

            var minDamage = int.Parse(values[2]);
            var maxDamage = int.Parse(values[3]);
            var reinforceMaxCount = int.Parse(values[4]);


            var unlockAttributeCounts = new List<int>();
            var list = values[5].Split('/');
            for(int j = 0, jcount = list.Length; j<jcount; j++)
            {
                unlockAttributeCounts.Add(int.Parse(list[j]));
            }


            var baseRequireReinforceCount = int.Parse(values[6]);

            var attackEffect = (Enums.Effect)Enum.Parse(typeof(Enums.Effect), values[7]);

            updateWeaponDatas[i].WriteData(kind, attackEffect, minDamage, maxDamage, reinforceMaxCount, unlockAttributeCounts, baseRequireReinforceCount);
        }


        AssetDatabase.SaveAssets();
    }

    #endregion


    #region 웨폰 특성 프리셋 데이터 로더
    [SerializeField] List<EquipAttributePresetData> updateEAPresetDatas;
    [SerializeField] private string eAPresetDocID;
    [SerializeField] private string eAPresetGID;

    [Button] public void EquipAttributePresetDataUpdateStart()
    {
        CoroutineHelper.StartCoroutine(DataUpdateCor(eAPresetDocID, eAPresetGID, EquipAttributePresetDataUpdate));
    }

    private void EquipAttributePresetDataUpdate(string csv)
    {
        var lines = csv.Split('\n');
        var dataCount = lines.Length - 1;

        for(int i = 0, icount = updateEAPresetDatas.Count; i<icount && i<dataCount; i++)
        {
            var path = AssetDatabase.GetAssetPath(updateEAPresetDatas[i].GetInstanceID());
            var values = lines[i + 1].Split(',');

            AssetDatabase.RenameAsset(path, values[0]);
            AssetDatabase.ImportAsset(path);

            var kind = (EquipKind)Enum.Parse(typeof(EquipKind), values[1]);


            var attributeList = new List<EquipAttributePresetData.Node>();
            var list = values[2].Split('/');
            for(int j = 0, jcount = list.Length; j<jcount; j++)
            {
                list[j] = list[j].Replace("\r", "");
                var pair = list[j].Split('(');
                attributeList.Add(new EquipAttributePresetData.Node()
                {
                    kind = (EquipAttribute)Enum.Parse(typeof(EquipAttribute), pair[0]),
                    percent = float.Parse(pair[1].Substring(0, pair[1].Length - 1)) / 100f
                });
            }

            updateEAPresetDatas[i].WriteData(kind, attributeList);
        }


        AssetDatabase.SaveAssets();
    }

    #endregion

    private IEnumerator DataUpdateCor(string docID, string gid, UnityAction<string> updateMethod)
    {
        if ((object)updateMethod == null)
        {
            yield break;
        }

        UnityWebRequest www = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{docID}/export?format=csv&gid={gid}");

        yield return www.SendWebRequest();

        if (www.result.Equals(UnityWebRequest.Result.ConnectionError) ||
            www.result.Equals(UnityWebRequest.Result.ProtocolError))
        {
            yield break;
        }

        updateMethod(www.downloadHandler.text);
    }
    #endregion
#endif
}
