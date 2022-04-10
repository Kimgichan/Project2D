using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapInfo : MonoBehaviour
{
    public enum MapState { Quest, Hunt, Result };

    [Header("Enterance")]
    public GameObject enterance;
    [Header("Exit")]
    public GameObject exit;
    [Header("Current Map")]
    public MapState currentMapState;

    [Header("Monster SpawnInfo")]
    public List<GameObject> monsterList = new List<GameObject>();
    private List<GameObject> activeMonsterList = new List<GameObject>();

    public List<GameObject> spawnPositionList = new List<GameObject>();


    private void Start()
    {
        if (currentMapState == MapState.Hunt)
        {
            SpawnMonster();
        }
    }
    private void SpawnMonster()
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            Vector3 spawnPoint = GetRandomPoint();
            GameObject temp = Instantiate(monsterList[i], spawnPoint, Quaternion.identity);
            activeMonsterList.Add(temp);
        }
    }
    private Vector3 GetRandomPoint()
    {
        //랜덤변수 생성 후 위치 수 만큼 나누고 나머지로 위치 확인
        int value = Random.Range(0, 100);
        value = value % spawnPositionList.Count;

        return spawnPositionList[value].transform.position;
    }

    private void OnDestroy()
    {
        if (activeMonsterList.Count != 0)
        {
            for (int i = 0; i < activeMonsterList.Count; i++)
            {
                Destroy(activeMonsterList[i]);
            }
        }
        activeMonsterList.Clear();
    }
}
