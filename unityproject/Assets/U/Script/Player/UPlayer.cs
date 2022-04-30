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

    //private bool isActionAllStop;
    //private bool isArrowDelay;

    WeaponItem weaponItem;
    IWeaponPrefab weaponObject;
    private IEnumerator dashCor;


    private static Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>> actionDic = new Dictionary<OrderTitle, UnityAction<UPlayer, List<object>>>()
    {
        //Idle�� valList �� State 
        { OrderTitle.Idle, (o, valList) =>       
        {
            o.moveVector = Vector2.zero;
            if(o.animState == OrderTitle.Dash || o.animState == OrderTitle.Attack) return;
            o.animState = OrderTitle.Idle;
            o.myRigidbody.velocity = Vector2.zero;
            o.GetSpumPrefabs.PlayAnimation(0); 
        } },

        //Move�� valList �� State/InputX/InputY 
        { OrderTitle.Move, (o, valList) =>
            {
             /* o.ChangeState(new MoveState(me));*/
             // ĳ���� �̵�

                if(o.animState == OrderTitle.Dash) return;


                if(o.animState == OrderTitle.Attack)
                {
                    o.myRigidbody.velocity = Vector2.zero;
                    o.moveVector = (Vector2)valList[0];
                    return;
                }

                if((Vector2)valList[0] == Vector2.zero)
                {
                    actionDic[OrderTitle.Idle](o, valList);
                    return;
                }
                else
                {
                    o.moveVector = (Vector2)valList[0];
                }
                o.myRigidbody.velocity = o.moveVector * o.GetSpeed;
                // �ִϸ��̼� ���
                o.GetSpumPrefabs.PlayAnimation(1);
                if (o.moveVector.x > 0f) o.spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
                else if (o.moveVector.x < 0f) o.spumPrefabs.transform.localScale = new Vector3(1, 1, 1);
            }
        },

        { OrderTitle.Attack,(o, valList) =>
            {
                //if(!o.isArrowDelay)
                //{
                //    o.isArrowDelay =true;
                //    o.StartCoroutine(o.ArrowAttackDelay());
                    
                //    Vector3 attackVector = new Vector3((float)valList[0],(float)valList[1], 0f);
                //    attackVector.x = attackVector.x + o.transform.position.x;
                //    attackVector.y = attackVector.y + o.transform.position.y;

                //    Quaternion attackQuaternion = Quaternion.Euler(0,0,o.gunAim.transform.rotation.z*100);
                
                //    // ȸ��.w ���� -�� ��� 
                //    if(o.gunAim.transform.rotation.w <0)
                //        attackQuaternion.w = attackQuaternion.w * -1;

                //    //����(������Ʈ, ����, ȸ��)
                //    Instantiate(o.weaPon, attackVector, attackQuaternion);

                //    o.moveJoystickVector.Set(0f,0f);
                //    o.StartCoroutine(o.ArrowAttackDash());
                   
                //}                
                if(o.weaponObject != null && o.dashCor == null)
                {
                    if (o.weaponObject.AttackAnim(0.8f, () => {
                        o.dashCor = o.DashDuration();
                        o.StartCoroutine(o.dashCor);
                    }))
                    {
                        o.animState = OrderTitle.Attack;
                        o.GetSpumPrefabs.PlayAnimation(0);
                    }
                }
            }
        },

        {
            OrderTitle.AttackStop, (o, valList) =>
            {
                if(o.weaponObject != null)
                {
                    o.weaponObject.StopAnim();
                    o.animState = OrderTitle.Idle;
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
        GameManager.Instance.playerController = this;
        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();

        attackJoystick.Drag += (Vector2 v2) =>
        {
            if (v2.sqrMagnitude >= 0.98)
            {
                OrderAction(new Order() { orderTitle = OrderTitle.Attack, parameters = new List<object>() { v2.x, v2.y } });
            }
            else
            {
                OrderAction(new Order() { orderTitle = OrderTitle.AttackStop, parameters = null });
            }
        };
        attackJoystick.PointerUp += (e) =>
        {
            OrderAction(new Order() { orderTitle = OrderTitle.AttackStop, parameters = null });
        };

        moveJoystick.Drag += (Vector2 v2) =>
        {
            moveJoystickVector = v2;
        };

        var weaponItem = GameManager.Instance.GameDB.WeaponManager.CreateWeaponItem(GameManager.Instance.GameDB.WeaponManager.GetWeaponData("WoodBow"));
        var weaponObject = Instantiate(GameManager.Instance.GameDB.WeaponManager.ToPrefab(weaponItem.Weapon) as BowModel, gunAim.transform);
        weaponObject.transform.localScale = new Vector3(0.37f, 0.52f, 1f);
        weaponObject.transform.localPosition = new Vector3(0f, 0.919f * 0.5f, -1f);
        this.weaponItem = weaponItem;
        this.weaponObject = weaponObject;

        //moveJoystick.Drag += (Vector2 v2) =>
        // {
        //     if (v2.sqrMagnitude >= 0.98)
        //     {
        //         Debug.Log("���� ��");
        //     }
        //     else
        //     {
        //         Debug.Log("��ġ");
        //         isActionAllStop = true;
        //     }
        // };
    }

    protected override void Update()
    {
        if (gunAim != null)
        {
            gunAim.transform.position = new Vector3(transform.position.x, transform.position.y, gunAim.transform.position.z);
        }
    }

    //IEnumerator ArrowAttackDelay()
    //{
    //    yield return new WaitForSeconds(2);
    //    isArrowDelay = false;
    //}


    IEnumerator DashDuration()
    {
        //float delay = 0.0f;

        //while (delay < 1.0f)
        //{
        //    if (moveJoystickVector.x != 0 || moveJoystickVector.y != 0)
        //    {
        //        Debug.Log("����");
        //        //myRigidbody.AddForce(moveJoystickVector * 2.0f, ForceMode2D.Impulse);
        //        myRigidbody.velocity = moveJoystickVector * 3;
        //    }
        //    yield return new WaitForSeconds(0.1f);
        //    delay += 0.1f;

        //}
        GetSpumPrefabs.PlayAnimation(0);
        animState = OrderTitle.Dash;
        float duration = 0.12f;
        while(duration > 0f)
        {
            duration -= Time.deltaTime;
            myRigidbody.velocity = moveVector * 12f;
            yield return null;
        }
        myRigidbody.velocity = Vector2.zero;
        moveVector = Vector2.zero;
        animState = OrderTitle.Idle;
        dashCor = null;
        //actionDic[OrderTitle.Idle](this, null);
    }
}
