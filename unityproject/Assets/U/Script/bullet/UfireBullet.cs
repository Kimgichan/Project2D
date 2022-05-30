//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UfireBullet : MonoBehaviour
//{
//    public float speed = 0.2f;
//    public float damage = 10.0f;
//    void Start()
//    {
//        Invoke("DestroyBullet", 2);
//    }


//    void Update()
//    {
//        transform.Translate(transform.up * speed * Time.deltaTime);
//    }

//    void DestroyBullet()
//    {
//        Destroy(gameObject);
//    }

//    public void OnTriggerStay2D(Collider2D _other)
//    {
//        if (_other.gameObject.tag == "EnemyHitBox")
//        {
//            _other.GetComponent<UHitCollider>().myEnemy.hitHP(damage);
//        }
//    }
//}
