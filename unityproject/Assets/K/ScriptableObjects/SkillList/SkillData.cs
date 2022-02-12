using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;

    [TextArea][SerializeField] string content = "(����� �ٽ� �ۼ����ּ���) /ex> �� ��ų�� {0} ~ {1}�� �������� �ش�. / Help> needSP�� �ʿ� SP, nums�� ���Ǵ� {n} ���� ��, {0}~{n}�� �������� ��� �����ϴ�. ������ ��� ����(int), Feature�� ������ �� ��ų�� Max Level�̴�.";
    public string Content(int level) => string.Format(content, feature[level].nums);
    public int MaxLevel => feature.Count;
    public int NextNeedSP(int level) => feature[level + 1].needSP;

    [SerializeField] List<Feature> feature;
    public int NumCount(int level) => feature[level].nums.Count;
    public int NumValue(int level, int indx) => feature[level].nums[indx];

    [System.Serializable] public class Feature
    {
        public int needSP = 0;
        public List<int> nums;
    }
}
