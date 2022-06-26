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
    /// ���� ������
    /// </summary>
    [ReadOnly] 
    [SerializeField] private WeaponItem weapon;


    #endregion


    #region �Լ� ���


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
        if ((object)weapon != null)
        {
            //weapon.Attack(attackController,  dir);
            GameManager.Instance
                .ControllerManager
                .DecoratorManager
                .EquipmentEffectPlay(weapon.Weapon.AttackEffect,
                    attackController, this, dir);
            return true;
        }

        return false;
    }

    #endregion
}
