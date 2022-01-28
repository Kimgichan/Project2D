using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckTest : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision collision)
    {
        Debug.Log(collision.gameObject.tag + "�� OnCollisionEnter ȣ���");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag + "�� OnTriggerEnter ȣ���");
        TestWarp(collision.gameObject);
    }
    private void TestWarp(GameObject go)
    {
        go.GetComponent<WarpZone>().WarpPlayer(this.gameObject);
    }
}
