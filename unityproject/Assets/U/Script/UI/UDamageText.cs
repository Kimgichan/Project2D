using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UDamageText : MonoBehaviour
{
    public float moveSpeed;
    // �������� �ӵ�
    public float alphaSpeed;
    // ������Ʈ ���� �ð�
    public float destoryTime;
    public int damage;

    TextMeshProUGUI text;
    Color alpha;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = damage.ToString();
        alpha = text.color;
        destoryTime = 1.0f;

        // �ش� ������Ʈ �Ӹ� ���� �������� Y��ǥ +
        transform.position = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);

        Invoke("DestoryObject", destoryTime);

    }

    void Update()
    {
        // Y��ǥ�� TEXT�̵�
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        // ���� ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    // ������Ʈ�� �����ϴ� �Լ�
    private void DestoryObject()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
}
