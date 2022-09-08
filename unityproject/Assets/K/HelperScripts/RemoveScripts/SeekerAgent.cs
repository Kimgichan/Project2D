using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerAgent : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackDist;
    [SerializeField] private float targetDist;
    [SerializeField] private float bellicosity;
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
        controller.OrderPlayAI(target, attackDist, targetDist, bellicosity);
    }
}
