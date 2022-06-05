using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class ControllerCollision : MonoBehaviour
{
    public ObjectController controller;

    private void Start()
    {
        //var col = GetComponent<Collider2D>();
        //col.isTrigger = true;
    }

    public UnityAction<Collider2D> triggerEnter;
    public UnityAction<Collider2D> triggerStay;
    public UnityAction<Collider2D> triggerExit;

    public virtual Collider2D[] CurrentCheckTrigger()
    {
        return new Collider2D[] { };
    }

    public virtual Collider2D GetCollider() { return null; }
}
