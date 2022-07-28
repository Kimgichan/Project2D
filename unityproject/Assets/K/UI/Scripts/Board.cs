using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;



public class Board : MonoBehaviour
{
    #region 변수 목록
    [SerializeField] private Button menuOpenBtn;
    [SerializeField] private GraphicRaycaster ray;

    [Header("메뉴 보드")]
    [SerializeField] private GameObject menuBoard;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button backBtn;


    [Header("조이스틱 패드")]
    [SerializeField] private GameObject inputBoard;
    [SerializeField] private VirtualJoystick moveBtn;
    [SerializeField] private List<EventTrigger> portionSlotList;
    [SerializeField] private VirtualJoystick attackBtn;
    [SerializeField] private Image rightJoystick;
    [SerializeField] private Image rightJoystickHandle;
    

    private UnityAction backEvent;
    #endregion


    #region 프로퍼티 목록
    #endregion


    #region 함수 목록
    private void Start()
    {
        



        PadEventInit();
    }


    /// <summary>
    /// Start에서 호출됨.<br/>
    /// 패드를 입력할 시 호출되는 이벤트 등을 등록.
    /// </summary>
    private void PadEventInit()
    {
        //조이스틱 패드 쪽
        moveBtn.Drag += (Vector2 v2) =>
        {
            //GameManager.Instance.playerController?.OrderAction(new Order() { orderTitle = OrderTitle.Move, parameter = new ObjectController.OrderParameters_Move { inputXY = v2 } });
            GameManager.Instance.playerController?.OrderMove(v2);
        };

        moveBtn.PointerUp += (BaseEventData e) =>
        {
            GameManager.Instance.playerController?.OrderIdle(false);
        };

        attackBtn.Drag += (Vector2 v2) =>
        {
            if(v2.sqrMagnitude >= 0.98f)
            {
                GameManager.Instance.playerController?.OrderAttack(v2);
            }
        };
    }
    #endregion
}
