using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipAttributeData", menuName = "Scriptable Object/Equip/AttributeData", order = int.MaxValue)]
public class EquipAttributeData : ScriptableObject
{
    #region 변수 목록
    [SerializeField] private Enums.EquipAttribute attribute;
    [SerializeField] private string tooltip;
    [SerializeField] private int maxLevel;
    [SerializeField] List<float> levelValues;
    #endregion

    #region 프로퍼티 목록
    public Enums.EquipAttribute EquipAttribute => attribute;
    public int MaxLevel => maxLevel;
    #endregion

    #region 함수 목록

    public int GetLevelValuesCount() => levelValues.Count;
    public float GetLevelValue(int indx) => levelValues[indx];

    #region 유니티 전용 함수
#if UNITY_EDITOR
    public void WriteData(Enums.EquipAttribute attribute, string tooltip, int maxLevel, List<float> levelValues)
    {
        this.attribute = attribute;
        this.tooltip = tooltip;
        this.maxLevel = maxLevel;
        this.levelValues = levelValues;
    }
#endif
    #endregion
    #endregion
}
