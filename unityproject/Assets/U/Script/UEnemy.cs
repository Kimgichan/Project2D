//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////UnityAction�� ���ǵ�
//using UnityEngine.Events;
//using Order = IController.Order;
//using OrderTitle = Enums.OrderTitle;

//public class UEnemy : UCharacter, IController
//{
//    // '��' ��ֹ��� ��ǥ(x,y)�� ���� ������ �����´�.
//    public TileWallScan TileWall;


//    private static Dictionary<OrderTitle, UnityAction<UEnemy, object>> actionDic = new Dictionary<OrderTitle, UnityAction<UEnemy, object>>()
//    {
//        //Idle�� valList �� State 
//        { OrderTitle.Idle, (o, valList) => {o.GetSpumPrefabs.PlayAnimation(0);  } },

//        //Move�� valList �� State/InputX/InputY 
//        { OrderTitle.Move, (o, valList) => {o.ChangeState(new MoveState(o)); } },

//        { OrderTitle.Follow, (o, valList) => 
//            {
//                o.transform.position = Vector3.MoveTowards(o.transform.position, o.GetotherColliderVector, 1.0f * Time.deltaTime);
//                o.GetSpumPrefabs.PlayAnimation(1);
//            }

//        },

//        { OrderTitle.Attack, (o, valList) => {o.ChangeState(new AttackState(o)); } },

//        { OrderTitle.Avoiding, (o, valList) => {o.ChangeState(new AvoidingState(o)); } }

//        //�� ������ ����

//        //
//        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

//        //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

//        //���� ���� �� valList ����_State/�����ð�

//        //��Ÿ���
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

//    // �浹 ���� �ȿ� ���� ����
//    public void OnTriggerStay2D(Collider2D _other)
//    {
        
//        // ���� �ȿ� ���� ������Ʈ�� �±װ� Player�ΰ�?
//        if (_other.gameObject.tag == "Player")
//        {
//            // otherColliderVector = ���� ����� position ��
//            otherColliderVector = _other.transform.position;
//            float dis           = Vector3.Distance(otherColliderVector, transform.position);

//            // ������ �Ÿ�
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
//    // �浹 �������� ���� ����
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
