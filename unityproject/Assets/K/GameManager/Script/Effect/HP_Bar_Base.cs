using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using NaughtyAttributes;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class HP_Bar_Base : Effect
{
    #region 변수 목록
    [SerializeField] protected float activeTimer;
    [SerializeField] protected Slider slider;
    [SerializeField] protected float valueChangeSpeed;

    [ReadOnly] [SerializeField] protected CreatureController controller;
    [ReadOnly] [SerializeField] protected bool start = false;

    [ReadOnly] [SerializeField] protected float currentPercent;

    protected IEnumerator updateCor;
    protected UnityAction endEvent;
    protected float currentTimer;

    #endregion


    #region 함수 목록

    protected void Start()
    {
        start = true;
    }

    public override void Push()
    {
        controller = null;
        updateCor = null;
        StopAllCoroutines();


        if (endEvent != null)
        {
            endEvent();
            endEvent = null;
        }

        GameManager.Instance.EffectManager.Push(this);
        gameObject.SetActive(false);
    }

    public void Play(CreatureController controller, UnityAction endEvent)
    {
        if(this.controller != controller)
        {
            this.controller = controller;
            if(updateCor != null)
            {
                StopCoroutine(updateCor);
            }
            updateCor = UpdateCor();
        }
        else if(updateCor == null)
        {
            updateCor = UpdateCor();
        }

        if (endEvent != null) this.endEvent = endEvent;
        else this.endEvent = null;

        currentTimer = activeTimer;
        StartCoroutine(updateCor);
    }
    public void Play(CreatureController controller)
    {
        if (this.controller != controller)
        {
            this.controller = controller;
            if (updateCor != null)
            {
                StopCoroutine(updateCor);
            }
            updateCor = UpdateCor();
        }
        else if (updateCor == null)
        {
            updateCor = UpdateCor();
        }

        currentTimer = activeTimer;
        StartCoroutine(updateCor);
    }

    protected IEnumerator UpdateCor()
    {
        yield return null;
        while (!start) yield return null;
        var rectTr = this.transform as RectTransform;
        rectTr.localScale = Vector3.one;
        rectTr.anchorMax = new Vector2(1f, 0f);
        rectTr.offsetMin = new Vector2(0f, rectTr.offsetMin.y);
        rectTr.offsetMax = new Vector2(0f, rectTr.offsetMax.y);
        rectTr.localPosition = Vector3.zero;
        rectTr.anchoredPosition = new Vector2(0f, 0f);


        currentPercent = (float)controller.CurrentHP / (float)controller.OriginalHP;

        while (currentTimer > 0f)
        {
            currentPercent = Mathf.Lerp(currentPercent, (float)controller.CurrentHP / (float)controller.OriginalHP, Time.deltaTime * valueChangeSpeed);

            slider.value = currentPercent;
            currentTimer -= Time.deltaTime;
            yield return null;
        }

        Push();
    }

    #endregion
}
