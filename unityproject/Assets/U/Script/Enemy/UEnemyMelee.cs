using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;
using Order = IController.Order;
using OrderTitle = IController.OrderTitle;

public class UEnemyMelee : UCharacter, IController
{
    //������������������������������������������������������������������������������������������������//
    //                                         ����                       
    
    // '��' ��ֹ��� ��ǥ(x,y)�� ���� ������ �����´�.
    public TileWallScan TileWall;

    private float avoidTimer = 0.0f;
    private bool avoidingStop = false;
    private float avoidingSpeed = 3.0f;
    private bool isStateaChange = true;
    private bool isActionAllStop = false;
    //_______________________________________________________________________________________//

    private static Dictionary<OrderTitle, UnityAction<UEnemyMelee, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UEnemyMelee, List<object>>>()
    {
        //Idle�� valList �� State 
        { OrderTitle.Idle, (o, valList) => {o.GetSpumPrefabs.PlayAnimation(0);  } },

        //Move�� valList �� State/InputX/InputY 
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
                    // �ٸ� ���·� �����ϴ� �� ���� ��.
                    o.isStateaChange  = false;
            
                    // �� ��ġ - ��� ��ġ = ��밡 �� ���� ����
                    Vector3 dir = o.transform.position - o.GetotherColliderVector;

                    if (dir.x >= 0.0f) nextVector.x = 1;
                    else nextVector.x = -1;

                    if (dir.y > 0.0f) nextVector.y = 1;
                    else nextVector.y = -1;

          
                    // �̵�
                    o.myRigidbody.AddForce(nextVector * o.avoidingSpeed, ForceMode2D.Impulse);

                }

                // 1�� �̻��� ��� �ٸ� ���º����� ���
                if (o.avoidTimer >= 1.5f)
                {
                    o.isStateaChange = true;
                }

                // Ÿ�̸Ӱ� ?�� �̻��� ���
                if(o.avoidTimer >= 3.0f)
                {
                    o.avoidingStop = false;
                    o.avoidTimer = 0.0f;
                }

        
                // �ִϸ��̼�
                o.GetSpumPrefabs.PlayAnimation(3);
            }
        },

         // ���� ���¿� ���� ó��
        { OrderTitle.Dead, (o, valList) =>

            {
                o.isActionAllStop = true;
                o.GetSpumPrefabs.PlayAnimation(2);

                //3�� �ڿ� Destory
                Destroy(o.gameObject, 2f);
            }
        }
        //�� ������ ����


        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

        //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

        //���� ���� �� valList ����_State/�����ð�

        //��Ÿ���
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
            // �ٸ� ���¿��� actionStop = true �����̴�.
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
            // �ٸ� ���¿��� actionStop = true �����̴�.
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

        // State��ũ��Ʈ�� '�ڽ�'������ �ֱ� ���� ����
        boxCollider2D = GetComponent<BoxCollider2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        myRigidbody = GetComponent<Rigidbody2D>();

        //ChangeState(new IdleState(this));


    }

    // �浹 ���� �ȿ� ���� ����
    public void OnTriggerStay2D(Collider2D _other)
    {

        // ���� �ȿ� ���� ������Ʈ�� �±װ� Player�ΰ�?
        if (_other.gameObject.tag == "Player")
        {
            // otherColliderVector = ���� ����� position ��
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
    // �浹 �������� ���� ����
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("�浹 ���� ����");
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
