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
    /// �������� ���� �� �ִ� ���� ����.
    /// </summary>
    [SerializeField] private int slotCount;


    /// <summary>
    /// �̰� ������ ����
    /// </summary>
    //[ReadOnly] [SerializeField] List<Item> items;

#if UNITY_EDITOR

    /// <summary>
    /// items�� InterfaceList.Item�� �ν����� â���� ǥ�ð� ���� �����Ƿ� <br/>
    /// itemInfos�� ���������� ǥ�� <br/>
    /// UNITY_EDITOR ��Ͽ����� ���� ��
    /// </summary>
    [ReadOnly] [SerializeField] 
    private List<Nodes.ItemInfo> itemInfos;
#endif

    private List<InterfaceList.Item> items;
    #endregion


    #region ������Ƽ ���

    public int SlotCount => slotCount;

    #endregion


    #region �Լ� ���

    private void Start()
    {
        //items = new List<Item>();
        items = new List<InterfaceList.Item>();
    }


    #region ���� �߰�, ����

    /// <summary>
    /// �������� ����� �� ���� ����
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
                // ��(Empty) ������ ���� ��� ���� �տ� �ִ� �� ���� �ε����� Ÿ������ ����.

                // ���� �������� ī��Ʈ�� ���� ��� Ÿ�� ���Կ� �������� �������.

                if (firstSlotIndx < 0)
                    firstSlotIndx = i;
                continue;
            }

            var currentItem = items[i];

            if(currentItem.Kind == item.Kind)
            {
                // ���� ���� ������ �������� ���� ������ �����Ѵٸ�
                // �� ������ ������ ������ MAX���� ä��.
                // ������ ���� ������ Ž��.
                // ������ ���� ������ �ֺ� ������(�ٸ� ���Կ� ä�������Ƿ�)�� ����.

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
            // �������� ������ ���Ҵٸ� Ÿ�� ���Կ� �������.

            items[firstSlotIndx] = item;

#if UNITY_EDITOR
            itemInfos[firstSlotIndx].From(item);
#endif

            return true;
        }
        else
        {
            // �κ��丮�� �� ���� ���� �������� ������� �� ����.
            return false;
        }
    }

    /// <summary>
    /// �κ��丮 ������ �� ���� ����
    /// indx�� ������ 0 ~ (items ī��Ʈ - 1)
    /// </summary>
    /// <param name="currentItemIndx"></param>
    /// <param name="destItemIndx"></param>
    /// <param name="moveCount">current���� dest�� �̵��ϴ� ������ ����</param>
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
    /// destroy�� true�� �������� �������� ����(Destroy �Լ��� ȣ��)�� <br/>
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
    /// other ���� null�̸� �������� �ٴڿ� ����.<br/>
    /// other ���� null�� �ƴϸ� sendItemCount��ŭ Swap��.
    /// 
    /// other ���� my(this)�� ���ٸ� MoveItem�� ����.<br/>
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
    public void AllDrop()
    {
        if (!drop) return;

        // ���� ����Ʈ�� ó���ؾ� �Ǵ� �κ�
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
    /// �κ��丮�� Ư�� ��� '�ٴ�'�� ����߸�
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
    /// ������ ���� Ȯ�常 ����<br/>
    /// ������ ����Ʈ���� ��� �����Ǹ� ��ҵ� �߰��� ����<br/>
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
