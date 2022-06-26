using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeaponPrefab
{

    /// <summary>
    /// attackEvent : ����� ���� ���� ����Ʈ���� �������� ��� � ���� ����Ʈ�� ���� ������.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="attackEvent">����� ���� ���� ����Ʈ���� �������� ��� � ���� ����Ʈ�� ���� ������.</param>
    /// <returns></returns>
    public bool AttackAnim(float timer, UnityAction attackEvent);
    public void StopAnim();

    public Transform GetTr();
}
