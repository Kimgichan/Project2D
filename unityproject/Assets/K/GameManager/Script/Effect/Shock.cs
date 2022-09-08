using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

public class Shock : Effect
{
    #region 변수 목록
    protected ObjectController attackController;

    /// <summary>
    /// 공격하는데까지 걸리는 시간
    /// </summary>
    [SerializeField] protected float attackTime;
    protected UnityAction<ObjectController> sendEvent;
    protected bool start = false;


    [SerializeField] protected SpriteRenderer sprite;
    #endregion

    #region 함수 목록

    #region Unity GameObject API
    protected void Start()
    {
        start = true;
    }
    #endregion


    #region Effect API
    public override void Push()
    {
        StopAllCoroutines();
        sendEvent = null;
        GameManager.Instance.EffectManager.Push(this);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="requireController"></param>
    /// <param name="pos"></param>
    /// <param name="attackTime"></param>
    /// <param name="sendEvents"></param>
    public virtual void Play(ObjectController requireController, Vector3 pos, float radius, float attackSpeed, UnityAction<ObjectController> sendEvent)
    {
        StopAllCoroutines();
        StartCoroutine(PlayCor(requireController, pos, radius, attackSpeed, sendEvent));
    }
    #endregion


    #region 내부 로직
    protected IEnumerator PlayCor(ObjectController requireController, Vector3 pos, float radius, float attackSpeed, UnityAction<ObjectController> sendEvent)
    {
        sprite.DOKill();
        transform.DOKill();
        while (!start) yield return null;

        transform.position = pos;

        attackController = requireController;
        this.sendEvent = sendEvent;

        sprite.color = Color.clear;
        transform.localScale = Vector3.zero;

        var attackTime = this.attackTime / attackSpeed;

        sprite.DOColor(Color.red, attackTime);
        transform.DOScale(Vector3.one * radius, attackTime).OnComplete(() =>
        {
            var cols = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Object"));


            // ObjectController 타격을 중복하지 않기 위해 검사하는 용도
            var overlapCheck = new HashSet<ObjectController>();

            for(int i = 0, icount = cols.Length; i<icount; i++)
            {
                var col = cols[i];

                var collision = col.gameObject.GetComponent<ControllerCollision>();
                if((object)collision == null || (object)collision.controller == attackController)
                {
                    continue;
                }
                if (collision.controller.tag.Equals(attackController.tag))
                {
                    continue;
                }

                if (!overlapCheck.Contains(collision.controller))
                {
                    overlapCheck.Add(collision.controller);
                    sendEvent(collision.controller);
                }
            }

            Push();
        });

        while (true)
        {
            transform.position = new Vector3(attackController.transform.position.x, attackController.transform.position.y, transform.position.z);
            yield return null;
        }
    }
    #endregion

    #endregion
}
