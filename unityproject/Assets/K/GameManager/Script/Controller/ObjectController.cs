using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    protected Enums.ControllerKind kind = Enums.ControllerKind.None;
    public Enums.ControllerKind Kind => kind;

    #region �Լ� ���

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

    public virtual void OrderDamage(int damage) {}

    public virtual void OrderSuper() {}


    /// <summary>
    /// CreatureController.OrderPathFind�� Ȯ���� �� ��
    /// </summary>
    /// <param name="targetTr"></param>
    public virtual void OrderPathFind(Transform targetTr) {}
    public virtual void OrderPathFindStop() {}


    /// <summary>
    /// CreatureController.OrderSetAI_Style�� Ȯ���� �� ��
    /// </summary>
    /// <param name="attackDist">��뿡�� ������ �����ϴ� �ִ� �Ÿ�(Scale ����)</param>
    /// <param name="targetDist">���� ��������� �Ÿ��� �����Ϸ� �ϴ� ��(Scale ����)</param>
    /// <param name="bellicosity">���������� ������������ ������.<br/>
    /// �� ���� 0~1<br/>
    /// �������� ������.</param>
    public virtual void OrderSetAI_Style(float attackDist, float targetDist, float bellicosity) {}

    /// <summary>
    /// OrderPathFind�� OrderSetAI_Style�� ����� ������
    /// </summary>
    /// <param name="targetTr"></param>
    /// <param name="attackDist"></param>
    /// <param name="targetDist"></param>
    /// <param name="bellicosity"></param>
    public virtual void OrderPlayAI(Transform targetTr, float attackDist, float targetDist, float bellicosity) {}

    /// <summary>
    /// �̸��� �ٸ����� OrderPathFindStop�� ���� ��� ����
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
