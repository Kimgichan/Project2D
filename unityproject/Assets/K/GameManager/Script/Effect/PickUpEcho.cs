using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

public class PickUpEcho : Effect
{
    [SerializeField] protected float cycletimer;
    [SerializeField] protected float downDistance;

    protected bool start = false;

    protected void Start()
    {
        effectKind = Enums.Effect.PickUp;
        start = true;
    }

    public override void Push()
    {
        if (start)
        {
            StopAllCoroutines();
            GameManager.Instance.EffectManager.Push(this);
            transform.DOKill();
        }
        gameObject.SetActive(false);
    }

    public override void Show(ObjectController requireController, in Vector3 pos, in Vector2 force, List<UnityAction<ObjectController>> sendEvents)
    {
        StartCoroutine(ShowCor(pos));
    }

    protected IEnumerator ShowCor(Vector3 pos)
    {
        while (!start) yield return null;

        transform.position = pos;

        transform.DOMoveY(transform.position.y - downDistance, cycletimer).SetLoops(-1, LoopType.Yoyo);
    }
}
