using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecoratorManager : MonoBehaviour
{

    #region 크리쳐 전용 데코레이터

    #region EquipmentDecorator 목록

    #region 이펙트 목록

    private Dictionary<Enums.Effect,
            UnityAction<CreatureController,
            EquipmentDecorator,
            Vector2>>
        equipmentEffectPlayTable;

    private void Start()
    {
        EquipmentEffectInit();
    }

    public void EquipmentEffectInit()
    {
        equipmentEffectPlayTable = new Dictionary<Enums.Effect, UnityAction<CreatureController, EquipmentDecorator, Vector2>>();

        equipmentEffectPlayTable.Add(Enums.Effect.Arrow_Base, EquipmentArrowBasePlay);
        equipmentEffectPlayTable.Add(Enums.Effect.Shock_Base, EquipmentShockBasePlay);
    }

    public void EquipmentEffectPlay(Enums.Effect effect, CreatureController controller, EquipmentDecorator equip, Vector2 dir)
    {
        if(equipmentEffectPlayTable.TryGetValue(effect, out UnityAction<CreatureController, EquipmentDecorator, Vector2> effectFunc))
        {
            effectFunc(controller, equip, dir);
        }
    }

    private void EquipmentArrowBasePlay(CreatureController controller, EquipmentDecorator equip, Vector2 dir)
    {
        //equip.WeaponItem.
        //발사 타임
        equip.WeaponItem.AttackAnim(controller.AttackSpeed, () =>
        {
            var arrow = GameManager.Instance.EffectManager.Pop(Enums.Effect.Arrow_Base) as Arrow;

            arrow.gameObject.SetActive(true);
            controller.OrderAttackStop();
            controller.OrderDash();

            dir = equip.WeaponPivot.parent.up;


            arrow.Play(controller, equip.WeaponPivot.parent.position, dir, (hitTarget) =>
            {
                equip.SendAttackEvent(hitTarget);
                hitTarget.OrderDamage(controller.RandomDamage);
                hitTarget.OrderPushed(dir.normalized * controller.Info.PushEnergy);
            });
        });
    }

    private void EquipmentShockBasePlay(CreatureController controller, EquipmentDecorator equip, Vector2 dir)
    {
        equip.WeaponItem.AttackAnim(controller.AttackSpeed, () =>
        {
            var shock = GameManager.Instance.EffectManager
                .Pop(Enums.Effect.Shock_Base) as Shock;

            shock.gameObject.SetActive(true);

            shock.Play(controller, controller.transform.position, controller.AttackRange * 0.5f, controller.AttackSpeed,
              (hitTarget) =>
              {
                  equip.SendAttackEvent(hitTarget);
                  hitTarget.OrderDamage(controller.RandomDamage);
                  hitTarget.OrderPushed(dir.normalized * controller.Info.PushEnergy);

                  controller.OrderAttackStop();
              });
        });
    }

    #endregion 

    #endregion

    #endregion


    #region 모노비헤이비어 API


    #endregion
}
