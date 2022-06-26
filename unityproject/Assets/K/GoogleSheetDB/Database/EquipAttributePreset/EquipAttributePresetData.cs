using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EquipKind = Enums.EquipKind;
using EquipAttribute = Enums.EquipAttribute;

[CreateAssetMenu(fileName = "EquipAttributePresetData", menuName = "Scriptable Object/Equip/AttributePreset", order = int.MaxValue)]
public class EquipAttributePresetData : ScriptableObject
{
    [SerializeField] private EquipKind kind;
    public EquipKind Kind => kind;


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

    public int AttributeCount => attributeList.Count;
    public Node GetNode(int indx) => attributeList[indx];  

    [System.Serializable]
    public struct Node
    {
        public EquipAttribute kind;
        public float percent;
    }    

    [System.Serializable]
    public struct AttributeNode
    {
        public EquipAttribute kind;
        public int count;
    }


#if UNITY_EDITOR
    public void WriteData(EquipKind kind, List<Node> attributeList)
    {
        Debug.LogError("경고: 인게임에서 수치를 변경하면 안 되는 값들입니다.");

        this.kind = kind;
        this.attributeList = attributeList;
    }
#endif
}
