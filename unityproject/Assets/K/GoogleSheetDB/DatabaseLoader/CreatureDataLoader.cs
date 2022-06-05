#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// 인게임에서 실행되는 함수는 아님. 
/// 게임이 실행되기 위해 필요한 데이터를 Update하는데 사용되는 함수.
/// 구글 시트에 테이블 정보를 Text로 뽑아서 csv에 넣어줄 것
/// </summary>
public class CreatureDataLoader : MonoBehaviour
{
    [SerializeField] List<CreatureData> datas;
    [SerializeField] TextAsset csv;

    // Start is called before the first frame update
    void Start()
    {
        var lines = csv.text.Split('\n');
        for(int i = 0, icount = datas.Count; i<icount; i++)
        {
            var path = AssetDatabase.GetAssetPath(datas[i].GetInstanceID());
            var values = lines[i + 1].Split(',');

        }
    }
}
#endif
