using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;


/// <summary>
/// �������� ��� �����ϰ� �׿� ���� ���� �ΰ�ȿ��, ���ݹ���� ó���ϴ� ���ڷ����� 
/// </summary>
public class EquipmentDecorator : MonoBehaviour
{
    #region ���� ���


    /// <summary>
    /// ������Ƽ ��Ͽ� �ִ� 'WeaponItem'�� ������ ��
    /// </summary>
    [ReadOnly] 
    [SerializeField] private WeaponItem weaponItem;



    [ReadOnly] [SerializeField] private int addOriginalHP;
    [ReadOnly] [SerializeField] private int addCurrentHP;
    #endregion


    #region ������Ƽ ���

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
    /// ���� ����� force
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


    #region �Լ� ���

    #region API
    /// <summary>
    /// ������ ���Ⱑ ��� ������ �������� �ʴ� ��� ���� �� false <br/>
    /// �׿� ���� ó���� ��Ʈ�ѷ� ���� �ȿ��� ������ ���� ����
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
    /// ������ �߰� ȿ�� ������ ���� ���. ���⿡�� �߰�
    /// </summary>
    /// <param name="hitTarget"></param>
    public void SendAttackEvent(ObjectController hitTarget)
    {

    }

    /// <summary>
    /// ������ �߰� ȿ�� ������ ���� ���. ���⿡�� �߰�
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


    #region ���� �Լ�

    /// <summary>
    /// 'EquipDecorator'���� ������� ���⸦ ��� �� ȣ��.<br/>
    /// ���⸦ ������ ���(����� �ܼ��� �ı��� ����)<br/>
    /// ���� ����� ���� �������� �ٴڿ� ������ ������ ����Ǵ� ������ ��� ��
    /// </summary>
    private void PopWeaponItem()
    {
        weaponItem.EraseWeaponObject();
        weaponItem = null;
    }

    #endregion

    #endregion
}
