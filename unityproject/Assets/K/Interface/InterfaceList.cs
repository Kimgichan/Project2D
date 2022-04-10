using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeaponPrefab
{
    public bool AttackAnim(float timer, UnityAction endEvent);
    public void StopAnim();
}
