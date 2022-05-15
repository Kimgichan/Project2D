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

    // 매개변수에 맞추어 TMP 프리팹을 인스턴스
    public void MessageDamage(int _damage, Transform _spawnPos)
    {
        // "DamageCanvas"자식으로 인스턴스
        GameObject DamageUI =  
            Instantiate(damageTextPrefab, _spawnPos.position, Quaternion.identity, GameObject.Find("DamageCanvas").transform);

        // 대미지 전달
        DamageUI.GetComponent<UDamageText>().SetDamage(_damage);
    }
}

