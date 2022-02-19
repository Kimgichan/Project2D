using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;

public class UEnemy : UCharacter, IController
{
    static UEnemy me;
    // '��' ��ֹ��� ��ǥ(x,y)�� ���� ������ �����´�.
    public TileWallScan TileWall;


    private static Dictionary<string, UnityAction<UEnemy, List<object>>> actionDic = new Dictionary<string, UnityAction<UEnemy, List<object>>>()
    {
        //Idle�� valList �� State 
        { "Idle", (o, valList) => {o.ChangeState(new IdleState(me));  } },

        //Move�� valList �� State/InputX/InputY 
        { "Move", (o, valList) => {o.ChangeState(new MoveState(me)); } },

        { "Follow", (o, valList) => {o.ChangeState(new FollowState(me)); } },

        { "Attack", (o, valList) => {o.ChangeState(new AttackState(me)); } },

        { "Avoiding", (o, valList) => {o.ChangeState(new AvoidingState(me)); } }

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

        // State��ũ��Ʈ�� '�ڽ�'������ �ֱ� ���� ����
        me              = this;

        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();
        myRigidbody     = GetComponent<Rigidbody2D>();

        ChangeState(new IdleState(this));
        
        
    }

    // �浹 ���� �ȿ� ���� ����
    public void OnTriggerStay2D(Collider2D _other)
    {
        
        // ���� �ȿ� ���� ������Ʈ�� �±װ� Player�ΰ�?
        if (_other.gameObject.tag == "Player")
        {
            // otherColliderVector = ���� ����� position ��
            otherColliderVector = _other.transform.position;
            float dis           = Vector3.Distance(otherColliderVector, transform.position);

            // ������ �Ÿ�
            if (dis < 1.0f) OrderAction(ReturnTheStateList("Avoiding"));
            else            OrderAction(ReturnTheStateList("Idle"));
        }
    }
    // �浹 �������� ���� ����
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
