using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeaponPrefab
{
    public bool AttackAnim(float timer, UnityAction endEvent);
    public void StopAnim();
}

public interface IController
{
    public void OrderAction(params Order[] orders);
    public void OrderAction(List<Order> orders);
    public GameObject GetGameObject();
    public Transform GetTransform();

    #region Order 관련 API
    public struct Order
    {
        public OrderTitle orderTitle;
        public object parameter;
    }

    // main = animState
    // sub = buffStates;
    public enum OrderTitle
    {
        #region OrderAction이 있는 OrderTitle
        Idle, // => main
        Move, // => main
        Attack, // 공격하는 행동을 취함 => main
        AttackStop, // 공격을 멈춤
        Dead,
        Dash,
        Follow, // 상대를 따라감
        Avoiding, // 상대와 거리를 벌림
        Pushed, // 일정 거리로 밀쳐짐
        Shock, //  일정 시간 깜빡이는 효과, 조작 불가능 => main
        Damage, // 데미지를 입음
        Super, // 일정 시간 데미지, 경직을 입지 않음 => sub
        
        #endregion

        #region OrderAction이 없는 OrderTitle
        AttackDash, // 공격을 할 때 부가적으로 생기는 대시 => main
        #endregion
    }
    #endregion
}