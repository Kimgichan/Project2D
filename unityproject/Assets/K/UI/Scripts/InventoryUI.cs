using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

public class InventoryUI : MonoBehaviour
{
    #region ���� ���
    [SerializeField] private List<ItemSlot> slots;
    [ReadOnly] [SerializeField] private InventoryDecorator inventory;
    #endregion


    #region ������Ƽ ���
    #endregion


    #region �Լ� ���


    public bool Open()
    {
        var board = GameManager.Instance.board;
        var user = GameManager.Instance.playerController;

        if(board == null || user == null)
        {
            gameObject.SetActive(false);
            return false;
        }

        if(!user.TryGetDecorator(Enums.Decorator.Inventory, out Component val))
        {
            gameObject.SetActive(false);
            return false;
        }

        gameObject.SetActive(true);

        inventory = val as InventoryDecorator;
        return true;
    }

    public void Close()
    {
        inventory = null;
        gameObject.SetActive(false);
    }

    [Button("�κ��丮 ������Ʈ")] public void InventoryUpdate()
    {
        if(!gameObject.activeSelf || inventory == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var destCount = inventory.GetItemCount();
        var currentCount = slots.Count;


        // destCount > currentCount �� ��
        for(int i = 0, icount = destCount - currentCount; i<icount; i++)
        {
            slots.Add(Instantiate(slots[0], slots[0].transform.parent));
        }


        // destCount < currentCount �� ��
        for(int i = currentCount - 1; i>=destCount; i--)
        {
            Destroy(slots[i]);
        }


        for(int i = 0; i<destCount; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Item = inventory.GetItem(i);
        }
    }

    #endregion
}
