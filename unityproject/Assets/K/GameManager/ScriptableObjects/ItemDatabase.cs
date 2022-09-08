using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Object/Item/Database", order = int.MaxValue)]
public class ItemDatabase : ScriptableObject
{
    #region 변수
    [SerializeField] private List<ScriptableObject> items;
    private Dictionary<string, ScriptableObject> itemTable;
    #endregion


    #region 프로퍼티
    #endregion


    #region 함수
    private void OnEnable()
    {
        if(items != null)
        {
            itemTable = new Dictionary<string, ScriptableObject>();
            for(int i = 0, icount = items.Count; i<icount; i++)
            {
                itemTable.Add(items[i].name, items[i]);
            }
        }
    }
    public ScriptableObject GetItemData(string name)
    {
        return itemTable[name];
    }
    #endregion
}
