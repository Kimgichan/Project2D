using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderAction = IController.Order;

public class Projectile : MonoBehaviour
{
    protected Enums.Projectile projectileKind;
    public Enums.Projectile Kind => projectileKind;

    protected IController attackController;

    protected OrderAction[] sendEvent;
    public virtual void Shot(IController attackController, Vector3 pos, Vector2 force, OrderAction[] sendEvent = null)
    {}

    //Projectile를 상속받은 오브젝트는 SetActive(false)보다는 Push를 사용할 것
    public virtual void Push() {}
}
