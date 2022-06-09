using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// speed라던가, 얼마나 밀릴 것인지에 대한 정도는<br/> 
/// 기획적으로 접근하는 것에 어려움이 있다 여겨져<br/>
/// 기획상으로는 배속으로 성능 표현 (unitValue * 1배, unitValue * 2배)<br/>
/// 그 unitValue값의 Database임.
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
