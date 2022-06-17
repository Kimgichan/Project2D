using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerAgent : MonoBehaviour
{
    [SerializeField] private Transform target;  
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCor());
    }

    IEnumerator StartCor()
    {
        yield return new WaitForSeconds(1f);

        var controller = GetComponent<ObjectController>();
        //controller.OrderPathFind(target);
        controller.OrderPlayAI(target, 1f, 1f, 0.7f);
    }
}
