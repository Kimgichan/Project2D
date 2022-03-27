using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSelectMapInfo : MonoBehaviour
{
    [Header("Enterance List")]
    public List<ZoneWarpZone> warpZoneList = new List<ZoneWarpZone>();
    [Header("Player Spawn Transform")]
    public Transform playerSpawnTr;
}
