using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EquipKind = Enums.EquipKind;
using EquipAttribute = Enums.EquipAttribute;

[CreateAssetMenu(fileName = "EquipAttributePresetData", menuName = "Scriptable Object/Equip/AttributePreset", order = int.MaxValue)]
public class EquipAttributePresetData : ScriptableObject
{
    [SerializeField] private EquipKind kind;
    public EquipKind Kind
    {
        get => kind;
        set
        {
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(EquipAttributePresetData.Kind)�Դϴ�.");
            kind = value;
        }
    }

    [SerializeField] private List<Node> attributeList;
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
    public List<Node> AttributeList
    {
        get
        {
            Debug.LogError("���: �ΰ��ӿ����� ��ġ�� �����ϸ� �� �Ǵ� ��(EquipAttributePresetData.AttributeList)�Դϴ�.");
            return attributeList;
        }
    }

    public int AttributeCount => attributeList.Count;
    public Node GetNode(int indx) => attributeList[indx];  

    [System.Serializable]
    public struct Node
    {
        public EquipAttribute kind;
        public float percent;
    }    
}
