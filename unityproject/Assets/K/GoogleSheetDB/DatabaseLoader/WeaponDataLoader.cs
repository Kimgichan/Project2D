using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using EquipKind = Enums.EquipKind;

#if UNITY_EDITOR
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
            

            var kind = (EquipKind)Enum.Parse(typeof(EquipKind), values[1]);
            //datas[i].Damage = new Vector2(float.Parse(values[2]), float.Parse(values[3]));
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

            datas[i].WriteData(kind, minDamage, maxDamage, reinforceMaxCount, unlockAttributeCounts, baseRequireReinforceCount);
        }
        AssetDatabase.SaveAssets();
    }
}
#endif
