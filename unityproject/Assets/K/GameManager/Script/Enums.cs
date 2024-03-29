using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Enums
{
    public enum EquipKind 
    { 
        Bow, 
        Sword, 
        Wand, 
        Armor,
    }

    public enum EquipAttribute 
    { 
        Dash, 
        FireCount, 
        Guide,
    }


    /// <summary>
    /// PickUp : 줍기 관련 이펙트<br/>
    /// Arrow : 투사체 관련 이펙트<br/>
    /// Shock : 범위 공격 관련 이펙트 && 근접 공격용으로도 사용<br/>
    /// </summary>
    public enum Effect 
    { 
        PickUp_Base, 
        
        Arrow_Base,
        Arrow_Target,

        Shock_Base,
        HP_Bar_Base,
        DamageText,

        Aim_Base,
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
        /// None : 아직 컨트롤러가 Start 함수를 호출하지 않았다는 의미<br/>
        /// 즉, 준비가 되지 않았다.
        /// </summary>
        None,

        Object,

        Creature,
    }


    #region 컨트롤러 데코레이터 
    public enum Decorator
    {
        /// <summary>
        /// 컨트롤러가 아이템을 착용 혹은 사용할 수 있게 해주는 데코레이터 
        /// </summary>
        Equipment,
        HUD,
        Inventory,
    }

    /// <summary>
    /// Decorator enum(int) 값이랑 인덱스 순서가 대응되게 배치할 것
    /// </summary>
    private static List<Type> decorators = new List<Type>()
    {
        typeof(EquipmentDecorator),
        typeof(HUDDecorator),
        typeof(InventoryDecorator),
    };

    public static Type GetDecoratorType(Decorator decorator)
    {
        return decorators[(int)decorator];
    }
    #endregion


    #region 아이템
    public enum ItemKind
    {
        Weapon,
        Portion,
        Projectile,
        Empty
    }

    /// <summary>
    /// ItemKind enum(int) 값이랑 인덱스 순서가 대응되게 배치할 것
    /// </summary>
    private static List<Type> items = new List<Type>()
    {
        typeof(WeaponItem),
        typeof(Portion),
        typeof(Projectile),
    };

    public static Type GetItemType(ItemKind item)
    {
        return items[(int)item];
    }


    public enum PortionKind
    {
        HP,
        MP,
    }
    #endregion
}
