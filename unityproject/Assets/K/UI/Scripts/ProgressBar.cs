using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    #region ����
    [SerializeField] private Image currentBar;
    public float maxWidth;
    [SerializeField] private float percentage;
    #endregion


    #region ������Ƽ
    public float Percentage
    {
        get => percentage;
        set
        {
            if (value < 0f) value = 0f;
            else if (value > 1f) value = 1f;

            percentage = value;

            var rectTr = currentBar.transform as RectTransform;
            if (rectTr == null) return;

            var size = rectTr.sizeDelta;
            size.x = maxWidth * percentage;
            rectTr.sizeDelta = size;
        }
    }
    #endregion


    #region �Լ�
    #endregion
}
