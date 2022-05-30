using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class CreatureTrigger : MonoBehaviour
{
    public CreatureController controller;

    private void Start()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    #region Æ®¸®°Å API
    public UnityAction<Collider2D> triggerEnter;
    public UnityAction<Collider2D> triggerStay;
    public UnityAction<Collider2D> triggerExit;


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
    #endregion
}
