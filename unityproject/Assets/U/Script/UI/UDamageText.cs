using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UDamageText : MonoBehaviour
{
    public float moveSpeed;
    // 투명조절 속도
    public float alphaSpeed;
    // 오브젝트 제거 시간
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

        // 해당 오브젝트 머리 위에 나오도록 Y좌표 +
        transform.position = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);

        Invoke("DestoryObject", destoryTime);

    }

    void Update()
    {
        // Y좌표로 TEXT이동
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        // 투명도 조절
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    // 오브젝트를 제거하는 함수
    private void DestoryObject()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
}
