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
        // transform.rotation.z�����δ� 360'�� ������ �� ����. ���� eulerAngles�� ���
        // ������ ���⺸�� 2�� ���� ���⿡ 2�� �������־���.
        rotationZ = transform.rotation.eulerAngles.z / 2;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotationZ);
        //Invoke("DestroyBullet", 2);
    }

    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    // ������Ʈ �����Լ�
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D _other)
    {
        // ���ݻ����ΰ�? �� Bullet�� ���ư��� �����ΰ�?
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
                Debug.Log("�浹");
                speed = 0.0f;
                isAttacState = false;
            }
        }
        else
        {
            //�÷��̾ Bullet�� ȸ���Ѵ�
            if (_other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}
