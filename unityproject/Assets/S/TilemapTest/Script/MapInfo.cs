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
        //�������� ���� �� ��ġ �� ��ŭ ������ �������� ��ġ Ȯ��
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
