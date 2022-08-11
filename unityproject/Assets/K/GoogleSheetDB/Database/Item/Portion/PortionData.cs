using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionData", menuName = "Scriptable Object/Item/Portion", order = int.MaxValue)]
public class PortionData : ScriptableObject
{
    #region ����
    [SerializeField] private Enums.PortionKind portionKind;
    [SerializeField] private Sprite itemIcon;
    #endregion


    #region ������Ƽ
    public Enums.PortionKind PortionKind => portionKind;
    public Sprite ItemIcon => itemIcon;
    #endregion


    #region �Լ�
    #endregion
}
