using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDamageCanvasManager : MonoBehaviour
{
    public GameObject damageTextPrefab;

    void Start()
    {
    }

    void Update()
    {
        
    }

    // �Ű������� ���߾� TMP �������� �ν��Ͻ�
    public void MessageDamage(int _damage, Transform _spawnPos)
    {
        // "DamageCanvas"�ڽ����� �ν��Ͻ�
        GameObject DamageUI =  
            Instantiate(damageTextPrefab, _spawnPos.position, Quaternion.identity, GameObject.Find("DamageCanvas").transform);

        // ����� ����
        DamageUI.GetComponent<UDamageText>().SetDamage(_damage);
    }
}

