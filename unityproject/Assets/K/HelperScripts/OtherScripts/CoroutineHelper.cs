using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��ó : https://gall.dcinside.com/mgallery/board/view/?id=game_dev&no=66595 <br/>
/// <br/>
/// ScriptableObject ó�� �ڷ�ƾ(�������̺� ��ӹ��� �ʾƼ�)��<br/> 
/// ����� �� ���� ��ü�� �ڷ�ƾ�� ����� �� �ֵ��� ����Ʈ�ϴ� �̱��� Ŭ����<br/>
/// </summary>
[ExecuteInEditMode]
public class CoroutineHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            if (monoInstance != null)
            {
                Destroy(gameObject);
                return;
            }
        }

        monoInstance = this;

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(monoInstance);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
            monoInstance = this;
    }
#endif

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
}
