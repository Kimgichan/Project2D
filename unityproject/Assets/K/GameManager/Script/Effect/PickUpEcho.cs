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
        start = true;
    }

    public override void Push()
    {
        StopAllCoroutines();
        GameManager.Instance.EffectManager.Push(this);
        transform.DOKill();
        gameObject.SetActive(false);
    }

    public virtual void Play(Vector3 pos)
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
