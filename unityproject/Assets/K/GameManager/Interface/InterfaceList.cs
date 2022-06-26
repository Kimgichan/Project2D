using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeaponPrefab
{

    /// <summary>
    /// attackEvent : 모션이 공격 판정 포인트까지 도달했을 경우 어떤 공격 이펙트가 나갈 것인지.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="attackEvent">모션이 공격 판정 포인트까지 도달했을 경우 어떤 공격 이펙트가 나갈 것인지.</param>
    /// <returns></returns>
    public bool AttackAnim(float timer, UnityAction attackEvent);
    public void StopAnim();

    public Transform GetTr();
}
