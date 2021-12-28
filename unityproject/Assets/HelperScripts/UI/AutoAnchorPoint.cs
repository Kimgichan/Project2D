using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutoAnchorPoint : MonoBehaviour
{
    [SerializeField] bool fix;

    RectTransform rect;
    // Start is called before the first frame update

    private void Awake()
    {
        if (!Application.isEditor)
            Destroy(this);
        else if (Application.isPlaying)
            Destroy(this);
    }
    void Start()
    {
        fix = false;
        rect = transform as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (!fix) return;

        var parentRect = rect.transform.parent as RectTransform;
        if (parentRect == null) return;

        var anchorMin = new Vector2(rect.anchorMin.x + rect.offsetMin.x / parentRect.rect.width, rect.anchorMin.y + rect.offsetMin.y / parentRect.rect.height);
        var anchorMax = new Vector2(rect.anchorMax.x + rect.offsetMax.x / parentRect.rect.width, rect.anchorMax.y + rect.offsetMax.y / parentRect.rect.height);
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        fix = false;
#endif
    }
}
