using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    SkillTree parent;
    Button btn;
    SkillData skill;
    Image icon;
    bool select = false;

    public string SkillName => name;
    public string ToolTip => skill.Tooltip;
    public Sprite Icon => skill.Icon;
    
    [HideInInspector] public int level;

    private void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        btn = transform.GetChild(0).GetComponent<Button>();

        //게임 매니저에서 관리하는 데이터로 공개되면 좀더 알아보기 쉽게 변경될 예정
        parent = transform.parent.parent.parent.parent.GetComponent<SkillTree>();

        btn.onClick.AddListener(() =>
        {
            if (select)
            {
                //포커싱 이후에 클릭 이벤트
                if (parent.Pass(this))
                {
                    level += 1;
                }
            }
            else
            {
                var infoPanel = parent.SkillTooltipPanel;
                infoPanel.Close();
                //한 번 클릭은 포커싱
                select = true;
                infoPanel.targetSlot = this;
                infoPanel.Content = this.skill.Tooltip;
                infoPanel.transform.SetParent(transform);
                infoPanel.gameObject.SetActive(true);
            }
        });

        var skill = GameManager.Instance.GetSkill(SkillName);
        this.skill = skill;
        icon.sprite = skill.Icon;
        level = 0;
    }


    // 선택이 풀렸을 때
    public void Unselected()
    {
        select = false;
    }
}
