using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class InventoryDecorator : MonoBehaviour
{
    #region ���� ���
    /// <summary>
    /// ����� ���, �����ϰ� �ִ� �������� ����߸� ������<br/>
    /// true�� ����߸�
    /// </summary>
    [SerializeField] private bool drop;

    /// <summary>
    /// ���� ī���.
    /// </summary>
    [SerializeField] private int colCount;


    /// <summary>
    /// �̰� ������ ����
    /// </summary>
    //[ReadOnly] [SerializeField] List<Item> items;

    [ReadOnly] [SerializeField] List<Nodes.ItemInfo> itemInfos;
    private List<InterfaceList.Item> items;
    #endregion


    #region ������Ƽ ���
    #endregion


    #region �Լ� ���

    private void Start()
    {
        //items = new List<Item>();
        items = new List<InterfaceList.Item>();
    }


    #region ���� �߰�, ����

    /// <summary>
    /// Item�� '����' �κ��丮�� ��Ҵٸ� true�� ����
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
    /// indx�� ������ 0 ~ items ī��Ʈ
    /// </summary>
    /// <param name="currentItemIndx"></param>
    /// <param name="destItemIndx"></param>
    /// <param name="moveCount">current���� dest�� �̵��ϴ� ������ ����</param>
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
    /// ��ȯ���� add�ϰ� ���� ����, ���� �ʾ����� 0����
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
    /// �κ��丮�� �ִ� ��� ��� ����߸�
    /// </summary>
    public void Drop()
    {
        if (!drop) return;

        // ���� ����Ʈ�� ó���ؾ� �Ǵ� �κ�



    }


    public int GetItemCount() => items.Count;

    public InterfaceList.Item GetItem(int i) => items[i];


    /// <summary>
    /// ������ ���� Ȯ�常 ����<br/>
    /// ������ ����Ʈ���� ��� �����Ǹ� ��ҵ� �߰��� ����<br/>
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
