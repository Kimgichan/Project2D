using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Scriptable Object/GameDatabase", order = int.MaxValue)]
public class GameDatabase : ScriptableObject
{

    #region ���� ���
    [SerializeField] private WeaponDatabase weaponDB;
    [SerializeField] private EffectDatabase effectDB;
    [SerializeField] private UnitValueDatabase unitValueDB;
    #endregion


    #region ������Ƽ ���
    public WeaponDatabase WeaponManager => weaponDB;
    public EffectDatabase EffectDB => effectDB;
    public UnitValueDatabase UnitValueDB => unitValueDB;
    #endregion


    #region �Լ� ���

    #endregion

}
