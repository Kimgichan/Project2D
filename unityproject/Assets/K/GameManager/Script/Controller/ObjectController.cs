using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    protected Enums.ControllerKind kind = Enums.ControllerKind.None;
    public Enums.ControllerKind Kind => kind;


    /// <summary>
    /// ObjectController Ŭ������ ��ӹ޴� �ڽ� Ŭ������<br/>
    /// Start �Լ��� protected override ���·� �����ϰ�<br/> 
    /// kind���� �������ٰ�.
    /// </summary>
    protected virtual void Start() 
    {
        kind = Enums.ControllerKind.Object;
    }
    protected virtual void OnEnable() { }

    #region ��Ʈ�ѷ��� �����ϴ� �׼� �Լ� ���

    /// <summary>
    /// �⺻(Idle) ���� ���<para/>
    /// compulsion: true = ������ Idle�� ����
    /// </summary>
    /// <param name="compulsion">true = ������ Idle�� ����</param>
    public virtual void OrderIdle(bool compulsion){}

    /// <summary>
    /// �̵� ���
    /// </summary>
    /// <param name="dir">��� �������� �̵��� ��</param>
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
