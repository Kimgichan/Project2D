using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillNode : MonoBehaviour
{
    [SerializeField] Image outColor;
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

    [SerializeField] Image icon;
    [SerializeField] Button btn;
    private SkillData skill;
    public SkillData Skill
    {
        get
        {
            return skill;
        }
        set
        {
            skill = value;
            icon.sprite = skill.Icon;
        }
    }

    public UnityAction<SkillNode> clickEvent;
    private void Start()
    {
        btn.onClick.AddListener(() => clickEvent(this));
    }
}
