using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerCollisionBox : ControllerCollision
{
    private BoxCollider2D col;
    // Start is called before the first frame update
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(triggerEnter != null)
        {
            triggerEnter(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(triggerStay != null)
        {
            triggerStay(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(triggerExit != null)
        {
            triggerExit(collision);
        }
    }

    public override Collider2D[] CurrentCheckTrigger()
    {
        return Physics2D.OverlapBoxAll(col.offset + (Vector2)col.transform.position, col.size, 0f);
    }

    public override Collider2D GetCollider()
    {
        return col;
    }
}
