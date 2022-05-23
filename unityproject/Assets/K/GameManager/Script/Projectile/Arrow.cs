using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using DG.Tweening;
using OrderAction = IController.Order; 

public class Arrow : Projectile
{
    private Vector2 force;
    private IEnumerator actionCor;
    private UnityAction<Collider2D> actionTriggerEnterFunc;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float dropRotationSpeed;
    [SerializeField] private Vector2 echoOffset;

    private bool start = false;
    private Effect echo;
    private Effect Echo
    {
        get => echo;
        set
        {
            if(echo != null)
            {
                echo.Push();
            }

            if (value == null)
            {
                echo = null;
            }
            else echo = value;
        }
    }

    [SerializeField] private TrailRenderer trail;
    private void Start()
    {
        projectileKind = Enums.Projectile.Arrow;
        start = true;
        trail.enabled = false;
    }

    //private void OnD
    //{
    //    if (start)
    //    {
    //        Debug.Log("????");
    //        GameManager.Instance.projectileManager.Push(this);
    //    }
    //}

    public override void Push()
    {
        if (start)
        {
            GameManager.Instance.ProjectileManager.Push(this);
        }

        Echo = null;
        gameObject.SetActive(false);
    }

    public override void Shot(IController attackController, Vector3 pos, Vector2 force, OrderAction[] sendEvent = null )
    {
        StartCoroutine(ShotCor(attackController, pos, force, sendEvent));
    }

    private IEnumerator ShotCor(IController attackController, Vector3 pos, Vector2 force, OrderAction[] sendEvent)
    {
        while (!start) yield return null;

        transform.position = pos;
        this.attackController = attackController;
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
        if(collision.gameObject.tag.Equals("Object") || collision.gameObject.tag.Equals("Monster") || collision.gameObject.tag.Equals("Player"))
        {
            if (actionTriggerEnterFunc != null)
                actionTriggerEnterFunc(collision);
        }
    }

    private void ShotTriggerEnter(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<IController>();
        if (controller != null && controller != attackController)
        {
            if (sendEvent != null)
                controller.OrderAction(sendEvent);
            sendEvent = null;
            if (actionCor != null)
            {
                StopCoroutine(actionCor);
                actionCor = null;
            }


            actionTriggerEnterFunc = null;
            //actionCor = DropCor();
            //StartCoroutine(actionCor);
            Drop(controller);
        }
    }
    private void DropTriggerEnter(Collider2D collision)
    {
        if(attackController == collision.gameObject.GetComponent<IController>())
        {
            Push();
            Echo = null;
        }
    }
    private IEnumerator MoveCor()
    {
        yield return null;
        trail.enabled = true;
        rigidbody.simulated = true;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(force.y, force.x)*Mathf.Rad2Deg-90f, Vector3.forward);
        while (true)
        {
            yield return null;
            rigidbody.velocity = force;
        }
    }

    private void Drop(IController hitController)
    {
        rigidbody.simulated = false;
        float distance = Random.Range(1.2f, 1.6f);
        Vector2 startPos = transform.position;
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        var hitList = Physics2D.CircleCastAll(startPos, 0.2f, dir, distance);

        for(int i = 0, icount = hitList.Length; i<icount; i++)
        {
            var controll = hitList[i].collider.gameObject.GetComponent<IController>();
            if (controll != attackController && controll != hitController)
            {
                //end = hitList[i].point;
                var newDistanceSqr = (hitList[i].point - startPos).sqrMagnitude;
                if (distance * distance > newDistanceSqr)
                {
                    //end = hitList[i].point;
                    distance = Mathf.Sqrt(newDistanceSqr);
                }
            }
        }

        float x = 0f;
        var rotateDir = (Random.Range(0, 2) == 0 ? -dropRotationSpeed : dropRotationSpeed);

        var end = startPos + dir * distance;
        Echo = GameManager.Instance.EffectManager.Pop(Enums.Effect.PickUp, new PickUpEcho.ParametersNode() { pos = new Vector3(end.x + echoOffset.x, end.y + echoOffset.y, end.y) });
        
        DOTween.To(() => x, (val) => x = val, distance, 0.65f).OnUpdate(() =>
        {
            var newPos = startPos + dir * x;
            newPos.y += -4 * Mathf.Pow(x / distance - 0.5f, 2f) + 1f;
            transform.position = new Vector3(newPos.x, newPos.y, newPos.y);
            transform.eulerAngles += new Vector3(0f, 0f, rotateDir * Time.deltaTime);
        }).OnComplete(() =>
        {
            trail.enabled = false;
            rigidbody.simulated = true;
            rigidbody.velocity = Vector2.zero;
            actionTriggerEnterFunc = DropTriggerEnter;
        });
    }

    //private IEnumerator DropCor()
    //{
    //    //yield return null;


    //    state = EquipState.Drop;
    //    actionTriggerEnterFunc = DropTriggerEnter;
    //}
}
