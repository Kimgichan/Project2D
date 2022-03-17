using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    //singleton
    private static ZoneManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static ZoneManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    ////////////////////////////////////////////
    
    public ZoneSelectMapInfo zoneSelectMap;
    public ZoneInfo currentZoneInfo;
    public List<ZoneInfo> zoneInfoList;

    public void WarpToNextZone(ZoneInfo zoneinfo, GameObject go)
    {
        currentZoneInfo = Instantiate(zoneinfo);
        //Destroy(zoneSelectMap.gameObject);
        zoneSelectMap.gameObject.SetActive(false);
        //go.transform.position = currentZoneInfo.currentMap.enterance.transform.position;
        go.transform.position = new Vector3(0, 0, 0);
    }
    public void ReturnToZoneSelectMap(GameObject go)
    {
        //zoneSelectMap = Instantiate(zoneSelectMap);
        zoneSelectMap.gameObject.SetActive(true);
        //move player
        go.gameObject.transform.position =  zoneSelectMap.playerSpawnTr.position;
    }
}
