using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "EquipAttributeDatabase", menuName = "Scriptable Object/Equip/AttributeDatabase", order = int.MaxValue)]
public class EquipAttributeDatabase : DatabaseLoader
{
    #region 변수 목록
    [SerializeField] private List<EquipAttributeData> equipAttributeDatas;


    #region 에디터 전용 변수 목록
#if UNITY_EDITOR

    [SerializeField] private string attributeDocID;
    [SerializeField] private string attributeGID;

#endif
    #endregion
    #endregion


    #region 프로퍼티 목록


    #endregion


    #region 함수 목록

    public EquipAttributeData GetData(Enums.EquipAttribute attribute)
    {
        for(int i = 0, icount = equipAttributeDatas.Count; i<icount; i++)
        {
            var data = equipAttributeDatas[i];
            if(data.EquipAttribute == attribute)
            {
                return data;
            }
        }
        return null;
    }


    #region 에디터 전용 함수 목록
#if UNITY_EDITOR

    [Button("장비 특성 DB 업데이트")] 
    public void EquipAttributeDataUpdate()
    {
        CoroutineHelper.StartCoroutine(DataUpdateCor(attributeDocID, attributeGID,
            EquipAttributeDataUpdateRun));
    }

    private void EquipAttributeDataUpdateRun(string csv)
    {
        var lines = csv.Split('\n');

        for(int i = 0, icount = equipAttributeDatas.Count; i<icount; i++)
        {
            var equipAttributeData = equipAttributeDatas[i];
            var values = lines[i + 1].Split(',');
            var path = AssetDatabase.GetAssetPath(equipAttributeDatas[i].
                GetInstanceID());

            AssetDatabase.RenameAsset(path, values[0]);

            var attribute = (Enums.EquipAttribute)Enum.Parse(
                typeof(Enums.EquipAttribute), values[0]);
            var tooltip = values[1];
            var maxLevel = int.Parse(values[2]);

            var levelValues = new List<float>();
            if (!values[3].Equals("-"))
            {
                var levelValuesStr = values[3].Split('/');
                for(int j = 0, jcount = levelValuesStr.Length; j < jcount; j++)
                {
                    levelValues.Add(float.Parse(levelValuesStr[j]));
                }
            }

            equipAttributeData.WriteData(attribute, tooltip, maxLevel, levelValues);
        }

        AssetDatabase.SaveAssets();
    }

#endif
    #endregion

    #endregion
}
