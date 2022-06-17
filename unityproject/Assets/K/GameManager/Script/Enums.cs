using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum EquipKind { Bow, Sword, Wand, WeaponAll, EquipAll}
    public enum EquipAttribute { Dash, FireCount, Guide}


    /// <summary>
    /// PickUp : 줍기 관련 이펙트<br/>
    /// Arrow : 투사체 관련 이펙트<br/>
    /// Shock : 범위 공격 관련 이펙트 && 근접 공격용으로도 사용<br/>
    /// </summary>
    public enum Effect { PickUp_Base, Arrow_Base, Shock_Base }

    
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
        /// None : 아직 컨트롤러가 Start 함수를 호출하지 않았다는 의미<br/>
        /// 즉, 준비가 되지 않았다.
        /// </summary>
        None,

        Object,

        Creature,
    }
}
