using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Enums
{
    public enum EquipKind { Bow, Sword, Wand, WeaponAll, EquipAll}
    public enum EquipAttribute { Dash, FireCount, Guide}


    /// <summary>
    /// PickUp : �ݱ� ���� ����Ʈ<br/>
    /// Arrow : ����ü ���� ����Ʈ<br/>
    /// Shock : ���� ���� ���� ����Ʈ && ���� ���ݿ����ε� ���<br/>
    /// </summary>
    public enum Effect 
    { 
        PickUp_Base, 
        Arrow_Base, 
        Shock_Base 
    }

    
    public enum CreatureState
    {
        Idle,
        Move,
        Attack,
        Dash, 
        Shock,
        PathFind,
        Push,
    }

    public enum ControllerKind
    {
        /// <summary>
        /// None : ���� ��Ʈ�ѷ��� Start �Լ��� ȣ������ �ʾҴٴ� �ǹ�<br/>
        /// ��, �غ� ���� �ʾҴ�.
        /// </summary>
        None,

        Object,

        Creature,
    }


    #region ��Ʈ�ѷ� ���ڷ����� 
    public enum Decorator
    {
        /// <summary>
        /// ��Ʈ�ѷ��� �������� ���� Ȥ�� ����� �� �ְ� ���ִ� ���ڷ����� 
        /// </summary>
        Equipment,
    }

    /// <summary>
    /// Decorator enum(int) ���̶� �ε��� ������ �����ǰ� ��ġ�� ��
    /// </summary>
    private static List<Type> decorators = new List<Type>()
    {
        typeof(EquipmentDecorator),
    };

    public static Type GetDecoratorType(Decorator decorator)
    {
        return decorators[(int)decorator];
    }
    #endregion
}
