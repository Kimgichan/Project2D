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
    /// 아이템을 담을 수 있는 슬롯 갯수.
    /// </summary>
    [SerializeField] private int slotCount;


    /// <summary>
    /// 이거 수정할 예정
    /// </summary>
    //[ReadOnly] [SerializeField] List<Item> items;

#if UNITY_EDITOR

    /// <summary>
    /// items의 InterfaceList.Item은 인스펙터 창에서 표시가 되지 않으므로 <br/>
    /// itemInfos로 로직적으로 표시 <br/>
    /// UNITY_EDITOR 블록에서만 사용될 것
    /// </summary>
    [ReadOnly] [SerializeField] 
    private List<Nodes.ItemInfo> itemInfos;
#endif

    private List<InterfaceList.Item> items;
    #endregion


    #region 프로퍼티 목록

    public int SlotCount => slotCount;

    #endregion


    #region 함수 목록

    private void Start()
    {
        //items = new List<Item>();
        items = new List<InterfaceList.Item>();
    }


    #region 슬롯 추가, 변경

    /// <summary>
    /// 아이템을 드랍할 때 쓰일 변수
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
                // 빈(Empty) 슬롯이 있을 경우 가장 앞에 있는 빈 슬롯 인덱스를 타깃으로 잡음.

                // 만약 아이템의 카운트가 남을 경우 타깃 슬롯에 아이템을 집어넣음.

                if (firstSlotIndx < 0)
                    firstSlotIndx = i;
                continue;
            }

            var currentItem = items[i];

            if(currentItem.Kind == item.Kind)
            {
                // 만약 같은 종류의 아이템을 담은 슬롯이 존재한다면
                // 그 슬롯의 아이탬 개수를 MAX까지 채움.
                // 남으면 다음 슬롯을 탐색.
                // 개수가 남지 않으면 텅빈 아이템(다른 슬롯에 채워졌으므로)은 삭제.

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
            // 아이템의 개수가 남았다면 타깃 슬롯에 집어넣음.

            items[firstSlotIndx] = item;

#if UNITY_EDITOR
            itemInfos[firstSlotIndx].From(item);
#endif

            return true;
        }
        else
        {
            // 인벤토리가 꽉 차서 남은 아이템을 집어넣을 수 없음.
            return false;
        }
    }

    /// <summary>
    /// 인벤토리 정리할 때 쓰일 변수
    /// indx의 범위는 0 ~ (items 카운트 - 1)
    /// </summary>
    /// <param name="currentItemIndx"></param>
    /// <param name="destItemIndx"></param>
    /// <param name="moveCount">current에서 dest로 이동하는 아이템 개수</param>
    private void MoveItem(int currentItemIndx, int destItemIndx, int moveCount)
    {
        if (moveCount <= 0) return;
        if (currentItemIndx == destItemIndx) return;
        if (items[currentItemIndx].Equals(null)) return;

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


    private void MoveItem(int myItemIndx, int sendItemCount, InventoryDecorator other, int otherItemIndx)
    {
        if (sendItemCount <= 0) return;
        if (items[myItemIndx].Equals(null)) return;

        var otherItem = other.GetItem(otherItemIndx);
        if(otherItem == null)
        {
            other.SetItem(items[myItemIndx], otherItemIndx);
            items[myItemIndx] = null;
        }
        else if(items[myItemIndx].Kind != otherItem.Kind)
        {
            other.SetItem(items[myItemIndx], otherItemIndx);
            items[myItemIndx] = otherItem;
        }
        else
        {
            sendItemCount = sendItemCount <= items[myItemIndx].CurrentCount ? 
                sendItemCount : items[myItemIndx].CurrentCount;

            items[myItemIndx].CurrentCount -= sendItemCount;
            items[myItemIndx].CurrentCount += other.AddCount(otherItem, sendItemCount);

            if(items[myItemIndx].CurrentCount <= 0)
            {
                items[myItemIndx].Destroy();
                items[myItemIndx] = null;
            }
        }


#if UNITY_EDITOR
        itemInfos[myItemIndx].From(items[myItemIndx]);
#endif
    }

    /// <summary>
    /// destroy가 true면 덮어지는 아이템을 삭제(Destroy 함수를 호출)함 <br/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="indx"></param>
    /// <param name="destroy"></param>
    private void SetItem(InterfaceList.Item item, int indx, bool destroy = false)
    {
        if (destroy)
        {
            if(items[indx] != null)
            {
                items[indx].Destroy();
            }
        }
        items[indx] = item;

#if UNITY_EDITOR
        itemInfos[indx].From(items[indx]);
#endif
    }


    /// <summary>
    /// other 값이 null이면 아이템을 바닥에 버림.<br/>
    /// other 값이 null이 아니면 sendItemCount만큼 Swap함.
    /// 
    /// other 값이 my(this)랑 같다면 MoveItem과 같음.<br/>
    /// </summary>
    /// <param name="myItemIndx"></param>
    /// <param name="sendItemCount"></param>
    /// <param name="other"></param>
    /// <param name="otherItemIndx"></param>
    public void SendItem(int myItemIndx, int sendItemCount, 
        InventoryDecorator other, int otherItemIndx)
    {
        if (sendItemCount <= 0) return;
        if (items[myItemIndx].Equals(null)) return;

        if(other == null)
        {
            Drop(myItemIndx, sendItemCount);
        }
        else if (Equals(other))
        {
            MoveItem(myItemIndx, otherItemIndx, sendItemCount);
        }
        else
        {
            MoveItem(myItemIndx, sendItemCount, other, otherItemIndx);
        }
    }

    public void RemoveItem(int indx, int count)
    {
        if (items[indx] == null) return;

        items[indx].CurrentCount -= count;

        if(items[indx].CurrentCount <= 0)
        {
            items[indx].Destroy();
            items[indx] = null;
        }

#if UNITY_EDITOR
        itemInfos[indx].From(items[indx]);
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
    public void AllDrop()
    {
        if (!drop) return;

        // 여긴 이펙트로 처리해야 되는 부분
        for(int i = 0, icount = items.Count; i<icount; i++)
        {
            var item = items[i];
            item.Drop();
        }

        items.Clear();

#if UNITY_EDITOR
        itemInfos.Clear();
#endif
    }

    /// <summary>
    /// 인벤토리에 특정 장비를 '바닥'에 떨어뜨림
    /// </summary>
    /// <param name="indx"></param>
    /// <param name="count"></param>
    public void Drop(int indx, int count)
    {
        if (items[indx] == null) return;

        if(items[indx].CurrentCount <= count)
        {
            items[indx].Drop();
            items[indx] = null;
        }
        else
        {
            var dropItem = items[indx].Copy();
            items[indx].CurrentCount = items[indx].CurrentCount - count;
            dropItem.CurrentCount = count;
            dropItem.Drop();
        }


#if UNITY_EDITOR
        itemInfos[indx].From(items[indx]);
#endif
    }

    public int GetItemCount() => items.Count;

    public InterfaceList.Item GetItem(int i) => items[i];


    /// <summary>
    /// 지금은 슬롯 확장만 가능<br/>
    /// 아이템 떨어트리는 기능 구현되면 축소도 추가될 예정<br/>
    /// </summary>
    /// <param name="slotCount"></param>
    public void SlotSetting(int slotCount)
    {
        this.slotCount = slotCount;

        if(SlotCount > items.Count)
        {
            var addCount = SlotCount - items.Count;
            for(int i = 0; i<addCount; i++)
            {
                items.Add(null);
            }
        }

#if UNITY_EDITOR
        if(SlotCount > itemInfos.Count)
        {
            var addCount = SlotCount - itemInfos.Count;
            for(int i = 0; i<addCount; i++)
            {
                itemInfos.Add(Nodes.ItemInfo.Empty);
            }
        }
#endif
    }


    #endregion
}
