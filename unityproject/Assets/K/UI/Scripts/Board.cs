using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;



public class Board : MonoBehaviour
{
    #region ���� ���
    [SerializeField] private Button menuOpenBtn;
    [SerializeField] private GraphicRaycaster ray;

    [Header("�޴� ����")]
    [SerializeField] private GameObject menuBoard;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button backBtn;


    [Header("���̽�ƽ �е�")]
    [SerializeField] private GameObject inputBoard;
    [SerializeField] private VirtualJoystick moveBtn;
    [SerializeField] private List<EventTrigger> portionSlotList;
    [SerializeField] private VirtualJoystick attackBtn;
    [SerializeField] private Image rightJoystick;
    [SerializeField] private Image rightJoystickHandle;
    

    private UnityAction backEvent;
    #endregion


    #region ������Ƽ ���
    #endregion


    #region �Լ� ���
    private void Start()
    {
        



        PadEventInit();
    }


    /// <summary>
    /// Start���� ȣ���.<br/>
    /// �е带 �Է��� �� ȣ��Ǵ� �̺�Ʈ ���� ���.
    /// </summary>
    private void PadEventInit()
    {
        //���̽�ƽ �е� ��
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
