using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;

    [TextArea][SerializeField] string content = "(지우고 다시 작성해주세요) /ex> 이 스킬은 {0} ~ {1}의 데미지를 준다. / Help> needSP는 필요 SP, nums는 사용되는 {n} 변수 즉, {0}~{n}의 변수까지 사용 가능하다. 변수는 모든 정수(int), Feature의 갯수가 이 스킬의 Max Level이다.";
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
