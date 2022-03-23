using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public enum MapState { Quest, Hunt, Result };

    [Header("Enterance")]
    public GameObject enterance;
    [Header("Exit")]
    public GameObject exit;
    [Header("Current Map")]
    public MapState currentMapState;
}
