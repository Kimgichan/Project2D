using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    #region 컨트롤러 이펙트 목록

    private Dictionary<Enums.Effect, UnityAction<ObjectController, Vector2>> objectControllerEffectPlayTable;

    private Dictionary<Enums.Effect, UnityAction<CreatureController, Vector2>> creatureControllerEffectPlayTable;


    private void Start()
    {
        ObjectEffectInit();
        CreatureEffectInit();
    }

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
    }

    private void CreatureArrowBasePlay(CreatureController controller, Vector2 dir)
    {
        var arrow = GameManager.Instance.EffectManager.Pop(Enums.Effect.Arrow_Base) as Arrow;

        arrow.gameObject.SetActive(true);

        Vector2 force = dir * controller.Info.Speed;
        arrow.Play(controller, controller.transform.position, force,
            (hitTarget) =>
            {
                hitTarget.OrderDamage(controller.Info.RandomDamage);
                hitTarget.OrderPushed(dir.normalized * controller.Info.PushEnergy);
            });
    }

    private void CreatureShockBasePlay(CreatureController controller, Vector2 dir)
    {
        var shock = GameManager.Instance.EffectManager
            .Pop(Enums.Effect.Shock_Base) as Shock;

        shock.gameObject.SetActive(true);
        shock.Play(controller, controller.transform.position, controller.Info.AttackRange * 0.5f, controller.Info.Speed,
          (hitTarget) =>
          {
              hitTarget.OrderDamage(controller.Info.RandomDamage);
              hitTarget.OrderPushed(dir.normalized * controller.Info.PushEnergy);
          });
    }

    #endregion

    #endregion


    #region 데코레이터 매니저

    [SerializeField] private DecoratorManager decoratorManager;
    public DecoratorManager DecoratorManager => decoratorManager;

    #endregion
}
