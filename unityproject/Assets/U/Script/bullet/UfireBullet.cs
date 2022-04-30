using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfireBullet : MonoBehaviour
{
    public float speed = 0.2f;
    public float damage = 10.0f;
    private float rotationZ;
    private bool isAttacState = true;

    void Start()
    {
        // transform.rotation.z만으로는 360'를 가져올 수 없다. 따라서 eulerAngles를 사용
        // 생각한 방향보다 2배 값이 높기에 2로 나누어주었다.
        rotationZ = transform.rotation.eulerAngles.z / 2;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotationZ);
        //Invoke("DestroyBullet", 2);
    }

    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    // 오브젝트 제거함수
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D _other)
    {
        // 공격상태인가? 즉 Bullet이 날아가는 상태인가?
        if (isAttacState)
        {
            if (_other.gameObject.tag == "EnemyHitBox")
            {
                _other.GetComponent<UHitCollider>().myEnemy.hitHP(damage);
                speed = 0.0f;
                isAttacState = false;
            }

            if (_other.gameObject.tag == "Wall")
            {
                Debug.Log("충돌");
                speed = 0.0f;
                isAttacState = false;
            }
        }
        else
        {
            //플레이어가 Bullet을 회수한다
            if (_other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}
