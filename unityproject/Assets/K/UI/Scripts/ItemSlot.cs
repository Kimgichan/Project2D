using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class ItemSlot : Effect
{
    #region 변수 목록
    [SerializeField] private Image icon;
    [SerializeField] private Text countState;
    private InterfaceList.Item item;
    #endregion


    #region 프로퍼티 목록
    public InterfaceList.Item Item
    {
        get => item;
        set
        {
            if (value == null)
            {
                item = null;

                return;
            }

            item = value;

            icon.sprite = item.Icon;
            countState.text = $"{item.CurrentCount}/{item.MaxCount}";
        }
    }
    #endregion


    #region 함수 목록
    public void ItemUpdate()
    {
        Item = Item;
    }
    public void ItemUpdate(InterfaceList.Item item)
    {
        Item = item;
    }
    #endregion
}
