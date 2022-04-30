using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction이 정의됨
using UnityEngine.Events;
using Order = IController.Order;
using OrderTitle = IController.OrderTitle;

public class UEnemyMelee : UCharacter, IController
{
    //￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣//
    //                                         변수                       
    
    // '벽' 장애물에 좌표(x,y)에 대한 정보를 가져온다.
    public TileWallScan TileWall;

    private float avoidTimer = 0.0f;
    private bool avoidingStop = false;
    private float avoidingSpeed = 3.0f;
    private bool isStateaChange = true;
    private bool isActionAllStop = false;
    //_______________________________________________________________________________________//

    private static Dictionary<OrderTitle, UnityAction<UEnemyMelee, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UEnemyMelee, List<object>>>()
    {
        //Idle의 valList 값 State 
        { OrderTitle.Idle, (o, valList) => {o.GetSpumPrefabs.PlayAnimation(0);  } },

        //Move의 valList 값 State/InputX/InputY 
        { OrderTitle.Move, (o, valList) => {o.ChangeState(new MoveState(o)); } },

        { OrderTitle.Follow, (o, valList) => 
            {
                o.transform.position = Vector3.MoveTowards(o.transform.position, o.GetotherColliderVector, 1.0f * Time.deltaTime);
                o.GetSpumPrefabs.PlayAnimation(1);
            }
        },

        { OrderTitle.Attack, (o, valList) => {o.GetSpumPrefabs.PlayAnimation(4); } },

        { OrderTitle.Avoiding, (o, valList) => 
            {
                o.avoidTimer += Time.deltaTime;

                if (!o.avoidingStop)
                {
                    Vector3     nextVector = new Vector3();
                    o.avoidingStop    = true;
                    // 다른 상태로 변경하는 걸 막는 다.
                    o.isStateaChange  = false;
            
                    // 내 위치 - 상대 위치 = 상대가 날 보는 방향
                    Vector3 dir = o.transform.position - o.GetotherColliderVector;

                    if (dir.x >= 0.0f) nextVector.x = 1;
                    else nextVector.x = -1;

                    if (dir.y > 0.0f) nextVector.y = 1;
                    else nextVector.y = -1;

          
                    // 이동
                    o.myRigidbody.AddForce(nextVector * o.avoidingSpeed, ForceMode2D.Impulse);

                }

                // 1초 이상일 경우 다른 상태변경을 허용
                if (o.avoidTimer >= 1.5f)
                {
                    o.isStateaChange = true;
                }

                // 타이머가 ?초 이상일 경우
                if(o.avoidTimer >= 3.0f)
                {
                    o.avoidingStop = false;
                    o.avoidTimer = 0.0f;
                }

        
                // 애니메이션
                o.GetSpumPrefabs.PlayAnimation(3);
            }
        },

         // 죽은 상태에 대한 처리
        { OrderTitle.Dead, (o, valList) =>

            {
                o.isActionAllStop = true;
                o.GetSpumPrefabs.PlayAnimation(2);

                //3초 뒤에 Destory
                Destroy(o.gameObject, 2f);
            }
        }
        //이 앞으론 예시


        //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과

        //피격을 받았을 때 valList 값 피격_State/데미지/경직시간/부가_디버프

        //기절 했을 때 valList 기절_State/기절시간

        //기타등등
    };

    List<string> ReturnTheStateList(string _state)
    {
        List<string> action = new List<string>();

        string actionStr = _state;
        action.Add(actionStr);

        return action;
    }

    public void OrderAction(params Order[] orders)
    {
        if (isActionAllStop)
        {
            // 다른 상태에서 actionStop = true 상태이다.
        }
        else
        {
            foreach (var order in orders)
            {
                if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemyMelee, List<object>> actionFunc))
                {
                    actionFunc(this, order.parameters);
                }
            }
        }
    }

    public void OrderAction(List<Order> orders)
    {
        if (isActionAllStop)
        {
            // 다른 상태에서 actionStop = true 상태이다.
        }
        else
        {
            foreach (var order in orders)
            {
                if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemyMelee, List<object>> actionFunc))
                {
                    actionFunc(this, order.parameters);
                }
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        // State스크립트에 '자신'정보를 주기 위한 변수
        boxCollider2D = GetComponent<BoxCollider2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        myRigidbody = GetComponent<Rigidbody2D>();

        //ChangeState(new IdleState(this));


    }

    // 충돌 범위 안에 들어온 상태
    public void OnTriggerStay2D(Collider2D _other)
    {

        // 범위 안에 들어온 오브젝트에 태그가 Player인가?
        if (_other.gameObject.tag == "Player")
        {
            // otherColliderVector = 들어온 상대의 position 값
            otherColliderVector = _other.transform.position;
            float dis = Vector3.Distance(otherColliderVector, transform.position);

            if (dis > 1.0f)
            {

                //OrderAction(ReturnTheStateList("Follow"));
                OrderAction(new Order() { orderTitle = OrderTitle.Follow });
            }
            else if (dis < 0.8f)
            {

                //OrderAction(ReturnTheStateList("Attack"));
                OrderAction(new Order() { orderTitle = OrderTitle.Attack });

            }
        }
    }
    // 충돌 범위에서 나간 상태
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("충돌 범위 나감");
            //OrderAction(ReturnTheStateList("Idle"));
            OrderAction(new Order() { orderTitle = OrderTitle.Idle });
        }
    }

    float timer = 0;
    int waitingTime = 1;

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            timer = 0;
        }

        if (hp < 0.0f)
        {
            OrderAction(new Order() { orderTitle = OrderTitle.Dead });
        }
    }


}
