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
    private ZoneWarpZone currentZoneWarpZone;
    //public List<ZoneInfo> zoneInfoList;

    public void WarpToNextZone(ZoneInfo zoneinfo, GameObject go, ZoneWarpZone zoneWarpZone)
    {
        currentZoneInfo = Instantiate(zoneinfo);
        zoneSelectMap.gameObject.SetActive(false);
        //go.transform.position = currentZoneInfo.currentMap.enterance.transform.position;
        go.transform.position = new Vector3(0, 0, 0);
        currentZoneWarpZone = zoneWarpZone;
    }
    public void ReturnToZoneSelectMap(GameObject go)
    {
        //zoneSelectMap = Instantiate(zoneSelectMap);
        zoneSelectMap.gameObject.SetActive(true);
        //move player
        go.gameObject.transform.position =  zoneSelectMap.playerSpawnTr.position;
    }
    public void CloseCurrentZoneWarpZone()
    {
        currentZoneWarpZone.CanEnter = false;
    }
}
