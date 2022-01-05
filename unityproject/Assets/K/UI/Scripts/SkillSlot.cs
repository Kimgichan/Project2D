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

        //���� �Ŵ������� �����ϴ� �����ͷ� �����Ǹ� ���� �˾ƺ��� ���� ����� ����
        parent = transform.parent.parent.parent.parent.GetComponent<SkillTree>();

        btn.onClick.AddListener(() =>
        {
            if (select)
            {
                //��Ŀ�� ���Ŀ� Ŭ�� �̺�Ʈ
                if (parent.Pass(this))
                {
                    level += 1;
                }
            }
            else
            {
                var infoPanel = parent.SkillTooltipPanel;
                infoPanel.Close();
                //�� �� Ŭ���� ��Ŀ��
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


    // ������ Ǯ���� ��
    public void Unselected()
    {
        select = false;
    }
}
