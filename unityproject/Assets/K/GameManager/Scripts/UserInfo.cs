using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour, GameManager.ITabKeyObject, SkillTree.ISkillTreeObject
{
    private string tab, key;

    UserInfoJson userInfoJson;
    Dictionary<string, UserSkillInfo> userSkillDic;


    ///////////////////////////////////////////////////////////////////////////////>
    // ��ų ���� �Լ�
    public void GetSkill(int indx, out string skillName, out int level)
    {
        try
        {
            var skillInfo = userInfoJson.userSkillList[indx];
            skillName = skillInfo.skillName;
            level = skillInfo.level;
        }
        catch
        {
            skillName = "";
            level = -1;
        }
    }


    //��ų ��� �Լ�
    public void SetSkill(in string skillName, in int level)
    {
        try
        {
            if(userSkillDic.TryGetValue(skillName, out UserSkillInfo v))
            {
                v.level = level;
            }
            else
            {
                var userSkillInfo = new UserSkillInfo(skillName, level);
                userSkillDic.Add(userSkillInfo.skillName, userSkillInfo);
                userInfoJson.userSkillList.Add(userSkillInfo);
            }
        }
        catch
        {
            Debug.LogError($"UserInfo ��ų��� �Լ� Error ��� ���� => (SkillData = {skillName})/(level = {level}).");
        }
    }

    public int Count() => userInfoJson.userSkillList.Count;
    ///////////////////////////////////////////////////////////////////////////////>


    ///////////////////////////////////////////////////////////////////////////////>
    //���� �Ŵ��� Ŭ�������� �����ؼ� �ҷ�����. �θ� ���� �Ŵ���.
    //�ʱ�ȭ
    public void Load(string tab, string key)
    {
        userInfoJson = JsonUtility.FromJson<UserInfoJson>(SaveLoadManager.Instance.Load(tab, key, SaveLoadManager.Risks.None));
        if (userInfoJson == null)
        {
            userInfoJson = new UserInfoJson();
            Save();
        }

        userSkillDic = new Dictionary<string, UserSkillInfo>();
        foreach(var skill in userInfoJson.userSkillList)
            userSkillDic.Add(skill.skillName, skill);

        this.tab = tab;
        this.key = key;
    }


    public void Save()
    {
        SaveLoadManager.Instance.Save(tab, key, JsonUtility.ToJson(userInfoJson), SaveLoadManager.Risks.Error);
    }

    [System.Serializable]
    public class UserInfoJson
    {
        //���� ���� ������ �з��� �� ���� ����
        public EClassification classification;
        public string userName;
        public int level;
        public float currentExp; // 0~1
        public int hp;
        public int mp;

        ///////////////////////////////////////////////////////////////>
        //��ų ����
        public List<UserSkillInfo> userSkillList;

        public UserInfoJson()
        {
            userSkillList = new List<UserSkillInfo>();
        }
        ///////////////////////////////////////////////////////////////>
    }

    [System.Serializable]
    public class UserSkillInfo 
    {
        public string skillName;
        public int level;

        public UserSkillInfo() { }
        public UserSkillInfo(in string skillName, int level)
        {
            this.skillName = skillName;
            this.level = level;
        }
    }
    //////////////////////////////////////////////////////////////////////////>
    

    //////////////////////////////////////////////////////////////////////////>
    // �� enum�� ���߿� ������ ���� �ʿ� ���� �������ִ� Ŭ������ ��������� �������� �ű� ����
    // �������ִ� ����
    // 1. UI -> ��ųƮ�� ������
    // 2. ������ ���� level�� ���� ��ȭ ��ҵ�
    public enum EClassification
    {
        //���
        Warrior,
        Assassine,
        Archer,
        //����
        Oak
    }
    //////////////////////////////////////////////////////////////////////////>
}
