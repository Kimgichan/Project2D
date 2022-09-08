using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "CreatureDatabase", menuName = "Scriptable Object/Controller/CreatureDatabase", order = int.MaxValue)]
public class CreatureDatabase : DatabaseLoader
{

    #region 변수 목록

    /// <summary>
    /// 구글 시트에 기재된 데이타 개수와 맞출 것.
    /// </summary>
    [SerializeField] private List<CreatureData> datas;

#if UNITY_EDITOR
    #region 유니티 에디터 전용 변수 목록
    [SerializeField] private string creatorDocID;
    [SerializeField] private string creatorGID;


    #endregion
#endif
    #endregion


    #region 함수 목록


#if UNITY_EDITOR
    #region 유니티 에디터 전용 함수 목록

    [Button("크리쳐 DB Update")] public void CreatureDataUpdate()
    {
        CoroutineHelper.StartCoroutine(DataUpdateCor(creatorDocID, creatorGID, CreatureDataUpdateRun));
    }

    private void CreatureDataUpdateRun(string csv)
    {
        var lines = csv.Split('\n');

        for(int i = 0, icount = datas.Count; i<icount; i++)
        {
            var values = lines[i + 1].Split(',');

            var path = AssetDatabase.GetAssetPath(datas[i].GetInstanceID());
            AssetDatabase.RenameAsset(path, values[0]);

            var hp = int.Parse(values[1]);
            var moveSpeed = float.Parse(values[2]);
            var baseAttack = (Enums.Effect)Enum.Parse(typeof(Enums.Effect), values[3]);
            var attackSpeed = float.Parse(values[4]);
            var minDamage = int.Parse(values[5]);
            var maxDamage = int.Parse(values[6]);

            var attackRange = float.Parse(values[7]);
            float push = float.Parse(values[8]);
            float stun = float.Parse(values[9]);

            datas[i].WriteData(baseAttack, hp, moveSpeed, minDamage, maxDamage, push, stun, attackSpeed, attackRange);
        }

        AssetDatabase.SaveAssets();
    }
    #endregion
#endif

    #endregion
}
