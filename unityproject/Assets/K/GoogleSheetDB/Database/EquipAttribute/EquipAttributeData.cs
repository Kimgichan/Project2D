using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipAttributeData", menuName = "Scriptable Object/Equip/AttributeData", order = int.MaxValue)]
public class EquipAttributeData : ScriptableObject
{
    #region ���� ���
    [SerializeField] private Enums.EquipAttribute attribute;
    [SerializeField] private string tooltip;
    [SerializeField] private int maxLevel;
    [SerializeField] List<float> levelValues;
    #endregion

    #region ������Ƽ ���
    public Enums.EquipAttribute EquipAttribute => attribute;
    public int MaxLevel => maxLevel;
    #endregion

    #region �Լ� ���

    public int GetLevelValuesCount() => levelValues.Count;
    public float GetLevelValue(int indx) => levelValues[indx];

    #region ����Ƽ ���� �Լ�
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
