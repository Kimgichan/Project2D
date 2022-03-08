using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] bool passive = true;
    public bool Passive => passive;

    [TextArea][SerializeField] string content = "(����� �ٽ� �ۼ����ּ���) /ex> �� ��ų�� {0} ~ {1}�� �������� �ش�. / Help> needSP�� �ʿ� SP, nums�� ���Ǵ� {n} ���� ��, {0}~{n}�� �������� ��� �����ϴ�. ������ ��� ����(int), Feature�� ������ �� ��ų�� Max Level�̴�. ��ų�� �нú갡 �ƴ� ��� nums�� ������ �ε����� ��Ÿ���̴�.";

    public string Content(int level)
    {
        var objects = new object[feature[level].nums.Count];
        feature[level].nums.ToArray().CopyTo(objects, 0);
        return string.Format(content, objects);
    }
    public int MaxLevel => feature.Count;
    public int NeedSP(int level) => feature[level].needSP;

    [SerializeField] List<Feature> feature;
    public int NumCount(int level) => feature[level].nums.Count;
    public int NumValue(int level, int indx) => feature[level].nums[indx];


    [System.Serializable] public class Feature
    {
        public int needSP = 0;
        public List<int> nums;
    }
}
