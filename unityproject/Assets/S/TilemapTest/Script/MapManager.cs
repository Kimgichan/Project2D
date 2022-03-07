using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{   
    //singleton
    private static MapManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    //////////////////////////////////////////////
    
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

    public void WarpToNext(GameObject go)
    {
        if(mapQueue.Count == 0)
        {
            Debug.Log("MapManger's mapQueue is empty");
            return;
        }
        MapInfo temp = Instantiate(mapQueue.Dequeue());
        go.gameObject.transform.position = temp.enterance.transform.position;
        Destroy(currentMapGO);
        
        currentMap = temp;
        currentMapGO = currentMap.gameObject;
    }


}
