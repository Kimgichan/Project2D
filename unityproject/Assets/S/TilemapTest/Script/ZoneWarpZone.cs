using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWarpZone : MonoBehaviour
{
    public ZoneInfo destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ZoneWarpZone¿« OnTriggerEnter ¿€µø : " + other.name);
        this.WarpPlayerToNextZone(other.gameObject);
    }

    public void WarpPlayerToNextZone(GameObject go)
    {
        ZoneManager.Instance.WarpToNextZone(destination, go);
    }

}
