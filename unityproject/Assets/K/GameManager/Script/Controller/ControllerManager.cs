using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    #region ũ���� DB
    [SerializeField] private CreatureDatabase creatureDatabase;
    public CreatureDatabase CreatureDatabase => creatureDatabase;
    #endregion


    private void Start()
    {
        ObjectEffectInit();
        CreatureEffectInit();
        ControllerSearchInit();
    }

    #region ��Ʈ�ѷ� ����Ʈ ���

    private Dictionary<Enums.Effect, UnityAction<ObjectController, Vector2>> objectControllerEffectPlayTable;

    private Dictionary<Enums.Effect, UnityAction<CreatureController, Vector2>> creatureControllerEffectPlayTable;

    #region Object ��Ʈ�ѷ� ����Ʈ �Լ� ���
    private void ObjectEffectInit()
    {
        objectControllerEffectPlayTable = new Dictionary<Enums.Effect, UnityAction<ObjectController, Vector2>>();

        objectControllerEffectPlayTable.Add(Enums.Effect.Arrow_Base, ObjectArrowBasePlay);
        objectControllerEffectPlayTable.Add(Enums.Effect.Shock_Base, ObjectShockBasePlay);
    }

    /// <summary>
    /// ObjectController ����Ʈ Play�ϴ� ������ ����.<br/>
    /// �׷��� ����Ʈ�� ��ġ���� �Լ� �ȿ� ��ġ�� ������ �� �ۿ� ����.<br/>
    /// ���� ����Ʈ�� ��ġ�� �Լ� �ȿ� ��ġ�� �����Ǳ� ��ġ �ʴ´ٸ�<br/>
    /// Creature ��Ʈ�ѷ� ����Ʈ �Լ� ����� �����ؼ�<br/>
    /// ���ο� ��Ʈ�ѷ�, �� ��Ʈ�ѷ��� �ٴ� ���� ����(CreatureController.Info ����)<br/>
    /// �� �ٿ�, ���ο� �̺�Ʈ �Լ� ����� '��Ʈ�ѷ� ����Ʈ ���' �Ʒ��� �ۼ��� �� ��.
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="controller"></param>
    /// <param name="dir"></param>
    public void ObjectEffectPlay(Enums.Effect effect, ObjectController controller, Vector2 dir)
    {
        if(objectControllerEffectPlayTable.TryGetValue(effect, out UnityAction<ObjectController, Vector2> effectFunc))
        {
            effectFunc(controller, dir);
        }
    }

    private void ObjectArrowBasePlay(ObjectController controller, Vector2 dir)
    {

    }

    private void ObjectShockBasePlay(ObjectController controller, Vector2 dir)
    {

    }
    #endregion


    #region Creature ��Ʈ�ѷ� ����Ʈ �Լ� ���

    private void CreatureEffectInit()
    {
        creatureControllerEffectPlayTable = new Dictionary<Enums.Effect, UnityAction<CreatureController, Vector2>>();

        creatureControllerEffectPlayTable.Add(Enums.Effect.Arrow_Base, CreatureArrowBasePlay);
        creatureControllerEffectPlayTable.Add(Enums.Effect.Shock_Base, CreatureShockBasePlay);
        creatureControllerEffectPlayTable.Add(Enums.Effect.Arrow_Target, CreatureArrowTargetPlay);
    }


    /// <summary>
    /// EquipDecorator�� ���� ���� ����Ʈ�� ���⿡�� ȣ��<br/>
    /// CreatureController ���� ����Ʈ(���ݿ�) Play�Լ��� CreatureData�� ������
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="dir"></param>
    public void CreatureEffectPlay(CreatureController controller, Vector2 dir)
    {
        if(creatureControllerEffectPlayTable.TryGetValue(controller.Info.BaseAttack, out UnityAction<CreatureController, Vector2> effectFunc))
        {
            effectFunc(controller, dir);
        }
        else
        {
            Debug.LogError("�������� �ʴ� ����Ʈ�� ȣ���߽��ϴ�.");
        }
    }

    private void CreatureArrowBasePlay(CreatureController controller, Vector2 dir)
    {
        var arrow = GameManager.Instance.EffectManager.Pop(Enums.Effect.Arrow_Base) as Arrow;

        arrow.gameObject.SetActive(true);

        arrow.Play(controller, controller.transform.position, dir,
            (hitTarget) =>
            {
                hitTarget.OrderDamage(controller.RandomDamage);
                hitTarget.OrderPushed(dir.normalized * controller.PushEnergy);
            });
    }

    private void CreatureShockBasePlay(CreatureController controller, Vector2 dir)
    {
        var shock = GameManager.Instance.EffectManager
            .Pop(Enums.Effect.Shock_Base) as Shock;

        shock.gameObject.SetActive(true);
        shock.Play(controller, controller.transform.position, controller.AttackRange * 0.5f, controller.AttackSpeed,
          (hitTarget) =>
          {
              hitTarget.OrderDamage(controller.RandomDamage);
              hitTarget.OrderPushed(dir.normalized * controller.PushEnergy);
          });
    }

    private void CreatureArrowTargetPlay(CreatureController controller, Vector2 dir)
    {
        var arrow = GameManager.Instance.EffectManager.Pop(Enums.Effect.Arrow_Target) as Arrow_Target;

        arrow.gameObject.SetActive(true);

        arrow.Play(controller, controller.transform.position, dir,
            (hitTarget) =>
            {
                hitTarget.OrderDamage(controller.RandomDamage);
                hitTarget.OrderPushed(dir.normalized * controller.PushEnergy);
            });
    }

    #endregion

    #endregion


    #region ���ڷ����� �Ŵ���

    [SerializeField] private DecoratorManager decoratorManager;
    public DecoratorManager DecoratorManager => decoratorManager;

    #endregion

    #region ��Ʈ�ѷ� ��ü Ž��

    #region ���� ���
    [SerializeField] private List<ObjectController> allObjectController;
    [SerializeField] private List<CreatureController> allCreatureController;
    #endregion


    #region �Լ� ���
    private void ControllerSearchInit()
    {
        allObjectController = new List<ObjectController>();
        allCreatureController = new List<CreatureController>();
    }

    public int GetObjectControllerCount() => allObjectController.Count;
    public ObjectController GetObjectController(int indx) => allObjectController[indx];
    public void AddObjectController(ObjectController addController)
    {
        if (allObjectController.Contains(addController))
        {
            Debug.LogError("��Ʈ�ѷ��� �̹� ������");
            return;
        }

        allObjectController.Add(addController);

        if(addController is CreatureController)
        {
            allCreatureController.Add(addController as CreatureController);
        }
    }

    public void RemoveObjectController(ObjectController removeController)
    {
        allObjectController.Remove(removeController);

        if(removeController is CreatureController)
        {
            allCreatureController.Remove(removeController as CreatureController);
        }
    }

    public int GetCreatureControllerCount() => allCreatureController.Count;
    public CreatureController GetCreatureController(int indx) => allCreatureController[indx];


    #endregion

    #endregion
}
