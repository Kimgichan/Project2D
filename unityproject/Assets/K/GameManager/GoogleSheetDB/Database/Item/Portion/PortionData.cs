using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionData", menuName = "Scriptable Object/Item/Portion", order = int.MaxValue)]
public class PortionData : ScriptableObject
{
    #region 변수
    [SerializeField] private Enums.PortionKind portionKind;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string content;
    [SerializeField] private int maxSlotCount;
    #endregion


    #region 프로퍼티
    public Enums.PortionKind PortionKind => portionKind;
    public Sprite ItemIcon => itemIcon;
    public string Content => content;

    public int MaxSlotCount => maxSlotCount;
    #endregion


    #region 함수
    #endregion
}
