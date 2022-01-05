using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTree : MonoBehaviour
{

    [SerializeField] List<OpenQuest> skillQuestList;
    Dictionary<string, OpenQuest> skillQuestDic;

    [SerializeField] SkillInfo skillInfo;
    public SkillInfo SkillTooltipPanel => skillInfo;
    ISkillTreeObject parent;

    public void Init(ISkillTreeObject parent)
    {
        this.parent = parent;

        for(int i = 0, icount = parent.Count(); i<icount; i++)
        {
            parent.GetSkill(i, out string skillName, out int level);
            if(skillQuestDic.TryGetValue(skillName, out OpenQuest v))
            {
                v.skillSlot.level = level;
            }
            else
            {
                Debug.LogError($"{parent}의 스킬트리에 {skillName}은/는 존재하지 않습니다.");
            }
        }
    }
    private void Start()
    {
        skillQuestDic = new Dictionary<string, OpenQuest>();
        foreach (var skillQuest in skillQuestList)
            skillQuestDic.Add(skillQuest.skillSlot.SkillName, skillQuest);
    }

    private void OnDisable()
    {
        //끝날 경우에 저장
        foreach(var openQuest in skillQuestList)
        {
            if(openQuest.skillSlot.level > 0)
            {
                parent.SetSkill(openQuest.skillSlot.SkillName, openQuest.skillSlot.level);
            }
        }

        //임시 코드 저장 확인 하려고
        parent.Save();
    }


    //퀘스트를 통과했는지. 그래서 스킬을 찍을 수 있는 지 검사
    public bool Pass(SkillSlot skillSlot)
    {
        if(skillQuestDic.TryGetValue(skillSlot.SkillName, out OpenQuest v))
        {
            foreach(var quest in v.requireQuest)
            {
                if (!skillQuestDic.ContainsKey(quest.priorSkillSlot.SkillName))
                    return false;
                else if (quest.priorSkillSlot.level < quest.requireLevel)
                    return false;
            }
            return true;
        }
        Debug.LogError($"존재하지 않는 스킬({skillSlot.SkillName})의 레벨을 올리려 했습니다.");
        return false;
    }


    [System.Serializable]
    public class OpenQuest
    {
        [Header("스킬명/조건1 => 오픈 sp 이상/조건2 => 선행 스킬들")]
        public SkillSlot skillSlot;
        public int requireSP;
        public List<PriorQuest> requireQuest;
    }


    [System.Serializable]
    public class PriorQuest
    {
        public SkillSlot priorSkillSlot;
        public int requireLevel;
    }


    //스킬트리를 이용하는 오브젝트는 이 인터페이스를 상속받아야함.
    public interface ISkillTreeObject
    {
        public void SetSkill(in string skillName, in int level);
        public void GetSkill(int indx, out string skillName, out int level);
        public int Count();
        public void Save();
    }
}
