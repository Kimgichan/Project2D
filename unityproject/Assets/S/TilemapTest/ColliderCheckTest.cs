using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag + "¿« OnTriggerEnter »£√‚µ ");
        TestWarp(collision.gameObject);
    }
    private void TestWarp(GameObject go)
    {
        //go.GetComponent<WarpZone>().WarpPlayerToExist(this.gameObject);
        //go.GetComponent<WarpZone>().WarpPlayerToNew(this.gameObject);
        //MapManager.Instance.WarpToNext(this.gameObject);
    }
}
