using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class TextAutoHeight : MonoBehaviour
{
    [SerializeField] private Text autoHeightText;
    
    [Button("텍스트 상자 높이 업데이트")] 
    public void UpdateHeightScale()
    {
        if (autoHeightText == null) return;

        var extents = autoHeightText.cachedTextGenerator.rectExtents.size * 0.5f;
        var setting = autoHeightText.GetGenerationSettings(extents);
        var lineHeight = autoHeightText.cachedTextGeneratorForLayout.GetPreferredHeight(autoHeightText.text, setting);
        var textHeight = lineHeight * autoHeightText.lineSpacing / setting.scaleFactor;

        var sizeDelta = (autoHeightText.transform as RectTransform).sizeDelta;
        sizeDelta.y = textHeight;
        (autoHeightText.transform as RectTransform).sizeDelta = sizeDelta;
    }
}
