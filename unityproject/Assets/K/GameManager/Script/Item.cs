using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

[Serializable]
public class Item
{
    #region 변수 목록
    [ReadOnly] [SerializeField] private Enums.ItemKind kind;
    [ReadOnly] [SerializeField] private object itemObject;

    /// <summary>
    /// 사용 중(장비 아이템은 착용, 소비 아이템은 퀵등록)이면 true
    /// </summary>
    [ReadOnly] [SerializeField] private bool use;
    #endregion


    #region 프로퍼티 목록
    public Enums.ItemKind Kind => kind;
    public object ItemObject => itemObject;

    /// <summary>
    /// 값이 -1이면 아이템이 아닌 잘못된 정보를 가지고 있다는 뜻. 즉, 에러.
    /// </summary>
    public int Count
    {
        get
        {
            if(kind == Enums.ItemKind.Weapon || kind == Enums.ItemKind.Armor)
            {
                return 1;
            }


            Debug.LogError("아이템이 아닌 잘못된 정보가 담김");
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


    #region 함수 목록
    public void SetItem(WeaponItem item)
    {
        itemObject = item;

        kind = Enums.ItemKind.Weapon;
    }

    #endregion
}
