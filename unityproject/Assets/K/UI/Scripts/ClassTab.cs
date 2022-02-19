using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClassTab : MonoBehaviour
{
    [SerializeField] private Image outColor;
    [SerializeField] private Button btn;
    [SerializeField] private Text className;

    public Color OutColor
    {
        get
        {
            return outColor.color;
        }
        set
        {
            outColor.color = value;
        }
    }
    public UnityAction<ClassTab> clickEvent;
    public string ClassName
    {
        get
        {
            return className.text;
        }
        set
        {
            className.text = value;
        }
    }

    private void Start()
    {
        btn.onClick.AddListener(() => clickEvent(this));   
    }
}
