using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR

using EquipKind = Enums.EquipKind;
using EquipAttribute = Enums.EquipAttribute;

public class EquipAttributePresetDataLoader : MonoBehaviour
{
    [SerializeField] private List<EquipAttributePresetData> datas;
    [SerializeField] TextAsset csv;

    // Start is called before the first frame update
    void Start()
    {
        var lines = csv.text.Split('\n');
        for(int i = 0, icount = datas.Count; i<icount; i++)
        {
            var path = AssetDatabase.GetAssetPath(datas[i].GetInstanceID());
            var values = lines[i + 1].Split(',');
            AssetDatabase.RenameAsset(path, values[0]);
            AssetDatabase.ImportAsset(path);

            datas[i].Kind = (EquipKind)Enum.Parse(typeof(EquipKind), values[1]);

            var attributeList = datas[i].AttributeList;
            attributeList.Clear();
            var list = values[2].Split('/');
            for(int j = 0, jcount = list.Length; j<jcount; j++)
            {
                var pair = list[j].Split('(');
                attributeList.Add(new EquipAttributePresetData.Node()
                {
                    kind = (EquipAttribute)Enum.Parse(typeof(EquipAttribute), pair[0]),
                    percent = float.Parse(pair[1].Substring(0, pair[1].Length - 1)) * 0.01f
                });
            }
        }

        AssetDatabase.SaveAssets();
    }
}
#endif
