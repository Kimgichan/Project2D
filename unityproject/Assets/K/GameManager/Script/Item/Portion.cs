using InterfaceList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portion : InterfaceList.Item
{
    #region 변수 목록
    private PortionData portionData;
    private int currentCount;
    #endregion



    #region 프로퍼티 목록

    #region Item 인터페이스

    public string Name => portionData.name;

    public Enums.ItemKind Kind => Enums.ItemKind.Portion;

    public bool Use
    {
        get
        {
            return false;
        }

        set
        {

        }
    }

    public int CurrentCount
    {
        get
        {
            return currentCount;
        }
        set
        {
            if (value > portionData.MaxSlotCount)
            {
                value = portionData.MaxSlotCount;
            }
            else if (value < 0) value = 0;

            currentCount = value;
        }
    }

    public int MaxCount => portionData.MaxSlotCount;

    public Sprite Icon => portionData.ItemIcon;

    public string Content
    {
        get
        {
            return portionData.Content;
        }
    }

    #endregion

    #endregion



    #region 함수 목록

    public Portion(PortionData portionData)
    {
        this.portionData = portionData;
        CurrentCount = 0;
    }

    #region Item 인터페이스

    public void Destroy()
    {

    }

    public void Drop()
    {

    }

    public InterfaceList.Item Copy()
    {
        var newItem = new Portion(portionData);
        return newItem;
    }

    public bool Equal(Item item)
    {
        var portion = item as Portion;
        if (portion == null) return false;

        return portionData.Equals(portion.portionData);
    }

    #endregion

    #endregion
}
