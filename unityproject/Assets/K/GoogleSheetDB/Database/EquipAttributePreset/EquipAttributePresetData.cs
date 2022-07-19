using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EquipKind = Enums.EquipKind;
using EquipAttribute = Enums.EquipAttribute;

[CreateAssetMenu(fileName = "EquipAttributePresetData", menuName = "Scriptable Object/Equip/AttributePreset", order = int.MaxValue)]
public class EquipAttributePresetData : ScriptableObject
{
    #region ���� ���

    [SerializeField] private EquipKind kind;
    [SerializeField] private List<Nodes.EquipAttributePercent> attributeList;

    #endregion


    #region ������Ƽ ���

    public EquipKind Kind => kind;
    public EquipAttribute RandomAttribut
    {
        get
        {
            float total = 0f;
            int icount = attributeList.Count;
            for(int i = 0; i<icount; i++)
            {
                total += attributeList[i].percent;
            }

            float choice = Random.Range(0f, total);
            total = 0f;
            for(int i = 0; i<icount; i++)
            {
                total += attributeList[i].percent;
                if(choice <= total)
                {
                    return attributeList[i].kind;
                }
            }

            return attributeList[icount - 1].kind;
        }
    } 

    public int AttributeCount => attributeList.Count;

    #endregion


    #region �Լ� ���
    public Nodes.EquipAttributePercent GetEquipAttributePercent(int indx) => attributeList[indx];


    #region ������ ���� �Լ� ���

#if UNITY_EDITOR
    public void WriteData(EquipKind kind, List<Nodes.EquipAttributePercent> attributeList)
    {
        Debug.LogError("���: �ΰ��ӿ��� ��ġ�� �����ϸ� �� �Ǵ� �����Դϴ�.");

        this.kind = kind;
        this.attributeList = attributeList;
    }
#endif
    #endregion

    #endregion
}
