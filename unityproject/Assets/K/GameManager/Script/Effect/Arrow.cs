using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using DG.Tweening;


public class Arrow : Effect
{
    #region 변수 목록
    protected ObjectController attackController;
    protected UnityAction<ObjectController> sendEvent;

    protected Vector2 force;
    protected IEnumerator actionCor;
    protected UnityAction<Collider2D> actionTriggerEnterFunc;
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected float dropRotationSpeed;
    [SerializeField] protected Vector2 echoOffset;

    protected bool start = false;

    /// <summary>
    /// echo 변수 말고 프로퍼티 목록에 있는 Echo를 사용할 것
    /// </summary>
    protected PickUpEcho echo;

    [SerializeField] protected TrailRenderer trail;

    #endregion

    #region 프로퍼티 목록
    protected PickUpEcho Echo
    {
        get => echo;
        set
        {
            if((object)echo != null)
            {
                echo.Push();
            }

            echo = value;
        }
    }


    #endregion


    #region 모노비헤이비어 API

    protected void Start()
    {
        start = true;
        trail.enabled = false;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (actionTriggerEnterFunc != null)
            actionTriggerEnterFunc(collision);
    }

    #endregion

    #region 함수 목록
    public override void Push()
    {
        if (start)
        {
            //GameManager.Instance.ProjectileManager.Push(this);
            GameManager.Instance.EffectManager.Push(this);
        }

        Echo = null;
        gameObject.SetActive(false);
    }

    public virtual void Play(ObjectController requireController, 
        in Vector3 pos, in Vector2 force,
        UnityAction<ObjectController> sendEvent = null )
    {
        StartCoroutine(ShotCor(requireController, pos, force, sendEvent));
    }

    protected IEnumerator ShotCor(ObjectController requireController, 
        Vector3 pos, Vector2 force, 
        UnityAction<ObjectController> sendEvent)
    {
        while (!start) yield return null;

        transform.position = pos;
        this.attackController = requireController;
        this.force = force;
        if (sendEvent != null)
            this.sendEvent = sendEvent;
        else this.sendEvent = null;

        if (actionCor != null) StopCoroutine(actionCor);

        actionCor = MoveCor();

        actionTriggerEnterFunc = ShotTriggerEnter;
        StartCoroutine(actionCor);
    }

    protected void ShotTriggerEnter(Collider2D collision)
    {
        var hitController = collision.gameObject.GetComponent<ControllerCollision>().controller;
        if (attackController.Equals(hitController)) return;

        if (hitController != null && sendEvent != null)
        {
            sendEvent(hitController);
        }
        sendEvent = null;
        if (actionCor != null)
        {
            StopCoroutine(actionCor);
            actionCor = null;
        }


        actionTriggerEnterFunc = null;

        Drop(collision.gameObject);
    }
    protected void DropTriggerEnter(Collider2D collision)
    {
        if(attackController.Equals(collision.gameObject.GetComponent<ControllerCollision>().controller))
        {
            Push();
            Echo = null;
        }
    }
    protected IEnumerator MoveCor()
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

    protected void Drop(GameObject hitTarget)
    {
        rigidbody.simulated = false;
        float distance = Random.Range(1.2f, 1.6f);
        Vector2 startPos = transform.position;

        Vector2 dir;
        if (hitTarget.CompareTag("Block"))
        {
            dir = ((Vector2)attackController.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        var hitList = Physics2D.CircleCastAll(startPos, 0.2f, dir, distance);


        for(int i = 0, icount = hitList.Length; i<icount; i++)
        {
            var controll = hitList[i].collider;
            if (hitTarget != controll.gameObject && controll.gameObject.CompareTag("Block"))
            {
                var newDistanceSqr = (hitList[i].point - startPos).sqrMagnitude;
                if (distance * distance > newDistanceSqr)
                {
                    distance = Mathf.Sqrt(newDistanceSqr);
                }
            }
        }

        float x = 0f;
        var rotateDir = (Random.Range(0, 2) == 0 ? -dropRotationSpeed : dropRotationSpeed);

        var end = startPos + dir * distance;
        //Echo = GameManager.Instance.EffectManager.Pop(Enums.Effect.PickUp_Base, new Vector3(end.x + echoOffset.x, end.y + echoOffset.y, end.y));
        Echo = GameManager.Instance.EffectManager.Pop(Enums.Effect.PickUp_Base) as PickUpEcho;

        Echo?.gameObject.SetActive(true);
        Echo?.Play(new Vector3(end.x + echoOffset.x, end.y + echoOffset.y, end.y));
        
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

    #endregion
}
