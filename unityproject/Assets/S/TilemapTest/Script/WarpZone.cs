using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("WarpZone�� OnTriggerEnter �۵� : " + other.name);
        //WarpPlayer(other.gameObject);
    }
    ////////////////////
    //�����ϴ� ������ ���� ����
    //[Header("Warp to Exist")]
    //public GameObject destination;

    //public void WarpPlayerToExist(GameObject go)
    //{
    //    go.gameObject.transform.position = destination.transform.position;
    //}


    ////////////////////
    //���Ӱ� ������ ������ ���� ����
    [Header("Warp to Create")]
    //���� ��
    public MapInfo currentMap;
    //���� ��
    public MapInfo destinationMap;
    public void WarpPlayerToNew(GameObject go)
    {
        MapInfo temp = Instantiate(destinationMap);
        go.gameObject.transform.position = temp.enterance.transform.position;
        Destroy(currentMap.gameObject);
    }
}
