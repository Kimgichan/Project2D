using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    [SerializeField] private GameDatabase game_db;
    [SerializeField] private Button menuOpenBtn;

    [Header("�޴� ����")]
    [SerializeField] private GameObject menuBoard;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

    [Header("��ų ����")]
    [SerializeField] private GameObject skillBoard;
    [SerializeField] private Text sp_text;
    [SerializeField] private List<ClassTab> classTabs;
    [SerializeField] private List<SkillNode> skillNodeList;
    [SerializeField] private Text skillNameTxt;
    [SerializeField] private Text skillLevelTxt;
    [SerializeField] private Text skillContentTxt;
    [SerializeField] private Button skillUpBtn;
    [SerializeField] private Button skillSettingBtn;
    [SerializeField] private Text receiptSP_txt;
    [SerializeField] private Button backBtn;

    private UnityAction backEvent;

    private ClassTab currentClassTabBtn = null;
    private ClassTab CurrentClassTabBtn
    {
        get
        {
            return currentClassTabBtn;
        }
        set
        {
            if(value == null)
            {
                if (currentClassTabBtn != null)
                {
                    currentClassTabBtn.OutColor += new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
                    currentClassTabBtn = null;
                }
                return;
            }

            if(currentClassTabBtn == null)
            {
                currentClassTabBtn = value;
                value.OutColor -= new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
                return;
            }

            if (currentClassTabBtn == value) return;

            currentClassTabBtn.OutColor += new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
            currentClassTabBtn = value;
            value.OutColor -= new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
        }
    }

    private SkillNode currentSkillNode = null;
    private SkillNode CurrentSkillNode
    {
        get
        {
            return currentSkillNode;
        }
        set
        {
            if (value == null)
            {
                if (currentSkillNode != null)
                {
                    currentSkillNode.OutColor += new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
                    currentSkillNode = null;
                }
                return;
            }

            if (currentSkillNode == null)
            {
                currentSkillNode = value;
                value.OutColor -= new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
                return;
            }

            if (currentSkillNode == value) return;

            currentSkillNode.OutColor += new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
            currentSkillNode = value;
            value.OutColor -= new Color(btnClickDark, btnClickDark, btnClickDark, 0f);
        }
    }
    private float btnClickDark = 0.3f;
    private void Start()
    {

        //�� �̺�Ʈ
        UnityAction menuBoard_To_menuOpenBtn;
        UnityAction skillBoard_To_menuBoard;
        UnityAction<SkillNode> skillBtnClickEvent = (skill) =>
        {
            CurrentSkillNode = skill;
            skillNameTxt.text = skill.Skill.name;
            // ���� ������ ������ => level
            // ���� ������ ������ => content
        };
        UnityAction<ClassTab> skillTabBtnClickEvent = (classTab) =>
        {
            CurrentSkillNode = null;
            var skills = game_db.GetClassSkills(classTab.ClassName);
            var currentCount = skillNodeList.Count;
            var destCount = skills.Count;
            var color = game_db.GetClassColor(classTab.ClassName);

            //destCount > currentCount => �߰� ����
            for (int i = 0, icount = destCount - currentCount; i < icount; i++)
            {
                var newNode = Instantiate(skillNodeList[0], skillNodeList[0].transform.parent);
                newNode.clickEvent = skillBtnClickEvent;
                skillNodeList.Add(newNode);
            }

            //destCount < currentCount => ��Ȱ��ȭ
            for (int i = destCount; i < currentCount; i++)
                skillNodeList[i].gameObject.SetActive(false);

            {
                int i = 0;
                foreach (var skill in skills)
                {
                    skillNodeList[i].Skill = skill;
                    skillNodeList[i].OutColor = color;
                    skillNodeList[i].gameObject.SetActive(true);
                    ++i;
                }
            }

            skillNameTxt.color = color;
            skillLevelTxt.color = color;
            skillContentTxt.color = color;
            skillUpBtn.image.color = color;
            skillSettingBtn.image.color = color;
            skillNodeList[0].clickEvent(skillNodeList[0]);

            CurrentClassTabBtn = classTab;
            if(destCount == 0)
            {
                skillNameTxt.text = "Empty";
                skillLevelTxt.text = "Empty";
                skillContentTxt.text = "Empty";
                receiptSP_txt.text = "Empty";
            }
        };

        menuBoard_To_menuOpenBtn = () =>
        {
            menuBoard.SetActive(false);
            menuOpenBtn.gameObject.SetActive(true);
            backBtn.gameObject.SetActive(false);
        };

        menuOpenBtn.gameObject.SetActive(true);
        menuOpenBtn.onClick.AddListener(() =>
        {
            menuOpenBtn.gameObject.SetActive(false);
            menuBoard.SetActive(true);
            backBtn.gameObject.SetActive(true);
            backEvent = menuBoard_To_menuOpenBtn;
        });


        skillBoard_To_menuBoard = () =>
        {
            CurrentClassTabBtn = null;
            CurrentSkillNode = null;
            skillBoard.SetActive(false);
            menuBoard.SetActive(true);
            backEvent = menuBoard_To_menuOpenBtn;
        };
        menuBoard.SetActive(false);
        skillBtn.onClick.AddListener(() =>
        {
            menuBoard.gameObject.SetActive(false);
            skillBoard.gameObject.SetActive(true);
            backEvent = skillBoard_To_menuBoard;


            // ��ųƮ�� ���� ó��

            var currentCount = classTabs.Count;
            var destCount = game_db.GetClassCount();

            // destCount > currentCount => �߰� Ŭ���� �� ����
            for (int i = 0, icount = destCount - currentCount; i < icount; i++)
            {
                var newClassTab = Instantiate(classTabs[0], classTabs[0].transform.parent);
                newClassTab.clickEvent = skillTabBtnClickEvent;
                classTabs.Add(newClassTab);
            }


            // currentCount > destCount => ��Ȱ��ȭ
            for (int i = destCount; i < currentCount; i++)
                classTabs[i].gameObject.SetActive(false);

            for (int i = 0; i < destCount; i++)
            {
                classTabs[i].ClassName = game_db.GetClassName(i);
                classTabs[i].OutColor = game_db.GetClassColor(classTabs[i].ClassName);
            }

            if (destCount > 0)
            {
                CurrentClassTabBtn = classTabs[0];
                skillTabBtnClickEvent(classTabs[0]);
            }
        });


        classTabs[0].clickEvent = skillTabBtnClickEvent;
        skillNodeList[0].clickEvent = skillBtnClickEvent;


        skillBoard.SetActive(false);

        backBtn.gameObject.SetActive(false);
        backBtn.onClick.AddListener(() => backEvent());
    }
}
