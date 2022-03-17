using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneInfo : MonoBehaviour
{   
    public List<MapInfo> mapList = new List<MapInfo>();
    private Queue<MapInfo> mapQueue = new Queue<MapInfo>();

    public MapInfo currentMap;
    private GameObject currentMapGO;

    private void Start()
    {
        for(int i = 0; i < mapList.Count; i++)
        {
            mapQueue.Enqueue(mapList[i]);
        }

        if(mapQueue.Count == 0)
        {
            Debug.Log("MapManger's mapQueue is empty");
        }
        currentMap = mapQueue.Dequeue();
        currentMapGO = Instantiate(currentMap).gameObject;
    }

    public void WarpToNextMap(GameObject go)
    {
        if (mapQueue.Count == 0)
        {
            Debug.Log("MapManger's mapQueue is empty");

            //send message to ZoneManger
            //return to ZoneSelectMap -> Destroy this zoneInfo, send to ZoneSelectMap
            Destroy(currentMapGO);

            ZoneManager.Instance.ReturnToZoneSelectMap(go);
            Destroy(this.gameObject);
        }
        else
        {
            MapInfo temp = Instantiate(mapQueue.Dequeue());
            go.gameObject.transform.position = temp.enterance.transform.position;
            Destroy(currentMapGO);

            currentMap = temp;
            currentMapGO = currentMap.gameObject;
        }
    }
}
