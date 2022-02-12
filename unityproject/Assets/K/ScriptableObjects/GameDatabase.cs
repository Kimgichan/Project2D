using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Scriptable Object/GameDatabase", order = int.MaxValue)]
public class GameDatabase : ScriptableObject
{
    public Color GetClassColor(string className) => classDic[className].color;


    [SerializeField] private List<ClassNode> classKind;
    private Dictionary<string, ClassFeature> classDic;
    public SkillData GetClassSkillData(string className, string skill) => classDic[className].skillDic[skill];
    public int GetClassSkillCount(string className) => classDic[className].skillDic.Count;

    public int GetClassCount() => classDic.Count;
    public string GetClassName(int indx) => classKind[indx].className;

    public Dictionary<string, SkillData>.ValueCollection GetClassSkills(string className) => classDic[className].skillDic.Values;


    private void OnEnable()
    {
        if(classKind != null)
        {
            classDic = new Dictionary<string, ClassFeature>();
            foreach(var classNode in classKind)
            {
                var feature = new ClassFeature();
                feature.color = classNode.color;

                var skillTreeUpdate = new Dictionary<string, SkillData>();
                foreach(var skill in classNode.skills)
                {
                    skillTreeUpdate.Add(skill.name, skill);
                }
                feature.skillDic = skillTreeUpdate;

                classDic.Add(classNode.className, feature);
            }
        }
    }

    [System.Serializable]
    public class ClassNode
    {
        public string className;
        public Color color;
        public List<SkillData> skills;
    }


    private class ClassFeature 
    {
        public Color color;
        public Dictionary<string, SkillData> skillDic;
    }

}
