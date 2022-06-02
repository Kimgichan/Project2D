using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    protected Enums.ControllerKind kind = Enums.ControllerKind.None;
    public Enums.ControllerKind Kind => kind;


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

    public virtual void OrderDamage() {}

    public virtual void OrderSuper() {}
    #endregion
}
