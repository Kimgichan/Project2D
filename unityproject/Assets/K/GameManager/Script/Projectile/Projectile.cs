using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    protected Enums.Projectile projectileKind;
    public Enums.Projectile Kind => projectileKind;

    protected CreatureController attackController;

    protected List<UnityAction<CreatureController>> sendEvents;
    public virtual void Shot(CreatureController attackController, 
        Vector3 pos, Vector2 force, 
        List<UnityAction<CreatureController>> sendEvents = null)
    {}

    //Projectile를 상속받은 오브젝트는 SetActive(false)보다는 Push를 사용할 것
    public virtual void Push() {}
}
