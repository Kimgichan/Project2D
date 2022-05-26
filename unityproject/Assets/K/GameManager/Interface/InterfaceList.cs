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

    #region Order ���� API
    public struct Order
    {
        public OrderTitle orderTitle;
        public object parameter;
    }

    // main = animState
    // sub = buffStates;
    public enum OrderTitle
    {
        #region OrderAction�� �ִ� OrderTitle
        Idle, // => main
        Move, // => main
        Attack, // �����ϴ� �ൿ�� ���� => main
        AttackStop, // ������ ����
        Dead,
        Dash,
        Follow, // ��븦 ����
        Avoiding, // ���� �Ÿ��� ����
        Pushed, // ���� �Ÿ��� ������
        Shock, //  ���� �ð� �����̴� ȿ��, ���� �Ұ��� => main
        Damage, // �������� ����
        Super, // ���� �ð� ������, ������ ���� ���� => sub
        
        #endregion

        #region OrderAction�� ���� OrderTitle
        AttackDash, // ������ �� �� �ΰ������� ����� ��� => main
        #endregion
    }
    #endregion
}