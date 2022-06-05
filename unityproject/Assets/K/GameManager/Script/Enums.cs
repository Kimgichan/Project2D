using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum EquipKind { Bow, Sword, Wand, WeaponAll, EquipAll}
    public enum EquipAttribute { Dash, FireCount, Guide}
    public enum Projectile { Arrow, MagicBall}
    public enum Effect { PickUp }

    
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
        /// None : 아직 컨트롤러가 Start 함수를 호출하지 않았다는 의미<br/>
        /// 즉, 준비가 되지 않았다.
        /// </summary>
        None,

        Object,

        Creature,
    }
}
