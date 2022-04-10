using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("WarpZone의 OnTriggerEnter 작동 : " + other.name);
        this.WarpPlayerToNew(other.gameObject);
    }

    ////////////////////
    //존재하는 맵으로 워프 구현
    //[Header("Warp to Exist")]
    //public GameObject destination;

    //public void WarpPlayerToExist(GameObject go)
    //{
    //    go.gameObject.transform.position = destination.transform.position;
    //}


    ////////////////////
    //새롭게 생성된 맵으로 워프 구현
    [Header("Warp to Create")]
    //현재 맵
    public MapInfo currentMap;
    //다음 맵
    public MapInfo destinationMap;
    public void WarpPlayerToNew(GameObject go)
    {
        if (go.tag == "Player")
        {
            GameObject.FindWithTag("ZoneInfo").GetComponent<ZoneInfo>().WarpToNextMap(go);
        }
    }
}
