using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillInfo : MonoBehaviour
{
    [SerializeField] Text content;
    public string Content
    {
        set => content.text = value;
    }

    [HideInInspector] public SkillSlot targetSlot;


    private void Start()
    {
        var btn = transform.GetChild(0).gameObject.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        StartCoroutine(FixRectCor());
    }
    public void Close()
    {
        gameObject.SetActive(false);

        if (targetSlot != null)
        {
            targetSlot.Unselected();
            targetSlot = null;
        }
    }

    IEnumerator FixRectCor()
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
        while ((transform as RectTransform).anchoredPosition != Vector2.zero)
        {
            yield return null;
            (transform as RectTransform).anchoredPosition = Vector2.zero;
        }

        transform.DOScale(Vector3.one, 0.25f);
    }
}
 