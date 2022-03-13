using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction이 정의됨
using UnityEngine.Events;
using Order = IController.Order;
using OrderTitle = IController.OrderTitle;

public class UPlayer : UCharacter, IController
{
    private OrderTitle animState;
    private HashSet<OrderTitle> buffStates;

    public GameObject gunAim;

    void gunAimPlayerFollow()
    {
        gunAim.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private static Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>>()
    {
        //Idle의 valList 값 State 
        { OrderTitle.Idle, (o, valList) =>       {o.GetSpumPrefabs.PlayAnimation(0); } },

        //Move의 valList 값 State/InputX/InputY 
        { OrderTitle.Move, (o, valList) =>
            {
             /* o.ChangeState(new MoveState(me));*/
             // 캐릭터 이동
             o.transform.Translate(o.GetMoveVector * o.GetSpeed * Time.deltaTime);
             // 애니메이션 재생
             o.GetSpumPrefabs.PlayAnimation(1);
            }
        },

        { OrderTitle.Attack,(o, valList) =>
            {

            }
        }

    //이 앞으론 예시


    //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과

    //피격을 받았을 때 valList 값 피격_State/데미지/경직시간/부가_디버프

    //기절 했을 때 valList 기절_State/기절시간

    //기타등등
    };
    public bool DoMove()
    {
        if (moveVector.x == 0f && moveVector.y == 0f) return false;
        else return true;
    }

    List<string> ReturnTheStateList(string _state)
    {
        List<string> action = new List<string>();

        string actionStr = _state;
        action.Add(actionStr);

        return action;
    }

    public void OrderAction(params Order[] orders)
    {

        foreach (var order in orders)
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    public void OrderAction(List<Order> orders)
    {

        foreach (var order in orders)
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        boxCollider2D = GetComponent<BoxCollider2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
    }

    protected override void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if (DoMove())
        {
            // 방향에 따라 캐릭터 모델 보는 방향 변경
            if (moveVector.x > 0f) spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if (moveVector.x < 0f) spumPrefabs.transform.localScale = new Vector3(1, 1, 1);

            OrderAction(new Order() { orderTitle = OrderTitle.Move });
            //OrderAction(new Order() { orderTitle = "move", parameters = new List<object>() { 1.0f, 1.0f } });
        }
        else
        {
            OrderAction(new Order() { orderTitle = OrderTitle.Idle });
        }

        //GunAim가 캐릭터 위치에서 나오게 업데이트.
        gunAimPlayerFollow();

    }
}
