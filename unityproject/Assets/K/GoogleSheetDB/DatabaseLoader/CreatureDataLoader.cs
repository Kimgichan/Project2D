#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// �ΰ��ӿ��� ����Ǵ� �Լ��� �ƴ�. 
/// ������ ����Ǳ� ���� �ʿ��� �����͸� Update�ϴµ� ���Ǵ� �Լ�.
/// ���� ��Ʈ�� ���̺� ������ Text�� �̾Ƽ� csv�� �־��� ��
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
