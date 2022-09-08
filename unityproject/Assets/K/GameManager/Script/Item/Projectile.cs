using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using InterfaceList;

public class Projectile : Item
{
    #region 변수
    private ProjectileData projectileData;
    private int currentCount;
    #endregion


    #region 프로퍼티

    #region Item
    public string Name => projectileData.name;

    public Enums.ItemKind Kind => Enums.ItemKind.Projectile;

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
        get => currentCount;
        set
        {
            if (value > projectileData.MaxSlotCount)
            {
                value = projectileData.MaxSlotCount;
            }
            else if (value < 0) value = 0;

            currentCount = value;
        }
    }

    public int MaxCount => projectileData.MaxSlotCount;

    public Sprite Icon => projectileData.ItemIcon;

    public string Content => projectileData.Content;
    #endregion
    #endregion


    #region 함수
    public Projectile(ProjectileData projectileData)
    {
        this.projectileData = projectileData;
        CurrentCount = 0;
    }


    #region Item
    public Item Copy()
    {
        var newItem = new Projectile(projectileData);
        return newItem;
    }

    public void Destroy()
    {
        
    }

    public void Drop()
    {
        
    }

    public bool Equal(Item item)
    {
        var projectile = item as Projectile;
        if (projectile == null) return false;

        return projectileData.Equals(projectile.projectileData);
    }
    #endregion
    #endregion
}
