using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        Vector3 newPos = Vector2.Lerp(transform.position, target.position + offset, Time.deltaTime);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
