using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class InventoryDecorator : MonoBehaviour
{
    #region 변수 목록
    /// <summary>
    /// 사망할 경우, 소지하고 있던 아이템을 떨어뜨릴 것인지<br/>
    /// true면 떨어뜨림
    /// </summary>
    [SerializeField] private bool drop;

    /// <summary>
    /// 열의 카운드.
    /// </summary>
    [SerializeField] private int colCount;


    /// <summary>
    /// 이거 수정할 예정
    /// </summary>
    //[ReadOnly] [SerializeField] List<Item> items;

    [ReadOnly] [SerializeField] List<Nodes.ItemInfo> itemInfos;
    private List<InterfaceList.Item> items;
    #endregion


    #region 프로퍼티 목록
    #endregion


    #region 함수 목록

    private void Start()
    {
        //items = new List<Item>();
        items = new List<InterfaceList.Item>();
    }


    #region 슬롯 추가, 변경

    /// <summary>
    /// Item을 '전부' 인벤토리에 담았다면 true값 리턴
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(InterfaceList.Item item)
    {
        if (item == null) return false;
        if (item.CurrentCount < 1) return false;

        int firstSlotIndx = -1;
        for(int i = 0, icount = items.Count; i<icount; i++)
        {
            if (items[i] == null)
            {
                if (firstSlotIndx < 0)
                    firstSlotIndx = i;
                continue;
            }

            var currentItem = items[i];

            if(currentItem.Kind == item.Kind)
            {

                item.CurrentCount = AddCount(currentItem, item.CurrentCount);

#if UNITY_EDITOR
                itemInfos[i].From(currentItem);
#endif

                if(item.CurrentCount <= 0)
                {
                    item.Destroy();
                    return true;
                }
            }
        }

        if(firstSlotIndx > -1)
        {
            items[firstSlotIndx] = item;

#if UNITY_EDITOR
            itemInfos[firstSlotIndx].From(item);
#endif

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// indx의 범위는 0 ~ items 카운트
    /// </summary>
    /// <param name="currentItemIndx"></param>
    /// <param name="destItemIndx"></param>
    /// <param name="moveCount">current에서 dest로 이동하는 아이템 개수</param>
    public void MoveItem(int currentItemIndx, int destItemIndx, int moveCount)
    {
        if (moveCount <= 0) return;
        if(moveCount > items[currentItemIndx].MaxCount)
        {
            moveCount = items[currentItemIndx].MaxCount;
        }


        if(items[destItemIndx] == null)
        {
            items[destItemIndx] = items[currentItemIndx];
            items[currentItemIndx].CurrentCount -= moveCount;

            if (items[currentItemIndx].CurrentCount <= 0)
            {
                items[currentItemIndx] = null;
            }
        }
        else if(items[currentItemIndx].Kind != items[destItemIndx].Kind)
        {
            var swap = items[currentItemIndx];
            items[currentItemIndx] = items[destItemIndx];
            items[destItemIndx] = swap;
        }
        else
        {
            var currentCount = items[currentItemIndx].CurrentCount;

            currentCount -= moveCount;
            currentCount += AddCount(
                items[destItemIndx],
                moveCount);

            items[currentItemIndx].CurrentCount = currentCount;

            if(items[currentItemIndx].CurrentCount <= 0)
            {
                items[currentItemIndx].Destroy();
                items[currentItemIndx] = null;
            }
        }

#if UNITY_EDITOR
        itemInfos[currentItemIndx].From(items[currentItemIndx]);
        itemInfos[destItemIndx].From(items[destItemIndx]);
#endif
    }

    /// <summary>
    /// 반환값은 add하고 남은 개수, 남지 않았으면 0리턴
    /// </summary>
    /// <param name="item"></param>
    /// <param name="addCount"></param>
    /// <returns></returns>
    private int AddCount(InterfaceList.Item item, int addCount)
    {
        if (addCount < 0) return 0;

        if (item.CurrentCount >= item.MaxCount)
        {
            item.CurrentCount = item.MaxCount;
            return addCount;
        }

        var restCount = addCount - (item.MaxCount - item.CurrentCount);


        item.CurrentCount += addCount;
        if (item.CurrentCount > item.MaxCount)
        {
            item.CurrentCount = item.MaxCount;
        }

        if (restCount > 0)
            return restCount;
        else return 0;
    }
    #endregion

    /// <summary>
    /// 인벤토리에 있는 모든 장비를 떨어뜨림
    /// </summary>
    public void Drop()
    {
        if (!drop) return;

        // 여긴 이펙트로 처리해야 되는 부분



    }


    public int GetItemCount() => items.Count;

    public InterfaceList.Item GetItem(int i) => items[i];


    /// <summary>
    /// 지금은 슬롯 확장만 가능<br/>
    /// 아이템 떨어트리는 기능 구현되면 축소도 추가될 예정<br/>
    /// </summary>
    /// <param name="rowCount"></param>
    public void SlotSetting(int rowCount)
    {
        var slotCount = rowCount * colCount;

        if(slotCount > items.Count)
        {
            var addCount = slotCount - items.Count;
            for(int i = 0; i<addCount; i++)
            {
                items.Add(null);
            }
        }

#if UNITY_EDITOR
        if(slotCount > itemInfos.Count)
        {
            var addCount = slotCount - itemInfos.Count;
            for(int i = 0; i<addCount; i++)
            {
                itemInfos.Add(Nodes.ItemInfo.Empty);
            }
        }
#endif
    }


    #endregion
}
