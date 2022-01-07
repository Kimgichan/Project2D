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
                Debug.LogError($"{parent}�� ��ųƮ���� {skillName}��/�� �������� �ʽ��ϴ�.");
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
        //���� ��쿡 ����
        foreach(var openQuest in skillQuestList)
        {
            if(openQuest.skillSlot.level > 0)
            {
                parent.SetSkill(openQuest.skillSlot.SkillName, openQuest.skillSlot.level);
            }
        }

        //�ӽ� �ڵ� ���� Ȯ�� �Ϸ���
        parent.Save();
    }


    //����Ʈ�� ����ߴ���. �׷��� ��ų�� ���� �� �ִ� �� �˻�
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
        Debug.LogError($"�������� �ʴ� ��ų({skillSlot.SkillName})�� ������ �ø��� �߽��ϴ�.");
        return false;
    }


    [System.Serializable]
    public class OpenQuest
    {
        [Header("��ų��/����1 => ���� sp �̻�/����2 => ���� ��ų��")]
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


    //��ųƮ���� �̿��ϴ� ������Ʈ�� �� �������̽��� ��ӹ޾ƾ���.
    public interface ISkillTreeObject
    {
        public void SetSkill(in string skillName, in int level);
        public void GetSkill(int indx, out string skillName, out int level);
        public int Count();
        public void Save();
    }
}
