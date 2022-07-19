using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// speed라던가, 얼마나 밀릴 것인지에 대한 정도는<br/> 
/// 기획적으로 접근하는 것에 어려움이 있다 여겨져<br/>
/// 기획상으로는 배속으로 성능 표현 (unitValue * 1배, unitValue * 2배)<br/>
/// 그 unitValue값의 Database임.
/// </summary>
[CreateAssetMenu(fileName = "UnitValueDatabase", menuName = "Scriptable Object/UnitValueList")]
public class UnitValueDatabase : DatabaseLoader
{
    #region 변수 목록
    [SerializeField] private float unitSpeed;
    [SerializeField] private float unitPush;
    [SerializeField] private float unitDist;
    [SerializeField] private float unitRange;
    [SerializeField] private float unitGuide;

    #region 에디터 전용 변수 목록
#if UNITY_EDITOR
    [Space(20)]
    [SerializeField] private string unitValueDocID;
    [SerializeField] private string unitValueGID;
#endif
    #endregion
    #endregion


    #region 프로퍼티 목록
    public float UnitSpeed => unitSpeed;
    public float UnitPush => unitPush;
    public float UnitDist => unitDist;
    public float UnitRange => unitRange;
    public float UnitGuide => unitGuide;
    #endregion


    #region 함수 목록

    #region 에디터 전용 함수 목록
#if UNITY_EDITOR
    [Button("Unit Value DB 업데이트")]
    public void UnitValueDataUpdate()
    {
        CoroutineHelper.StartCoroutine(DataUpdateCor(unitValueDocID, unitValueGID,
            UnitValueDataUpdateRun));
    }

    private void UnitValueDataUpdateRun(string csv)
    {
        var lines = csv.Split('\n');

        var values = lines[1].Split(',');

        unitSpeed = float.Parse(values[0]);
        unitPush = float.Parse(values[1]);
        unitDist = float.Parse(values[2]);
        unitRange = float.Parse(values[3]);
        unitGuide = float.Parse(values[4]);
    }
#endif
    #endregion
    #endregion
}
