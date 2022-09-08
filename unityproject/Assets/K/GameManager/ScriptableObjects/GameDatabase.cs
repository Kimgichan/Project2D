using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Scriptable Object/GameDatabase", order = int.MaxValue)]
public class GameDatabase : ScriptableObject
{

    #region 변수 목록
    [SerializeField] private WeaponDatabase weaponDB;
    [SerializeField] private EffectDatabase effectDB;
    [SerializeField] private UnitValueDatabase unitValueDB;
    #endregion


    #region 프로퍼티 목록
    public WeaponDatabase WeaponManager => weaponDB;
    public EffectDatabase EffectDB => effectDB;
    public UnitValueDatabase UnitValueDB => unitValueDB;
    #endregion


    #region 함수 목록

    #endregion

}
