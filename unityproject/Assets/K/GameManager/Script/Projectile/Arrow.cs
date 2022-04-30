using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using EquipState = Enums.EquipState;
using OrderAction = IController.Order; 

public class Arrow : Projectile
{
    IController attackController;
    private EquipState state;
    private Vector2 force;
    private IEnumerator actionCor;
    private UnityAction<Collider2D> actionTriggerEnterFunc;
    [SerializeField] private Rigidbody2D rigidbody;
    OrderAction[] sendEvent;
    [SerializeField] private AnimationCurve curve;
 
    public EquipState State
    {
        get => state;
    }

    private void Start()
    {
        state = EquipState.Disalve;
    }

    public override void Shot(IController attackController,Vector2 force, OrderAction[] sendEvent)
    {
        this.attackController = attackController;
        state = EquipState.Shot;
        this.force = force;
        if (sendEvent != null)
            this.sendEvent = sendEvent;
        else this.sendEvent = null;

        if (actionCor != null) StopCoroutine(actionCor);

        actionCor = MoveCor();

        actionTriggerEnterFunc = ShotTriggerEnter;
        StartCoroutine(actionCor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Object")
        {
            if (actionTriggerEnterFunc != null)
                actionTriggerEnterFunc(collision);
        }
    }

    private void ShotTriggerEnter(Collider2D collision)
    {
        state = EquipState.Stop;
        var controller = collision.gameObject.GetComponent<IController>();
        if (controller != null)
        {
            if (sendEvent != null)
                controller.OrderAction(sendEvent);
        }

        sendEvent = null;
        if (actionCor != null)
        {
            StopCoroutine(actionCor);
        }


        actionTriggerEnterFunc = null;
        actionCor = DropCor();
        StartCoroutine(actionCor);
    }
    private void DropTriggerEnter(Collider2D collision)
    {
        if(attackController == collision.gameObject.GetComponent<IController>())
        {
            state = EquipState.Disalve;
        }
    }
    private IEnumerator MoveCor()
    {
        while (true)
        {
            yield return null;
            rigidbody.velocity = force;
        }
    }

    private IEnumerator DropCor()
    {
        //yield return null;
        var start = (Vector2)transform.position;
        var f = force.magnitude;
        var x = Random.Range(0f, 1f);
        var y = Random.Range(0f, 1f);
        var xy = x + y;

        if(Random.Range(0, 2) > 0)
        {
            x *= -1f;
        }

        if(Random.Range(0, 2) > 0)
        {
            y *= -1f;
        }

        var end = new Vector2(f * x / xy + start.x, f * y / xy + start.y);
        var duration = 0.8f;
        var time = 0.0f;
        var hoverHeihgt = start.y + 0.7f;
        while(time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0.0f, hoverHeihgt, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);
            yield return null;
        }

        state = EquipState.Drop;
        actionTriggerEnterFunc = DropTriggerEnter;
    }
}
