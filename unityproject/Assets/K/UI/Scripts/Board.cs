using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Board : MonoBehaviour
{
    [SerializeField] private GameDatabase game_db;
    [SerializeField] private Button menuOpenBtn;
    [SerializeField] private GraphicRaycaster ray;

    [Header("메뉴 보드")]
    [SerializeField] private GameObject menuBoard;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

    [Header("스킬 보드")]
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

    [Header("스킬 슬롯 등록 팜업창")]
    [SerializeField] private GameObject skillEnterPomup;
    [SerializeField] private List<Button> skillEnterSlotBtn;
    [SerializeField] private List<Image> skillEnterSlotIcon;

    [Header("조이스틱 패드")]
    [SerializeField] private GameObject inputBoard;
    [SerializeField] private VirtualJoystick moveBtn;
    [SerializeField] private List<EventTrigger> portionSlotList;
    [SerializeField] private VirtualJoystick attackBtn;
    [SerializeField] private List<VirtualJoystick> skillSlotList;
    [SerializeField] private List<Image> skillSlotList_image;
    [SerializeField] private List<CooltimeSlot> skillSlotList_coolTime;
    [SerializeField] private EventTrigger cancelBtn;
    [SerializeField] private Image rightJoystick;
    [SerializeField] private Image rightJoystickHandle;
    

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

    private bool normalAttack = false;

    private int CurrentSP
    {
        get
        {
            return game_db.PlayerSP;
        }
        set
        {
            game_db.PlayerSP = value;
            sp_text.text = $"{game_db.PlayerSP}SP";
        }
    }
    private void Start()
    {

        //백 이벤트
        UnityAction menuBoard_To_menuOpenBtn;
        UnityAction skillBoard_To_menuBoard;
        UnityAction<SkillNode> skillBtnClickEvent = (skill) =>
        {
            CurrentSkillNode = skill;
            skillNameTxt.text = skill.Skill.name;
            // 유저 정보가 있으면 => level
            var level = game_db.GetPlayerSkillLevel(CurrentClassTabBtn.ClassName, skill.Skill);
            skillLevelTxt.text = $"LV. {level}";
            // 유저 정보가 있으면 => content
            if(level <= 1)
            {
                if (level < 1 || skill.Skill.Passive)
                {
                    skillSettingBtn.gameObject.SetActive(false);
                }
                else skillSettingBtn.gameObject.SetActive(true);

                skillContentTxt.text = skill.Skill.Content(0);
            }
            else
            {
                if (skill.Skill.Passive)
                    skillSettingBtn.gameObject.SetActive(false);
                else
                    skillSettingBtn.gameObject.SetActive(true);

                //값이 0부터 시작하는 인덱스라서 - 1 해줌
                skillContentTxt.text = skill.Skill.Content(level - 1);
            }

            if (level < skill.Skill.MaxLevel)
            {
                skillUpBtn.gameObject.SetActive(true);
                receiptSP_txt.text = $"{CurrentSP}/{skill.Skill.NeedSP(level)}"; // level은 인덱스기 때문에 - 1을 해주지 않으면 nextLevel임
            }
            else
                skillUpBtn.gameObject.SetActive(false);

        };
        UnityAction<ClassTab> skillTabBtnClickEvent = (classTab) =>
        {
            CurrentClassTabBtn = classTab;
            CurrentSkillNode = null;
            var skills = game_db.GetClassSkills(classTab.ClassName);
            var currentCount = skillNodeList.Count;
            var destCount = skills.Count;
            var color = game_db.GetClassColor(classTab.ClassName);

            //destCount > currentCount => 추가 생성
            for (int i = 0, icount = destCount - currentCount; i < icount; i++)
            {
                var newNode = Instantiate(skillNodeList[0], skillNodeList[0].transform.parent);
                newNode.clickEvent = skillBtnClickEvent;
                skillNodeList.Add(newNode);
            }

            //destCount < currentCount => 비활성화
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

            if (destCount == 0)
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
            inputBoard.gameObject.SetActive(true);
            backBtn.gameObject.SetActive(false);
        };

        inputBoard.gameObject.SetActive(true);
        menuOpenBtn.gameObject.SetActive(true);
        menuOpenBtn.onClick.AddListener(() =>
        {
            menuOpenBtn.gameObject.SetActive(false);
            inputBoard.gameObject.SetActive(false);
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
            CurrentSP = game_db.PlayerSP;

            // 스킬트리 셋팅 처음

            var currentCount = classTabs.Count;
            var destCount = game_db.GetClassCount();

            // destCount > currentCount => 추가 클래스 탭 생성
            for (int i = 0, icount = destCount - currentCount; i < icount; i++)
            {
                var newClassTab = Instantiate(classTabs[0], classTabs[0].transform.parent);
                newClassTab.clickEvent = skillTabBtnClickEvent;
                classTabs.Add(newClassTab);
            }


            // currentCount > destCount => 비활성화
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


        // 스킬 업 되면 저장됨.
        skillUpBtn.onClick.AddListener(() =>
        {
            try
            {
                var needSP = CurrentSkillNode.Skill.NeedSP(game_db.GetPlayerSkillLevel(CurrentClassTabBtn.ClassName, CurrentSkillNode.Skill));
                if (CurrentSP > needSP)
                {
                    CurrentSP -= needSP;
                    game_db.AddPlayerSkill(CurrentClassTabBtn.ClassName, CurrentSkillNode.Skill);
                    skillBtnClickEvent(CurrentSkillNode);
                }
            }
            catch
            {}
        });

        skillBoard.SetActive(false);

        backBtn.gameObject.SetActive(false);
        backBtn.onClick.AddListener(() => backEvent());

        {
            // AttackBtn
            {
                UnityAction baseJoystickColorSet = () =>
                {
                    ColorUtility.TryParseHtmlString("#00F8FF", out Color col);
                    rightJoystick.color = col;
                    rightJoystickHandle.color = col;
                };
                UnityAction burnJoystickColorSet = () =>
                {
                    ColorUtility.TryParseHtmlString("#FF5F69", out Color col);
                    rightJoystick.color = col;
                    rightJoystickHandle.color = col;
                };
                UnityAction<int> coolTimeAction = (n) =>
                {
                    var skillInfo = game_db.SkillSlot(n);
                    if (skillInfo != null && (!skillSlotList_coolTime[n].gameObject.activeSelf))
                    {
                        skillSlotList_coolTime[n].cooltime = (float)skillInfo.skillInfo.skill.NumValue(skillInfo.skillInfo.level - 1,
                                skillInfo.skillInfo.skill.NumCount(skillInfo.skillInfo.level - 1) - 1);
                        skillSlotList_coolTime[n].gameObject.SetActive(true);
                    }
                };

                var entryPointerEnter = new EventTrigger.Entry();
                entryPointerEnter.eventID = EventTriggerType.PointerEnter;
                entryPointerEnter.callback.AddListener((e) =>
                {
                    burnJoystickColorSet();
                });
                cancelBtn.triggers.Add(entryPointerEnter);

                var entryPointerExit = new EventTrigger.Entry();
                entryPointerExit.eventID = EventTriggerType.PointerExit;
                entryPointerExit.callback.AddListener((e) =>
                {
                    baseJoystickColorSet();
                });
                cancelBtn.triggers.Add(entryPointerExit);

                cancelBtn.gameObject.SetActive(false);

                Func<PointerEventData, bool> cancelCheck = (e) =>
                {
                    List<RaycastResult> results = new List<RaycastResult>();
                    ray.Raycast(e, results);

                    foreach (var result in results)
                    {
                        if (result.gameObject == cancelBtn.gameObject)
                        {
                            Debug.Log("캔슬");
                            return true;
                        }
                    }
                    return false;
                };

                attackBtn.PointerDown = (e) =>
                {
                    baseJoystickColorSet();

                    foreach (var skill in skillSlotList)
                        skill.enabled = false;
                    normalAttack = false;
                };

                attackBtn.Drag = (v) =>
                {
                    if (v.sqrMagnitude >= 0.98f)
                    {
                        if (!normalAttack)
                        {
                            normalAttack = true;
                            burnJoystickColorSet();
                        }
                    }
                    else
                    {
                        if (normalAttack)
                        {
                            normalAttack = false;
                            baseJoystickColorSet();
                        }
                    }
                };

                attackBtn.PointerUp = (e) =>
                {
                    foreach (var skill in skillSlotList)
                        skill.enabled = true;
                };

                for(int i =0, icount = game_db.SkillSlotCount; i<icount; i++)
                {
                    if(game_db.SkillSlot(i) != null)
                    {
                        skillSlotList_image[i].sprite = game_db.SkillSlot(i).skillInfo.skill.Icon;
                        skillSlotList_image[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        skillSlotList_image[i].gameObject.SetActive(false);
                    }
                    skillSlotList_coolTime[i].gameObject.SetActive(false);
                }

                //e가 null이면 강제로 PointerUp이 됐다는 뜻
                skillSlotList[0].PointerDown = (e) =>
                {
                    baseJoystickColorSet();

                    attackBtn.enabled = false;
                    skillSlotList[1].enabled = false;
                    skillSlotList[2].enabled = false;

                    cancelBtn.gameObject.SetActive(true);
                };
                skillSlotList[0].PointerUp = (e) =>
                {
                    attackBtn.enabled = true;
                    skillSlotList[1].enabled = true;
                    skillSlotList[2].enabled = true;

                    if (!cancelCheck(e as PointerEventData))
                    {
                        // 캔슬된게 아니면 실행이 되야함.
                        coolTimeAction(0);
                    }

                    cancelBtn.gameObject.SetActive(false);
                };

                skillSlotList[1].PointerDown = (e) =>
                {
                    baseJoystickColorSet();

                    attackBtn.enabled = false;
                    skillSlotList[0].enabled = false;
                    skillSlotList[2].enabled = false;

                    cancelBtn.gameObject.SetActive(true);
                };
                skillSlotList[1].PointerUp = (e) =>
                {
                    attackBtn.enabled = true;
                    skillSlotList[0].enabled = true;
                    skillSlotList[2].enabled = true;

                    if (!cancelCheck(e as PointerEventData))
                    {
                        // 캔슬된게 아니면 실행이 되야함.
                        coolTimeAction(1);
                    }

                    cancelBtn.gameObject.SetActive(false);
                };

                skillSlotList[2].PointerDown = (e) =>
                {
                    baseJoystickColorSet();

                    attackBtn.enabled = false;
                    skillSlotList[0].enabled = false;
                    skillSlotList[1].enabled = false;

                    cancelBtn.gameObject.SetActive(true);
                };
                skillSlotList[2].PointerUp = (e) =>
                {
                    attackBtn.enabled = true;
                    skillSlotList[0].enabled = true;
                    skillSlotList[1].enabled = true;

                    if (!cancelCheck(e as PointerEventData))
                    {
                        // 캔슬된게 아니면 실행이 되야함.
                        coolTimeAction(2);
                    }

                    cancelBtn.gameObject.SetActive(false);
                };
            }
        }

        {
            // 스킬 슬롯 등록 팜업창 셋팅
            skillEnterPomup.SetActive(false);
            skillSettingBtn.onClick.AddListener(() =>
            {
                skillEnterPomup.SetActive(true);

                for(int i =0, icount = game_db.SkillSlotCount; i<icount; i++)
                {
                    if (game_db.SkillSlot(i) != null)
                    {
                        skillEnterSlotIcon[i].sprite = game_db.SkillSlot(i).skillInfo.skill.Icon;
                        skillEnterSlotIcon[i].gameObject.SetActive(true);
                    }
                    else skillEnterSlotIcon[i].gameObject.SetActive(false);
                }

                // 백 이벤트 등록
                backEvent = () =>
                {
                    skillEnterPomup.SetActive(false);
                    backEvent = skillBoard_To_menuBoard;
                };
            });

            for(int i = 0, icount = game_db.SkillSlotCount; i<icount; i++)
            {
                var c = i;
                skillEnterSlotBtn[i].onClick.AddListener(() =>
                {
                    for(int n = 0, ncount = icount; n < ncount; n++)
                    {
                        if(game_db.SkillSlot(n) != null && game_db.SkillSlot(n).skillInfo.skill == currentSkillNode.Skill)
                        {
                            game_db.EmptySkillSlot(n);
                            skillEnterSlotIcon[n].gameObject.SetActive(false);
                            skillSlotList_image[n].gameObject.SetActive(false);
                            break;
                        }
                    }

                    var newEnter = new GameDatabase.SlotEnterSkill();
                    newEnter.className = CurrentClassTabBtn.ClassName;
                    newEnter.skillInfo = new GameDatabase.PlayerSkillInfo() { level = game_db.GetPlayerSkillLevel(newEnter.className, CurrentSkillNode.Skill), skill = CurrentSkillNode.Skill };
                    game_db.SetSkillSlot(c, newEnter);
                    skillEnterSlotIcon[c].sprite = newEnter.skillInfo.skill.Icon;
                    skillEnterSlotIcon[c].gameObject.SetActive(true);
                    skillSlotList_image[c].sprite = newEnter.skillInfo.skill.Icon;
                    skillSlotList_image[c].gameObject.SetActive(true);
                });
            }
        }
    }
}
