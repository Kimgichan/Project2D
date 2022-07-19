using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;


/// <summary>
/// 무기라던가 장비를 장착하고 그에 따른 또한 부가효과, 공격방식을 처리하는 데코레이터 
/// </summary>
public class EquipmentDecorator : MonoBehaviour
{
    #region 변수 목록


    /// <summary>
    /// 프로퍼티 목록에 있는 'WeaponItem'을 참고할 것
    /// </summary>
    [ReadOnly] 
    [SerializeField] private WeaponItem weaponItem;



    [ReadOnly] [SerializeField] private int addOriginalHP;
    [ReadOnly] [SerializeField] private int addCurrentHP;
    #endregion


    #region 프로퍼티 목록

    public WeaponItem WeaponItem
    {
        get
        {
            return weaponItem;
        }
        set
        {
            if((object)weaponItem != null)
            {
                PopWeaponItem();
            }

            weaponItem = value;
        }
    }

    public int MinDamage
    {
        get
        {
            var val = 0;

            if ((object)WeaponItem == null) return val;

            val += WeaponItem.MinDamage;

            return val;
        }
    }

    public int MaxDamage
    {
        get
        {
            var val = 0;

            if ((object)WeaponItem == null) return val;

            val += WeaponItem.MaxDamage;

            return val;
        }
    }


    public int RandomDamage
    {
        get
        {
            var val = 0;

            if ((object)WeaponItem == null) return val;

            val += WeaponItem.RandomDamage;

            return val;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return 0f;
        }
    }
    public float AttackRange
    {
        get
        {
            return 0f;
        }
    }
    public float PushEnergy
    {
        get
        {
            return 0f;
        }
    }

    public float Dash
    {
        get
        {
            return 0f;
        }
    }

    /// <summary>
    /// 유도 기능의 force
    /// </summary>
    public float Guide
    {
        get
        {
            var val = 0f;
            if ((object)WeaponItem == null) return val;

            //val = GameManager.Instance.GameDB.UnitValueDB.UnitGuide;

            var guide = WeaponItem.GetStat(Enums.EquipAttribute.Guide);
            if(guide.Level > 0)
            {
                val += GameManager.Instance.GameDB.UnitValueDB.UnitGuide *
                    guide.EquipAttributeData.GetLevelValue(guide.Level);
            }

            return val;
        }
    }

    public int AddOriginalHP
    {
        get
        {
            return 0;
        }
    }

    public int AddCurrentHP
    {
        get
        {
            return 0;
        }
        set
        {

        }
    }

    #endregion


    #region 함수 목록

    #region API
    /// <summary>
    /// 장착한 무기가 없어서 공격을 진행하지 않는 경우 리턴 값 false <br/>
    /// 그에 따른 처리는 컨트롤러 범위 안에서 수행할 것을 권장
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="dir"></param>
    /// <param name="sendEvent"></param>
    /// <returns></returns>
    public bool Attack(CreatureController attackController, Vector2 dir)
    {
        if ((object)WeaponItem != null)
        {
            //weapon.Attack(attackController,  dir);
            GameManager.Instance
                .ControllerManager
                .DecoratorManager
                .EquipmentEffectPlay(WeaponItem.Weapon.AttackEffect,
                    attackController, this, dir);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 파츠의 추가 효과 같은게 있을 경우. 여기에서 추가
    /// </summary>
    /// <param name="hitTarget"></param>
    public void SendAttackEvent(ObjectController hitTarget)
    {

    }

    /// <summary>
    /// 파츠의 추가 효과 같은게 있을 경우. 여기에서 추가
    /// </summary>
    public void SendHitEvent(ObjectController attackTarget)
    {

    }

    public void CreateWeapon(WeaponData weaponData, Transform parent)
    {
        var weaponItem = GameManager.Instance.GameDB.WeaponManager.CreateWeaponItem(weaponData);
        weaponItem.DrawWeaponObject(parent);
        WeaponItem = weaponItem;
    }

    #endregion


    #region 내부 함수

    /// <summary>
    /// 'EquipDecorator'에서 사용중인 무기를 때어낼 때 호출.<br/>
    /// 무기를 버리는 경우(현재는 단순히 파괴를 구현)<br/>
    /// 최종 모습은 무기 아이템을 바닥에 떨구는 연출이 진행되는 것으로 고려 중
    /// </summary>
    private void PopWeaponItem()
    {
        weaponItem.EraseWeaponObject();
        weaponItem = null;
    }

    #endregion

    #endregion
}
