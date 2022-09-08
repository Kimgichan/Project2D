using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using DG.Tweening;

public class ItemDescription : MonoBehaviour
{
    #region 변수
    [SerializeField] private Text title;
    [ReadOnly] [SerializeField] private string titleStr;
    [SerializeField] private Text content;
    #endregion


    #region 프로퍼티
    public string Title
    {
        get => titleStr;
        set
        {
            titleStr = value;
            title.text = $"< {titleStr} >";
        }
    }

    public string Content
    {
        get => content.text;
        set => content.text = value;
    }
    #endregion


    #region 함수
    private void Start()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0f;
    }

    public void Play(InterfaceList.Item item, float duration)
    {
        if (item == null) return;

        Play(item.Name, item.Content, duration);
    }

    public void Play(string title, string content, float duration)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) return;

        canvasGroup.DOKill();

        Title = title;
        Content = content;

        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, duration).SetEase(Ease.InExpo);
    }
    #endregion
}
