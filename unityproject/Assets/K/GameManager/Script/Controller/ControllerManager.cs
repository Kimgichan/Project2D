using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    #region 크리쳐 DB
    [SerializeField] private CreatureDatabase creatureDatabase;
    public CreatureDatabase CreatureDatabase => creatureDatabase;
    #endregion


    private void Start()
    {
        ObjectEffectInit();
        CreatureEffectInit();
        ControllerSearchInit();
    }

    #region 컨트롤러 이펙트 목록

    private Dictionary<Enums.Effect, UnityAction<ObjectController, Vector2>> objectControllerEffectPlayTable;

    private Dictionary<Enums.Effect, UnityAction<CreatureController, Vector2>> creatureControllerEffectPlayTable;

    #region Object 컨트롤러 이펙트 함수 목록
    private void ObjectEffectInit()
    {
        objectControllerEffectPlayTable = new Dictionary<Enums.Effect, UnityAction<ObjectController, Vector2>>();

        objectControllerEffectPlayTable.Add(Enums.Effect.Arrow_Base, ObjectArrowBasePlay);
        objectControllerEffectPlayTable.Add(Enums.Effect.Shock_Base, ObjectShockBasePlay);
    }

    /// <summary>
    /// ObjectController 이펙트 Play하는 참고값이 없음.<br/>
    /// 그래서 이펙트의 수치들이 함수 안에 수치로 고정될 수 밖에 없음.<br/>
    /// 만약 이펙트의 수치가 함수 안에 수치로 고정되길 원치 않는다면<br/>
    /// Creature 컨트롤러 이펙트 함수 목록을 참고해서<br/>
    /// 새로운 컨트롤러, 그 컨트롤러에 붙는 참고 정보(CreatureController.Info 같은)<br/>
    /// 를 붙여, 새로운 이벤트 함수 목록을 '컨트롤러 이펙트 목록' 아래에 작성해 줄 것.
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


    #region Creature 컨트롤러 이펙트 함수 목록

    private void CreatureEffectInit()
    {
        creatureControllerEffectPlayTable = new Dictionary<Enums.Effect, UnityAction<CreatureController, Vector2>>();

        creatureControllerEffectPlayTable.Add(Enums.Effect.Arrow_Base, CreatureArrowBasePlay);
        creatureControllerEffectPlayTable.Add(Enums.Effect.Shock_Base, CreatureShockBasePlay);
        creatureControllerEffectPlayTable.Add(Enums.Effect.Arrow_Target, CreatureArrowTargetPlay);
    }


    /// <summary>
    /// EquipDecorator가 없는 공격 이펙트는 여기에서 호출<br/>
    /// CreatureController 전용 이펙트(공격용) Play함수는 CreatureData를 참고함
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
            Debug.LogError("존재하지 않는 이펙트를 호출했습니다.");
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


    #region 데코레이터 매니저

    [SerializeField] private DecoratorManager decoratorManager;
    public DecoratorManager DecoratorManager => decoratorManager;

    #endregion

    #region 컨트롤러 객체 탐색

    #region 변수 목록
    [SerializeField] private List<ObjectController> allObjectController;
    [SerializeField] private List<CreatureController> allCreatureController;
    #endregion


    #region 함수 목록
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
            Debug.LogError("컨트롤러가 이미 존재함");
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
