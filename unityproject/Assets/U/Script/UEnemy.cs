//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////UnityAction이 정의됨
//using UnityEngine.Events;
//using Order = IController.Order;
//using OrderTitle = Enums.OrderTitle;

//public class UEnemy : UCharacter, IController
//{
//    // '벽' 장애물에 좌표(x,y)에 대한 정보를 가져온다.
//    public TileWallScan TileWall;


//    private static Dictionary<OrderTitle, UnityAction<UEnemy, object>> actionDic = new Dictionary<OrderTitle, UnityAction<UEnemy, object>>()
//    {
//        //Idle의 valList 값 State 
//        { OrderTitle.Idle, (o, valList) => {o.GetSpumPrefabs.PlayAnimation(0);  } },

//        //Move의 valList 값 State/InputX/InputY 
//        { OrderTitle.Move, (o, valList) => {o.ChangeState(new MoveState(o)); } },

//        { OrderTitle.Follow, (o, valList) => 
//            {
//                o.transform.position = Vector3.MoveTowards(o.transform.position, o.GetotherColliderVector, 1.0f * Time.deltaTime);
//                o.GetSpumPrefabs.PlayAnimation(1);
//            }

//        },

//        { OrderTitle.Attack, (o, valList) => {o.ChangeState(new AttackState(o)); } },

//        { OrderTitle.Avoiding, (o, valList) => {o.ChangeState(new AvoidingState(o)); } }

//        //이 앞으론 예시

//        //
//        //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과

//        //피격을 받았을 때 valList 값 피격_State/데미지/경직시간/부가_디버프

//        //기절 했을 때 valList 기절_State/기절시간

//        //기타등등
//    };

//    public Enums.ControllerKind GetKind() => Enums.ControllerKind.Creature;
//    List<string> ReturnTheStateList(string _state)
//    {
//        List<string> action = new List<string>();

//        string actionStr = _state;
//        action.Add(actionStr);

//        return action;
//    }
//    public Transform GetTransform() => transform;
//    public void OrderAction(params Order[] orders)
//    {

//        foreach (var order in orders)
//        {
//            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemy, object> actionFunc))
//            {
//                actionFunc(this, order.parameter);
//            }
//        }
//    }

//    public void OrderAction(List<Order> orders)
//    {

//        foreach (var order in orders)
//        {
//            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemy, object> actionFunc))
//            {
//                actionFunc(this, order.parameter);
//            }
//        }
//    }
//    public GameObject GetGameObject() => gameObject;

//    protected override void Start()
//    {
//        base.Start();

//        boxCollider2D   = GetComponent<BoxCollider2D>();
//        spumPrefabs     = GetComponent<SPUM_Prefabs>();
//        myRigidbody     = GetComponent<Rigidbody2D>();

//        ChangeState(new IdleState(this));
        
        
//    }

//    // 충돌 범위 안에 들어온 상태
//    public void OnTriggerStay2D(Collider2D _other)
//    {
        
//        // 범위 안에 들어온 오브젝트에 태그가 Player인가?
//        if (_other.gameObject.tag == "Player")
//        {
//            // otherColliderVector = 들어온 상대의 position 값
//            otherColliderVector = _other.transform.position;
//            float dis           = Vector3.Distance(otherColliderVector, transform.position);

//            // 대상과의 거리
//            if (dis < 1.0f)
//            {
//                //OrderAction(ReturnTheStateList("Avoiding"));
//                OrderAction(new Order() { orderTitle = OrderTitle.Avoiding });
//            }
//            else
//            {
//                //OrderAction(ReturnTheStateList("Idle"));
//                OrderAction(new Order() { orderTitle = OrderTitle.Idle });
//            }
//        }
//    }
//    // 충돌 범위에서 나간 상태
//    public void OnTriggerExit2D(Collider2D other)
//    {
//        //OrderAction(ReturnTheStateList("Idle"));
//        OrderAction(new Order() { orderTitle = OrderTitle.Idle });
//    }

//    float timer = 0;
//    int waitingTime = 1;

//    protected override void Update()
//    {
//        base.Update();

//        timer += Time.deltaTime;
//        if(timer > waitingTime)
//        {
//            timer = 0;
//        }
//    }
//}
