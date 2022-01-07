using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataBase", menuName = "Skill/SkillDataBase")]
public class SkillDataBase : ScriptableObject
{
    [SerializeField] List<SkillData> skillList;
    Dictionary<string, SkillData> skillDic;
    private void OnEnable()
    {
        skillDic = new Dictionary<string, SkillData>();
        foreach (var skill in skillList)
            skillDic.Add(skill.SkillName, skill);
    }

    public SkillData GetSkill(int indx)
    {
        if (indx < 0 || indx >= skillList.Count) 
            return null;
        return 
            skillList[indx];
    }
    public SkillData GetSkill(string skillName)
    {
        if (skillDic.TryGetValue(skillName, out SkillData val))
            return val;
        else 
            return null;
    }
    public int Count => skillList.Count;
}
