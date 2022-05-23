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
    void OrderAction(params Order[] orders);
    void OrderAction(List<Order> orders);

    public struct Order
    {
        public OrderTitle orderTitle;
        public List<object> parameters;
    }

    public enum OrderTitle
    {
        Idle,
        Move,
        Attack,
        AttackStop,
        Follow,
        Avoiding,
        Dead,
        Dash
    }
}