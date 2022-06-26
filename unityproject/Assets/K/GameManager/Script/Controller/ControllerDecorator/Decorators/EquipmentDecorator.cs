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
    /// 참고 데이터
    /// </summary>
    [ReadOnly] 
    [SerializeField] private WeaponItem weapon;


    #endregion


    #region 함수 목록


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
