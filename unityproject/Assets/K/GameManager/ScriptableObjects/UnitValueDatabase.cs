using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// speed�����, �󸶳� �и� �������� ���� ������<br/> 
/// ��ȹ������ �����ϴ� �Ϳ� ������� �ִ� ������<br/>
/// ��ȹ�����δ� ������� ���� ǥ�� (unitValue * 1��, unitValue * 2��)<br/>
/// �� unitValue���� Database��.
/// </summary>
[CreateAssetMenu(fileName = "UnitValueDatabase", menuName = "Scriptable Object/UnitValueList")]
public class UnitValueDatabase : DatabaseLoader
{
    #region ���� ���
    [SerializeField] private float unitSpeed;
    [SerializeField] private float unitPush;
    [SerializeField] private float unitDist;
    [SerializeField] private float unitRange;
    [SerializeField] private float unitGuide;

    #region ������ ���� ���� ���
#if UNITY_EDITOR
    [Space(20)]
    [SerializeField] private string unitValueDocID;
    [SerializeField] private string unitValueGID;
#endif
    #endregion
    #endregion


    #region ������Ƽ ���
    public float UnitSpeed => unitSpeed;
    public float UnitPush => unitPush;
    public float UnitDist => unitDist;
    public float UnitRange => unitRange;
    public float UnitGuide => unitGuide;
    #endregion


    #region �Լ� ���

    #region ������ ���� �Լ� ���
#if UNITY_EDITOR
    [Button("Unit Value DB ������Ʈ")]
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
