using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portion : InterfaceList.Item
{
    #region ���� ���

    #endregion



    #region ������Ƽ ���

    #region Item �������̽�

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



    #region �Լ� ���

    #region Item �������̽�

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
