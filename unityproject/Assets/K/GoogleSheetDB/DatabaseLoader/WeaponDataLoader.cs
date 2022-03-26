using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using EquipKind = Enums.EquipKind;
public class WeaponDataLoader : MonoBehaviour
{
    [SerializeField] List<WeaponData> datas;
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
            datas[i].Damage = new Vector2(float.Parse(values[2]), float.Parse(values[3]));
            datas[i].ReinforceMaxCount = int.Parse(values[4]);
            

            var unlockRequires = datas[i].UnlockAttributeCounts_Warning;
            unlockRequires.Clear();
            var list = values[5].Split('/');
            for(int j = 0, jcount = list.Length; j<jcount; j++)
            {
                unlockRequires.Add(int.Parse(list[j]));
            }

            datas[i].BaseRequireReinforceCount = int.Parse(values[6]);
        }

        AssetDatabase.SaveAssets();
    }
}
