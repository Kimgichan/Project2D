using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �� Ŭ������ �����ؼ� �������� ObjectController�� �������� �� ControllerManager�� ������ִ� �ý����� ������ ��
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
