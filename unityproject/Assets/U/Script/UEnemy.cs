using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction이 정의됨
using UnityEngine.Events;

public class UEnemy : UCharacter, IController
{
    static UEnemy me;
    // '벽' 장애물에 좌표(x,y)에 대한 정보를 가져온다.
    public TileWallScan TileWall;


    private static Dictionary<string, UnityAction<UEnemy, List<object>>> actionDic = new Dictionary<string, UnityAction<UEnemy, List<object>>>()
    {
        //Idle의 valList 값 State 
        { "Idle", (o, valList) => {o.ChangeState(new IdleState(me));  } },

        //Move의 valList 값 State/InputX/InputY 
        { "Move", (o, valList) => {o.ChangeState(new MoveState(me)); } },

        { "Follow", (o, valList) => {o.ChangeState(new FollowState(me)); } },

        { "Attack", (o, valList) => {o.ChangeState(new AttackState(me)); } },

        { "Avoiding", (o, valList) => {o.ChangeState(new AvoidingState(me)); } }

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

    public void OrderAction(params object[] orders)
    {
        foreach (var order in orders as IController.Order[])
        {
            if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UEnemy, List<object>> actionFunc))
            {
                actionFunc(this, order.parameters);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        // State스크립트에 '자신'정보를 주기 위한 변수
        me              = this;

        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();
        myRigidbody     = GetComponent<Rigidbody2D>();

        ChangeState(new IdleState(this));
        
        
    }

    // 충돌 범위 안에 들어온 상태
    public void OnTriggerStay2D(Collider2D _other)
    {
        
        // 범위 안에 들어온 오브젝트에 태그가 Player인가?
        if (_other.gameObject.tag == "Player")
        {
            // otherColliderVector = 들어온 상대의 position 값
            otherColliderVector = _other.transform.position;
            float dis           = Vector3.Distance(otherColliderVector, transform.position);

            // 대상과의 거리
            if (dis < 1.0f) OrderAction(ReturnTheStateList("Avoiding"));
            else            OrderAction(ReturnTheStateList("Idle"));
        }
    }
    // 충돌 범위에서 나간 상태
    public void OnTriggerExit2D(Collider2D other)
    {
        OrderAction(ReturnTheStateList("Idle"));
    }

    float timer = 0;
    int waitingTime = 1;

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if(timer > waitingTime)
        {
            timer = 0;
        }
    }
}
