using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// speed�����, �󸶳� �и� �������� ���� ������<br/> 
/// ��ȹ������ �����ϴ� �Ϳ� ������� �ִ� ������<br/>
/// ��ȹ�����δ� ������� ���� ǥ�� (unitValue * 1��, unitValue * 2��)<br/>
/// �� unitValue���� Database��.
/// </summary>
[CreateAssetMenu(fileName = "UnitValueDatabase", menuName = "Scriptable Object/UnitValueList")]
public class UnitValueDatabase : ScriptableObject
{
    [SerializeField] private float unitSpeed;
    public float UnitSpeed => unitSpeed;

    [SerializeField] private float unitPush;
    public float UnitPush => unitPush;

    [SerializeField] private float unitDist;
    public float UnitDist => unitDist;
}
