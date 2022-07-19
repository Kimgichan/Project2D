using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 이 클래스를 참고해서 동적으로 ObjectController를 생성했을 때 ControllerManager에 등록해주는 시스템을 구현할 것
/// </summary>
public class TestEnterController : MonoBehaviour
{
    [SerializeField] private ObjectController enterController;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.ControllerManager.AddObjectController(enterController);
    }
}
