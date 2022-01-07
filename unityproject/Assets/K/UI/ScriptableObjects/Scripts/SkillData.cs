using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] int maxLevel;
    public int MaxLevel => maxLevel;
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;
    public string SkillName => name;

    [SerializeField] [TextArea] string tooltip;
    public string Tooltip => tooltip;
}

