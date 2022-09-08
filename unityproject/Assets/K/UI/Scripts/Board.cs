using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using NaughtyAttributes;


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

    [Header("인벤토리 패널")]
    [SerializeField] private InventoryUI inventoryPanel;
    

    private Stack<UnityAction> backEvents;

    [ReadOnly] protected Aim_Base aim;
    #endregion 


    #region 프로퍼티 목록
    #endregion


    #region 함수 목록
    private void Start()
    {
        backEvents = new Stack<UnityAction>();
        backBtn.onClick.AddListener(BackBtnClick);
        menuOpenBtn.onClick.AddListener(MenuOpenBtnClick);
        inventoryBtn.onClick.AddListener(InventoryBtnClick);


        PadEventInit();
        inputBoard.SetActive(true);
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
            var handler = GameManager.Instance.playerController;

            if (handler == null) return;

            if (v2.sqrMagnitude >= 0.98f)
            {
                handler.OrderAttack(v2);
            }


            var equipment = handler.GetDecorator(Enums.Decorator.Equipment) as EquipmentDecorator;
            if (equipment != null && equipment.WeaponItem != null)
            {
                var rotateTr = equipment.WeaponPivot.parent;

                var v2Nor = v2.normalized;
                rotateTr.rotation = Quaternion.AngleAxis(Mathf.Atan2(v2Nor.y, v2Nor.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
            }
        };


        attackBtn.Drag += (Vector2 v2) =>
        {
            var handler = GameManager.Instance.playerController;

            if (handler == null) return;

            if (aim == null)
            {
                aim = GameManager.Instance.EffectManager.Pop(Enums.Effect.Aim_Base) as Aim_Base;

                if (aim == null) return;

                aim.gameObject.SetActive(true);

                var equip = handler.GetDecorator(Enums.Decorator.Equipment) as EquipmentDecorator;

                if (equip == null)
                {
                    aim.gameObject.transform.parent = handler.gameObject.transform;
                }
                else
                {
                    aim.gameObject.transform.parent = equip.WeaponPivot.parent;
                }
                aim.transform.localPosition = Vector3.zero;
            }
            else
                aim.OnDrag(v2);
        };

        attackBtn.PointerUp += (BaseEventData e) =>
        {
            if(aim != null)
            {
                aim.Push();
                aim = null;
            }
        };
    }

    private void MenuOpenBtnClick()
    {
        inputBoard.SetActive(false);
        menuOpenBtn.gameObject.SetActive(false);
        menuBoard.SetActive(true);
        backBtn.gameObject.SetActive(true);
        backEvents.Push(MenuOpenBtnBack);
    }

    private void MenuOpenBtnBack()
    {
        inputBoard.SetActive(true);
        backBtn.gameObject.SetActive(false);
        menuOpenBtn.gameObject.SetActive(true);
        menuBoard.SetActive(false);
    }

    private void InventoryBtnClick()
    {
        menuBoard.SetActive(false);
        inventoryPanel.Open();
        backEvents.Push(InventoryBtnBack);
    }

    private void InventoryBtnBack()
    {
        menuBoard.SetActive(true);
        inventoryPanel.Close();
    }

    private void BackBtnClick()
    {
        if(backEvents.Count > 0)
        {
            backEvents.Pop()();
        }
    }
    #endregion
}
