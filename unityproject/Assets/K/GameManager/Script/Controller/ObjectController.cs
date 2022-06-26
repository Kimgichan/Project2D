using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    protected Enums.ControllerKind kind = Enums.ControllerKind.None;
    public Enums.ControllerKind Kind => kind;

    #region 함수 목록

    /// <summary>
    /// ObjectController 클래스를 상속받는 자식 클래스는<br/>
    /// Start 함수를 protected override 형태로 정의하고<br/> 
    /// kind값을 정의해줄것.
    /// </summary>
    protected virtual void Start()
    {
        kind = Enums.ControllerKind.Object;
    }
    protected virtual void OnEnable() { }


    #region 컨트롤러가 제공하는 액션 함수 목록

    /// <summary>
    /// 기본(Idle) 상태 명령<para/>
    /// compulsion: true = 강제로 Idle로 변경
    /// </summary>
    /// <param name="compulsion">true = 강제로 Idle로 변경</param>
    public virtual void OrderIdle(bool compulsion){}

    /// <summary>
    /// 이동 명령
    /// </summary>
    /// <param name="dir">어느 방향으로 이동할 지</param>
    public virtual void OrderMove(Vector2 dir) {}

    public virtual void OrderAttack(Vector2 dir) {}

    public virtual void OrderAttackStop() {}

    public virtual void OrderDash() {}

    public virtual void OrderPushed(Vector2 force) {}

    public virtual void OrderShock() {}

    public virtual void OrderDamage(int damage) {}

    public virtual void OrderSuper() {}


    /// <summary>
    /// CreatureController.OrderPathFind를 확인해 볼 것
    /// </summary>
    /// <param name="targetTr"></param>
    public virtual void OrderPathFind(Transform targetTr) {}
    public virtual void OrderPathFindStop() {}


    /// <summary>
    /// CreatureController.OrderSetAI_Style을 확인해 볼 것
    /// </summary>
    /// <param name="attackDist">상대에게 공격을 개시하는 최대 거리(Scale 값임)</param>
    /// <param name="targetDist">상대와 어느정도의 거리를 유지하려 하는 지(Scale 값임)</param>
    /// <param name="bellicosity">공격적인지 수비적인지를 결정함.<br/>
    /// 값 범위 0~1<br/>
    /// 높을수록 공격적.</param>
    public virtual void OrderSetAI_Style(float attackDist, float targetDist, float bellicosity) {}

    /// <summary>
    /// OrderPathFind와 OrderSetAI_Style의 기능이 합쳐짐
    /// </summary>
    /// <param name="targetTr"></param>
    /// <param name="attackDist"></param>
    /// <param name="targetDist"></param>
    /// <param name="bellicosity"></param>
    public virtual void OrderPlayAI(Transform targetTr, float attackDist, float targetDist, float bellicosity) {}

    /// <summary>
    /// 이름은 다르지만 OrderPathFindStop과 같은 기능 수행
    /// </summary>
    public virtual void OrderStopAI() {}
    #endregion


    public Component GetDecorator(Enums.Decorator decorator)
    {
        return GetComponent(Enums.GetDecoratorType(decorator));
    }

    public bool TryGetDecorator(Enums.Decorator decorator, out Component value)
    {
        value = GetDecorator(decorator);
        if((object)value != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
