using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag + " Collision Enter");
    }
}
