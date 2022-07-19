using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using NaughtyAttributes;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class DamageText : Effect
{
    #region 변수 목록
    [SerializeField] protected float activeTimer;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected Text text;

    [SerializeField] protected float zOrder;
    #endregion

    #region 프로퍼티 목록

    #endregion

    #region 함수 목록

    public override void Push()
    {
        GameManager.Instance.EffectManager.Push(this);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">3D(월드공간 좌표값)</param>
    /// <param name="damage"></param>
    public virtual void Play(Vector3 pos, int damage)
    {
        StopAllCoroutines();
        StartCoroutine(DelayPlayCor(pos, damage));
    }

    private IEnumerator DelayPlayCor(Vector3 pos, int damage)
    {
        transform.SetParent(GameManager.Instance.HudPanel.transform);

        yield return new WaitForSeconds(0.2f);

        transform.localScale = Vector3.one;

        var screenPoint = Camera.main.WorldToScreenPoint(pos + (Vector3)offset);
        screenPoint.z = zOrder;
        (transform as RectTransform).position = screenPoint;

        var rectTr = text.transform as RectTransform;
        rectTr.DOKill();
        rectTr.pivot = new Vector2(0.5f, 1f);
        rectTr.DOPivotY(0f, activeTimer).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Push();
        });

        text.DOKill();
        var color = text.color;
        color.a = 1f;
        text.color = color;

        color.a = 0f;
        text.DOColor(color, activeTimer).SetEase(Ease.InQuint);

        text.text = $"{damage}";

        while (true)
        {
            yield return null;

            screenPoint = Camera.main.WorldToScreenPoint(pos + (Vector3)offset);
            screenPoint.z = zOrder;
            (transform as RectTransform).position = screenPoint;
        }
    }
    #endregion
}
