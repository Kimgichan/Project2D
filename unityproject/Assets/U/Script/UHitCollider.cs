using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UHitCollider : MonoBehaviour
{
    public UEnemyRanged myEnemy;

    void Start()
    {
    }


    public void hit(float _damage)
    {
        myEnemy.hitHP(_damage);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
