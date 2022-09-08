using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Object/Item/Projectile", order = int.MaxValue)]
public class ProjectileData : ScriptableObject
{
    #region ����
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string content;
    [SerializeField] private int maxSlotCount;
    #endregion


    #region ������Ƽ
    public Sprite ItemIcon => itemIcon;
    public string Content => content;
    public int MaxSlotCount => maxSlotCount;
    #endregion


    #region �Լ�
    #endregion
}
