using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour, GameManager.ITabKeyObject, SkillTree.ISkillTreeObject
{
    private string tab, key;

    UserInfoJson userInfoJson;
    Dictionary<string, UserSkillInfo> userSkillDic;


    ///////////////////////////////////////////////////////////////////////////////>
    // 스킬 정보 함수
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


    //스킬 등록 함수
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
            Debug.LogError($"UserInfo 스킬등록 함수 Error 등록 내용 => (SkillData = {skillName})/(level = {level}).");
        }
    }

    public int Count() => userInfoJson.userSkillList.Count;
    ///////////////////////////////////////////////////////////////////////////////>


    ///////////////////////////////////////////////////////////////////////////////>
    //게임 매니저 클래스에서 생성해서 불러오기. 부모도 게임 매니저.
    //초기화
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
        //대충 무슨 종인지 분류할 때 쓰일 변수
        public EClassification classification;
        public string userName;
        public int level;
        public float currentExp; // 0~1
        public int hp;
        public int mp;

        ///////////////////////////////////////////////////////////////>
        //스킬 정보
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
    // 이 enum은 나중에 종족에 따른 필요 정보 셋팅해주는 클래스가 만들어지면 그쪽으로 옮길 예정
    // 셋팅해주는 내용
    // 1. UI -> 스킬트리 프리셋
    // 2. 종족에 따른 level에 따른 변화 요소들
    public enum EClassification
    {
        //사람
        Warrior,
        Assassine,
        Archer,
        //괴물
        Oak
    }
    //////////////////////////////////////////////////////////////////////////>
}
