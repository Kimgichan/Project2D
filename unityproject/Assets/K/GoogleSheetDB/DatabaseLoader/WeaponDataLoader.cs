#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using EquipKind = Enums.EquipKind;

/// <summary>
/// 인게임에서 실행되는 함수는 아님. 
/// 게임이 실행되기 위해 필요한 데이터를 Update하는데 사용되는 함수.
/// 구글 시트에 테이블 정보를 Text로 뽑아서 csv에 넣어줄 것
/// </summary>
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
