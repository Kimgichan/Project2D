using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWarpZone : MonoBehaviour
{
    public ZoneInfo destination;
    private bool canEnter = true;
    public bool CanEnter
    {
        get { return canEnter; }
        set { canEnter = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ZoneWarpZone¿« OnTriggerEnter ¿€µø : " + other.name);
        if (canEnter)
        {
            this.WarpPlayerToNextZone(other.gameObject);
        }
        else
        {
            Debug.Log("ZoneWarpZone's canEnter == false");
        }
    }

    public void WarpPlayerToNextZone(GameObject go)
    {
        ZoneManager.Instance.WarpToNextZone(destination, go, this);
    }

}
