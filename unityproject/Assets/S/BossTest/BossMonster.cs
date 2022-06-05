using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    [Header("Boss Spec")]
    public int hp;

    [Header("Skill 1: Root Attack")]
    public int atkDamage;
    public float cooltime = 3.0f;

    public RootAttack root;
    public float atkRadius;

    private GameObject target;
    private bool canRootAtk = false;
    private bool isAtkStart = false;
    private void Awake()
    {
        DetectTarget();
        if (canRootAtk)
        {
            //Invoke("AttackTarget", cooltime);
            StartCoroutine("AttackTarget");
        }
    }
    private void Update()
    {
     
    }
    private void FixedUpdate()
    {
    
    }

    private void DetectTarget()
    {
        target = FindObjectOfType<UPlayer>().gameObject;
        canRootAtk = true;
        if (target == null)
        {
            Debug.Log("BossMonster : cant find target");
            canRootAtk = false;
        }
    }
    //private void AttackTarget()
    //{
    //    Instantiate(root, target.transform.position, Quaternion.identity);
    //}
    private IEnumerator AttackTarget()
    {
        while (canRootAtk)
        {
            Instantiate(root, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(cooltime);
        }
    }
}
