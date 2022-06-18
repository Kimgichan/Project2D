using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttack : MonoBehaviour
{
    public GameObject pre;
    public GameObject main;

    public float lifeTime = 5.0f;
    public float predictTime = 3.0f;
    private void Start()
    {
        pre.SetActive(true);
        main.SetActive(false);
        StartCoroutine("AttackAfterSeconds");
    }
    private void StartAttack()
    {
        Debug.Log("StartAttack called");
        main.SetActive(true);
        Destroy(this.gameObject, lifeTime);
    }
    IEnumerator AttackAfterSeconds()
    {
        yield return new WaitForSeconds(predictTime);
        pre.SetActive(false);
        StartAttack();
    }
}
