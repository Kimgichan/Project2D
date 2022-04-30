using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderAction = IController.Order;

public class Projectile : MonoBehaviour
{
    public virtual void Shot(IController attackController, Vector2 force, OrderAction[] sendEvent)
    {}
}
