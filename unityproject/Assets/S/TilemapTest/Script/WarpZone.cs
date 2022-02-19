using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{
    public GameObject destination;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("WarpZone¿« OnTriggerEnter ¿€µø : " + other.name);
        //WarpPlayer(other.gameObject);
    }
    public void WarpPlayer(GameObject go)
    {
        go.gameObject.transform.position = destination.transform.position;
    }
}
