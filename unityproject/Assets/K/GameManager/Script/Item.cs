using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

[Serializable]
public class Item
{
    #region ���� ���
    [ReadOnly] [SerializeField] private Enums.ItemKind kind;
    [ReadOnly] [SerializeField] private object itemObject;

    /// <summary>
    /// ��� ��(��� �������� ����, �Һ� �������� �����)�̸� true
    /// </summary>
    [ReadOnly] [SerializeField] private bool use;
    #endregion


    #region ������Ƽ ���
    public Enums.ItemKind Kind => kind;
    public object ItemObject => itemObject;

    /// <summary>
    /// ���� -1�̸� �������� �ƴ� �߸��� ������ ������ �ִٴ� ��. ��, ����.
    /// </summary>
    public int Count
    {
        get
        {
            if(kind == Enums.ItemKind.Weapon || kind == Enums.ItemKind.Armor)
            {
                return 1;
            }


            Debug.LogError("�������� �ƴ� �߸��� ������ ���");
            return -1;
        }
    }

    public Sprite Icon
    {
        get
        {
            if(Kind == Enums.ItemKind.Weapon)
            {
                var weaponItem = ItemObject as WeaponItem;
                return weaponItem.Weapon.Icon;
            }
            

            return null;
        }
    }

    public bool Use
    {
        get
        {
            return use;
        }
        set
        {
            use = value;
        }
    }
    #endregion


    #region �Լ� ���
    public void SetItem(WeaponItem item)
    {
        itemObject = item;

        kind = Enums.ItemKind.Weapon;
    }

    #endregion
}
