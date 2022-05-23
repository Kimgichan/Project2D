using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PickUpEcho : Effect
{
    [SerializeField] private float cycletimer;
    [SerializeField] private float downDistance;

    private bool start = false;

    void Start()
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

    // parameters ¸ñ·Ï
    // 0 => Vector3 pos
    public struct ParametersNode
    {
        public Vector3 pos;
    }
    public override void Show(object parametersNode)
    {
        StartCoroutine(ShowCor(((ParametersNode)parametersNode).pos));
    }

    private IEnumerator ShowCor(Vector3 pos)
    {
        while (!start) yield return null;

        transform.position = pos;

        transform.DOMoveY(transform.position.y - downDistance, cycletimer).SetLoops(-1, LoopType.Yoyo);
    }
}
