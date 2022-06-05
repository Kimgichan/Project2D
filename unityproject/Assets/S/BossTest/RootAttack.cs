using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttack : MonoBehaviour
{
    public float lifeTime = 5.0f;
    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

}
