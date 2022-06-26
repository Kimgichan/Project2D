using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 출처 : https://gall.dcinside.com/mgallery/board/view/?id=game_dev&no=66595 <br/>
/// <br/>
/// ScriptableObject 처럼 코루틴(모노비헤이비어를 상속받지 않아서)을<br/> 
/// 사용할 수 없는 객체가 코루틴을 사용할 수 있도록 서포트하는 싱글톤 클래스<br/>
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
