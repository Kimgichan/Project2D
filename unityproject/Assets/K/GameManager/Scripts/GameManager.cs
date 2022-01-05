using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string tab = "GameManager";
    private string key = "FileList";

    private static GameManager instance;
    public static GameManager Instance => instance;

    //DataBase ��� ����
    [SerializeField] private SkillDataBase skillDataBase;
    public int SkillCount => skillDataBase.Count;
    public SkillData GetSkill(int indx) => skillDataBase.GetSkill(indx);
    public SkillData GetSkill(in string skillName) => skillDataBase.GetSkill(skillName);
    //

    //������ �ִ� ���� �˸� ��.
    HashSet<TabKeyData> tabKeyHashSet;

    public void EnterTabKey(in string tab, in string key)
    {
        var addData = new TabKeyData(tab, key);
        if (!tabKeyHashSet.Contains(addData))
            tabKeyHashSet.Add(addData);
    }

    Dictionary<string, GameObject> dataDic;
    private Dictionary<string, Type> infoTypeDic;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        if (tabKeyHashSet == null)
        {
            tabKeyHashSet = new HashSet<TabKeyData>(new GameManagerJson.Compare());
            Save();
        }


        ////////////////////////////////////////////////////////////////////////
        //���� ������ ������ ����ִٸ� �����ϴ� �ڵ尡 �ʿ� 
        //��ü������ � ����? 
        //���� �Ŵ���(���ӸŴ��� ����)�� �ν����� ���ϴ� ������ ������ �����ϴ� ����

        ///////////////////////////////////////////////////////////////////////>

        dataDic = new Dictionary<string, GameObject>();

        infoTypeDic = new Dictionary<string, Type>()
        {
            { "User", typeof(UserInfo) }
        };

        ///////////////////////////////////////////////////////////////////>
        //tab->key ���� or ����(GameObject)
        //û�� ����
        //1. �������� ���� Ÿ��
        {
            var removeTabKeyList = new List<TabKeyData>();
            foreach (var v in tabKeyHashSet)
            {
                try
                {
                    var data = new GameObject($"{v.tab}->{v.key}");
                    var tabkeyObject = data.AddComponent(infoTypeDic[v.tab]) as ITabKeyObject;
                    data.transform.SetParent(transform);
                    tabkeyObject.Load(v.tab, v.key);
                    dataDic.Add(data.name, data);
                }
                catch
                {
                    removeTabKeyList.Add(v);
                    Debug.LogError($"{v.tab}�� infoTypeDic�� ��ϵ��� ���� Ÿ���Դϴ�.");
                }
            }

            foreach(var v in removeTabKeyList)
                tabKeyHashSet.Remove(v);
        }
        ///////////////////////////////////////////////////////////////////>
        
    }

    private void Load()
    {
        //�ҷ��� ����� �� ����� �߰�
        var lodeData = JsonUtility.FromJson<GameManagerJson>(SaveLoadManager.Instance.Load(tab, key, SaveLoadManager.Risks.None));


        //tabkey load
        tabKeyHashSet = lodeData.ToHashSet_TabKey();
    }

    private void Save()
    {
        //������ ����� �� ����� �߰�
        var gmData = new GameManagerJson();


        // tabkey save
        foreach (var tabkey in tabKeyHashSet)
            gmData.tabList.Add(tabkey);


        SaveLoadManager.Instance.Save(tab, key, JsonUtility.ToJson(gmData), SaveLoadManager.Risks.Error);
    }


    //tabkey�� ���ӸŴ������� �����Ǵ� save, load�Ǵ� ����Ÿ�� ��� �� �������̽��� ��ӹ޾ƾ���.
    public interface ITabKeyObject
    {
        public void Load(string tab, string key);
        public void Save();
    }

    [System.Serializable]
    public class GameManagerJson 
    {
        public List<TabKeyData> tabList;

        public GameManagerJson()
        {
            tabList = new List<TabKeyData>();
        }

        public HashSet<TabKeyData> ToHashSet_TabKey()
        {
            var val = new HashSet<TabKeyData>(new Compare());
            foreach(var v in tabList)
            {
                val.Add(v);
            }
            return val;
        }

        public class Compare : IEqualityComparer<TabKeyData> 
        {
            public bool Equals(TabKeyData v1, TabKeyData v2)
            {
                if (v1 == null || v2 == null)
                    return false;
                else if ($"{v1.key}{v1.tab}".Equals($"{v2.key}{v2.tab}"))
                    return false;
                else return true;
            }

            public int GetHashCode(TabKeyData v)
            {
                return $"{v.tab}{v.key}".GetHashCode();
            }
        }
    }

    [System.Serializable]
    public class TabKeyData
    {
        public string tab;
        public string key;

        public TabKeyData(string tab, string key)
        {

            this.tab = tab;
            this.key = key;
        }
    }
}
