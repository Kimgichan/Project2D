using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;
using Order = IController.Order;
using OrderTitle = IController.OrderTitle;

public class UPlayer : UCharacter, IController
{
    private OrderTitle animState;
    private HashSet<OrderTitle> buffStates;

    public GameObject gunAim;
    public GameObject weaPon;
    public VirtualJoystick attackJoystick;
    public VirtualJoystick moveJoystick;
    private Vector2 moveJoystickVector;

    private bool isActionStop;
    private bool isArrowDelay;

    public void messgeAttack(Vector2 _vector)
    {
        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��
        OrderAction(new Order() { orderTitle = OrderTitle.Attack, parameters = new List<object>() { _vector.x, _vector.y} });
    }

    void gunAimPlayerFollow()
    {
        gunAim.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private static Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>>()
    {
        //Idle�� valList �� State 
        { OrderTitle.Idle, (o, valList) =>       {o.GetSpumPrefabs.PlayAnimation(0); } },

        //Move�� valList �� State/InputX/InputY 
        { OrderTitle.Move, (o, valList) =>
            {
             /* o.ChangeState(new MoveState(me));*/
             // ĳ���� �̵�

                if((Vector2)valList[0] == Vector2.zero)
                {
                    actionDic[OrderTitle.Idle](o, valList);
                    return;
                }

                o.moveVector = (Vector2)valList[0];
                o.transform.Translate(o.moveVector * o.GetSpeed * Time.deltaTime);
                // �ִϸ��̼� ���
                o.GetSpumPrefabs.PlayAnimation(1);
                if (o.moveVector.x > 0f) o.spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
                else if (o.moveVector.x < 0f) o.spumPrefabs.transform.localScale = new Vector3(1, 1, 1);
            }
        },

        { OrderTitle.Attack,(o, valList) =>
            {
                if(!o.isArrowDelay)
                {
                    o.isArrowDelay =true;
                    o.StartCoroutine(o.ArrowAttackDelay());
                    
                    Vector3 attackVector = new Vector3((float)valList[0],(float)valList[1], 0f);
                    attackVector.x = attackVector.x + o.transform.position.x;
                    attackVector.y = attackVector.y + o.transform.position.y;

                    Quaternion attackQuaternion = Quaternion.Euler(0,0,o.gunAim.transform.rotation.z*100);
                
                    // ȸ��.w ���� -�� ��� 
                    if(o.gunAim.transform.rotation.w <0)
                        attackQuaternion.w = attackQuaternion.w * -1;

                    //����(������Ʈ, ����, ȸ��)
                    Instantiate(o.weaPon, attackVector, attackQuaternion);

                    o.moveJoystickVector.Set(0f,0f);
                    o.StartCoroutine(o.ArrowAttackDash());
                   
                }                
            }
        }

    //�� ������ ����


    //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

    //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

    //���� ���� �� valList ����_State/�����ð�

    //��Ÿ���
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
        if (isActionStop)
        {
            // �ٸ� ���¿��� actionStop = true �����̴�.
        }
        else
        {
            foreach (var order in orders)
            {
                if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
                {
                    actionFunc(this, order.parameters);
                }
            }
        }
    }

    public void OrderAction(List<Order> orders)
    {
        if (isActionStop)
        {
            // �ٸ� ���¿��� actionStop = true �����̴�.
        }
        else
        {
            foreach (var order in orders)
            {
                if (actionDic.TryGetValue(order.orderTitle, out UnityAction<UPlayer, List<object>> actionFunc))
                {
                    actionFunc(this, order.parameters);
                }
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.playerController = this;
        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();

        isActionStop = false;

        attackJoystick.Drag += (Vector2 v2) =>
        {
            if (v2.sqrMagnitude >= 0.98)
            {
                isActionStop = false;
                OrderAction(new Order() { orderTitle = OrderTitle.Attack, parameters = new List<object>() { v2.x, v2.y } });
            }
            else
            {
                isActionStop = true;
            }
        };

        moveJoystick.Drag += (Vector2 v2) =>
        {
            moveJoystickVector = v2;
        };

        //moveJoystick.Drag += (Vector2 v2) =>
        // {
        //     if (v2.sqrMagnitude >= 0.98)
        //     {
        //         Debug.Log("���� ��");
        //     }
        //     else
        //     {
        //         Debug.Log("��ġ");
        //         isActionStop = true;
        //     }
        // };
    }

    protected override void Update()
    {
        if(gunAim != null)
        {
            gunAim.transform.position = transform.position;
        }
    }

    IEnumerator ArrowAttackDelay()
    {
        yield return new WaitForSeconds(2);
        isArrowDelay = false;
    }


    IEnumerator ArrowAttackDash()
    {
        float delay = 0.0f;

        while (delay <2.0f)
        {
            if (moveJoystickVector.x != 0 || moveJoystickVector.y != 0)
            {
                myRigidbody.AddForce(moveJoystickVector * 2.0f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.1f);
            delay += 0.1f;
     
        }
    }
}
