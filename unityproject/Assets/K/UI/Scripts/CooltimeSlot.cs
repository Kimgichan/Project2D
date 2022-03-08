using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CooltimeSlot : MonoBehaviour
{
    [HideInInspector] public float cooltime;
    [SerializeField] Image timerImage;
    [SerializeField] Text cooltimeTxt;

    private void OnEnable()
    {
        var timer = 1f;

        DOTween.To(() => timer, v => timer = v, 0f, cooltime).OnUpdate(() =>
        {
            timerImage.fillAmount = timer;
            cooltimeTxt.text = $"{Mathf.CeilToInt(cooltime * timer)}";
        }).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
