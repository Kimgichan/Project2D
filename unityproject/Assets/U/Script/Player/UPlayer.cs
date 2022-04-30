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

    public GameObject           gunAim;
    public UWeaponManager       weaponManager;
    private BowModel            weapon;
    //public IWeaponPrefab      TestWeapon;
    public VirtualJoystick      attackJoystick;
    public VirtualJoystick      moveJoystick;
    private Vector2             moveJoystickVector;
    private Vector2             JoystickVector;
    
    private bool isActionAllStop;
    private bool isArrowDelay;

    public void messgeAttack(Vector2 _vector)
    {
        //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과
        OrderAction(new Order() { orderTitle = OrderTitle.Attack, parameters = new List<object>() { _vector.x, _vector.y} });
    }

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
                // 캐릭터 이동
                if((Vector2)valList[0] == Vector2.zero)
                {
                    actionDic[OrderTitle.Idle](o, valList);
                    return;
                }

                o.moveVector = (Vector2)valList[0];
                o.transform.Translate(o.moveVector * o.GetSpeed * Time.deltaTime);
                // 애니메이션 재생
                o.GetSpumPrefabs.PlayAnimation(1);
                if (o.moveVector.x > 0f) o.spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
                else if (o.moveVector.x < 0f) o.spumPrefabs.transform.localScale = new Vector3(1, 1, 1);
            }
        },

        { OrderTitle.Attack,(o, valList) =>
            {
                // 다음 공격에 걸리는 딜레이 상태확인
                if(!o.isArrowDelay)
                {
                    // 공격 애니메이션
                    o.weapon.AttackAnim(0.3f, null);

                    //공격 딜레이 = true와 동시에 코루틴을 통해 몇 초뒤(ArrowAttackDelay에서 딜레이 시간) false변경
                    o.isArrowDelay =true;
                    o.StartCoroutine(o.ArrowAttackDelay());
                    
                    Vector3 attackVector = new Vector3((float)valList[0],(float)valList[1], 0f);
                    attackVector.x = attackVector.x + o.transform.position.x;
                    attackVector.y = attackVector.y + o.transform.position.y;

                    Quaternion attackQuaternion = Quaternion.Euler(0,0,o.gunAim.transform.rotation.z * 100f);
                
                    // 회전.w 값이 -인 경우 
                    if(o.gunAim.transform.rotation.w <0)
                        attackQuaternion.w = attackQuaternion.w * -1;

                    //Bullet생성(오브젝트, 방향, 회전)
                    //Instantiate(o.weaPon.bullet, attackVector, attackQuaternion);
                    Instantiate(o.weaponManager.bullet, o.weaponManager.transform.position, o.weaponManager.transform.rotation);

                    
                    // 공격 후 대쉬
                    o.moveJoystickVector.Set(0f,0f);
                    o.StartCoroutine(o.ArrowAttackDash());
                   
                }                
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
        if (isActionAllStop)
        {
            // 다른 상태에서 actionStop = true 상태이다.
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
        if (isActionAllStop)
        {
            // 다른 상태에서 actionStop = true 상태이다.
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

        isActionAllStop = false;

        // 공격 조이스틱 유니티 액션
        attackJoystick.Drag += (Vector2 v2) =>
        {
            if (v2.sqrMagnitude >= 0.98)
            {
                isActionAllStop = false;
                OrderAction(new Order() { orderTitle = OrderTitle.Attack, parameters = new List<object>() { v2.x, v2.y } });
            }
            else
            {
                isActionAllStop = true;
            }
        };
        // 움직임 조이스틱 유니티 액션
        moveJoystick.Drag += (Vector2 v2) =>
        {
            moveJoystickVector = v2;
        };
        

        var data = GameManager.Instance.GameDB.WeaponManager.GetWeaponData("IronBow");
        var weaponBowl = GameManager.Instance.GameDB.WeaponManager.ToPrefab(data);

        weapon = Instantiate(weaponBowl as BowModel, weaponManager.transform);
        (weaponBowl as BowModel).transform.localPosition    = new Vector3(0.0f, 0.5f, 0f);
        (weaponBowl as BowModel).transform.localScale       = new Vector3(0.3f, 0.3f, 0.3f);
        

    }

    protected override void Update()
    {
        //Debug.Log(moveJoystickVector);
        if(gunAim != null)
        {
            gunAim.transform.position = transform.position;
        }
        
        //무기 애니메이션
        if (Input.GetKeyDown(KeyCode.Z))
        {
            weapon.AttackAnim(1f, null);
        }

    }

    IEnumerator ArrowAttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isArrowDelay = false;
    }


    IEnumerator ArrowAttackDash()
    {
        float   speed                             = 5.0f;
        float   delay                           = 0.5f;
        bool    isMoveJoystickVectorOneLockdown = false;
        Vector2 targetPostion                   = (Vector2)transform.position;;

        var time = 0f;
        isActionAllStop = true;

        while (time < delay)
        {
            if (moveJoystickVector.x != 0 || moveJoystickVector.y != 0)
            {
                if(!isMoveJoystickVectorOneLockdown)
                {
                    Debug.Log("이동 조이스틱" + moveJoystickVector);
                    targetPostion = (targetPostion + moveJoystickVector)*1.0f;

                    isMoveJoystickVectorOneLockdown = true;
                    
                }
                time += Time.deltaTime / 0.5f;
                transform.position = Vector3.Lerp(transform.position, targetPostion, time);
            }
            yield return null;
     
        }
        isActionAllStop = false;
        moveJoystickVector.Set(0f, 0f);
    }

}
