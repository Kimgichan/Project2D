using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    public GameObject _player;
    private Transform tr;

    public Vector3 gapBetweenPlayer = new Vector3(0, 0, -15);
    // Start is called before the first frame update
    private void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.Log("PlayerFollowCam의 _player가 비어있습니다.");
        }
        transform.position = _player.transform.position + gapBetweenPlayer;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = _player.transform.position + gapBetweenPlayer;
    }
}
