using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portion : InterfaceList.Item
{
    #region 변수 목록

    #endregion



    #region 프로퍼티 목록

    #region Item 인터페이스

    public string Name => "";

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
            return 0;
        }
        set
        {

        }
    }

    public int MaxCount => 1;

    public Sprite Icon => null;

    public string Content
    {
        get
        {
            return "";
        }
    }

    #endregion

    #endregion



    #region 함수 목록

    #region Item 인터페이스

    public void Destroy()
    {

    }

    public void Drop()
    {

    }

    public InterfaceList.Item Copy()
    {
        var newItem = new Portion();
        return newItem;
    }

    #endregion

    #endregion
}
