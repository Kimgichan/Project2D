using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Scriptable Object/GameDatabase", order = int.MaxValue)]
public class GameDatabase : ScriptableObject
{
    [SerializeField] private List<ClassNode> classKind;
    private Dictionary<string, ClassFeature> classDic;
    private PlayerInfo playerInfo = new PlayerInfo();
    
    private void OnEnable()
    {
        playerInfo.sp = 100;

        for(int i = 0; i<3; i++)
        {
            playerInfo.slotEnterSkills.Add(null);
        }

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

            LoadPlayerInfo();   
        }
    }

    public Color GetClassColor(string className) => classDic[className].color;
    public SkillData GetClassSkillData(string className, string skill) => classDic[className].skillDic[skill];
    public int GetClassSkillCount(string className) => classDic[className].skillDic.Count;
    public int GetClassCount() => classDic.Count;
    public string GetClassName(int indx) => classKind[indx].className;
    public Dictionary<string, SkillData>.ValueCollection GetClassSkills(string className) => classDic[className].skillDic.Values;


    public void AddPlayerSkill(string className, SkillData skill)
    {
        if(playerInfo.skills.TryGetValue(className, out Dictionary<string, PlayerSkillInfo> skills))
        {
            if(skills.TryGetValue(skill.name, out PlayerSkillInfo value))
            {
                if (value.skill.MaxLevel > value.level)
                    ++value.level;
            }
            else
            {
                skills.Add(skill.name, new PlayerSkillInfo()
                {
                    level = 1,
                    skill = skill
                });
            }
        }
        else
        {
            playerInfo.skills.Add(className, new Dictionary<string, PlayerSkillInfo>() 
            {{ 
                skill.name, new PlayerSkillInfo() 
                {
                    level = 1, 
                    skill = skill 
                }}
            });
        }
        SavePlayerInfo_Skills();
    }
    public void RemovePlayerSkill(string className, SkillData skill)
    {
        if(playerInfo.skills.TryGetValue(className, out Dictionary<string, PlayerSkillInfo> skills))
        {
            if(skills.TryGetValue(skill.name, out PlayerSkillInfo value))
            {
                if ((--value.level) <= 0)
                {
                    skills.Remove(skill.name);
                    if(skills.Count <= 0)
                    {
                        playerInfo.skills.Remove(className);
                    }
                }
            }
        }
    }
    public int GetPlayerSkillLevel(string className, SkillData skill)
    {
        if (playerInfo.skills.TryGetValue(className, out Dictionary<string, PlayerSkillInfo> skills))
        {
            if(skills.TryGetValue(skill.name, out PlayerSkillInfo value))
            {
                return value.level;
            }
        }
        return 0;
    }


    public int PlayerSP
    {
        get
        {
            return playerInfo.sp;
        }
        set
        {
            playerInfo.sp = value;
        }
    }
    public SlotEnterSkill SkillSlot(int indx) => playerInfo.slotEnterSkills[indx];
    public int SkillSlotCount => playerInfo.slotEnterSkills.Count;

    public void EmptySkillSlot(int indx)
    {
        playerInfo.slotEnterSkills[indx] = null;
        SavePlayerInfo_Skills();
    }
    public void SetSkillSlot(int indx, SlotEnterSkill slotEnterSkill)
    {
        playerInfo.slotEnterSkills[indx] = slotEnterSkill;
        SavePlayerInfo_Skills();
    }

    public void LoadPlayerInfo()
    {
        try
        {
            var strData = File.ReadAllText($"{Application.persistentDataPath}/PlayerSkills.json");
            strData = SecurityString.Decrypt(strData, "skillTree");
            var jsonData = JsonUtility.FromJson<PlayerSkillsJson>(strData);
            playerInfo.SetPlayerSkillsJson(jsonData, this);
        }
        catch
        {
            playerInfo.skills.Clear();
            Debug.Log("스킬 정보가 비었습니다.");
        }


    }
    public void SavePlayerInfo_Skills()
    {
        var strData = JsonUtility.ToJson(playerInfo.GetPlayerSkillsJson());
        strData = SecurityString.Encrypt(strData, "skillTree");
        File.WriteAllText($"{Application.persistentDataPath}/PlayerSkills.json", strData);
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

    public class PlayerInfo
    {
        public int sp;
        public Dictionary<string, Dictionary<string, PlayerSkillInfo>> skills = new Dictionary<string, Dictionary<string, PlayerSkillInfo>>();
        public List<SlotEnterSkill> slotEnterSkills = new List<SlotEnterSkill>();


        public void SetPlayerSkillsJson(PlayerSkillsJson data, GameDatabase db)
        {
            skills.Clear();
            foreach(var playerSkillsJsonNode in data.skillTree)
            {
                var playerSkillInfos = new Dictionary<string, PlayerSkillInfo>();
                foreach(var playerSkillJsonNode in playerSkillsJsonNode.skills)
                {
                    playerSkillInfos.Add(playerSkillJsonNode.skillName, new PlayerSkillInfo()
                    {
                        level = playerSkillJsonNode.level,
                        skill = db.GetClassSkillData(playerSkillsJsonNode.className, playerSkillJsonNode.skillName)
                    });
                }
                skills.Add(playerSkillsJsonNode.className, playerSkillInfos);
            }

            sp = data.sp;

            int i = 0;
            foreach(var slot in data.slotEnterSkills)
            {
                if (!slot.className.Equals(""))
                {
                    var newSlotEnterSkill = new SlotEnterSkill();
                    newSlotEnterSkill.className = slot.className;
                    newSlotEnterSkill.skillInfo = skills[slot.className][slot.skillName];
                    slotEnterSkills[i] = newSlotEnterSkill;
                }
                ++i;
            } 
        }
        public PlayerSkillsJson GetPlayerSkillsJson()
        {
            var returnValue = new PlayerSkillsJson();
            returnValue.sp = sp;
            foreach(var slot in slotEnterSkills)
            {
                if (slot != null)
                    returnValue.slotEnterSkills.Add(new PlayerSkillsJson.SlotEnterSkillJson() { className = slot.className, skillName = slot.skillInfo.skill.name });
                else returnValue.slotEnterSkills.Add(new PlayerSkillsJson.SlotEnterSkillJson() { className = "", skillName = "" });
            }
            foreach (var skill in skills)
            {
                var playerSkillsJsonNode = new PlayerSkillsJson.PlayerSkillsJsonNode();
                foreach(var skillInfo in skill.Value)
                {
                    playerSkillsJsonNode.skills.Add(new PlayerSkillsJson.PlayerSkillJsonNode()
                    {
                        level = skillInfo.Value.level,
                        skillName = skillInfo.Key
                    });
                }
                playerSkillsJsonNode.className = skill.Key;
                returnValue.skillTree.Add(playerSkillsJsonNode);
            }

            return returnValue;
        }
    }

    [System.Serializable]
    public class PlayerSkillsJson
    {
        [System.Serializable]
        public class PlayerSkillJsonNode
        {
            public string skillName = "";
            public int level = 0;
        }
        [System.Serializable]
        public class PlayerSkillsJsonNode
        {
            public string className = "";
            public List<PlayerSkillJsonNode> skills = new List<PlayerSkillJsonNode>();
        }
        [System.Serializable]
        public class SlotEnterSkillJson
        {
            public string className;
            public string skillName;
        }

        public int sp;
        public List<SlotEnterSkillJson> slotEnterSkills = new List<SlotEnterSkillJson>();
        public List<PlayerSkillsJsonNode> skillTree = new List<PlayerSkillsJsonNode>();
    }

    public class PlayerSkillInfo
    {
        public SkillData skill;
        public int level;
    }
    public class SlotEnterSkill
    {
        public string className;
        public PlayerSkillInfo skillInfo;
    }


    [SerializeField] private WeaponDatabase weaponDB;
    public WeaponDatabase WeaponManager => weaponDB;


    [SerializeField] private ProjectileDatabase projectileDB;
    public ProjectileDatabase ProjectileDB => projectileDB;

    [SerializeField] private EffectDatabase effectDB;
    public EffectDatabase EffectDB => effectDB;
}
