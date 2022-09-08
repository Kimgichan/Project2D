using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushItemAgent : MonoBehaviour
{
    [SerializeField] PortionData portionData0;
    [SerializeField] PortionData portionData1;
    [SerializeField] int count0;
    [SerializeField] int count1;
    [SerializeField] private InventoryDecorator inventory;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("아이템 생성");

        inventory.SlotSetting(20);

        for(int i = 0, icount = count0; i<icount; i++)
        {
            var newItem = new Portion(portionData0);
            newItem.CurrentCount = 1;

            inventory.AddItem(newItem);
        }

        for (int i = 0, icount = count1; i < icount; i++)
        {
            var newItem = new Portion(portionData1);
            newItem.CurrentCount = 1;

            inventory.AddItem(newItem);
        }
    }
}
