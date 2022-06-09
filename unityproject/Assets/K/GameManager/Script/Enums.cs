using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum EquipKind { Bow, Sword, Wand, WeaponAll, EquipAll}
    public enum EquipAttribute { Dash, FireCount, Guide}
    public enum Effect { PickUp, Arrow }

    
    public enum CreatureState
    {
        Idle,
        Move,
        Attack, 
        Dash,
        Shock,
        PathFind,
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
}
