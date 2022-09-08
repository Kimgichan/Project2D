using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class ItemSlot : Effect
{
    #region ���� ���
    public int indx;
    [SerializeField] private Image icon;
    [SerializeField] private Text countState;
    private InterfaceList.Item item;
    #endregion


    #region ������Ƽ ���
    public InterfaceList.Item Item
    {
        get => item;
        set
        {
            if (value == null)
            {
                item = null;
                icon.gameObject.SetActive(false);
                if (countState != null)
                    countState.gameObject.SetActive(false);
                return;
            }

            item = value;

            icon.gameObject.SetActive(true);
            icon.sprite = item.Icon;

            if (countState != null)
            {
                countState.gameObject.SetActive(true);
                countState.text = $"{item.CurrentCount}/{item.MaxCount}";
            }
        }
    }
    #endregion


    #region �Լ� ���
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
