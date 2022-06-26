using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecoratorManager : MonoBehaviour
{

    #region 크리쳐 전용 데코레이터

    #region EquipmentDecorator 목록

    private Dictionary<Enums.Effect,
            UnityAction<CreatureController,
            EquipmentDecorator,
            Vector2>>
        equipmentEffectPlayTable;

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

    }

    private void EquipmentShockBasePlay(CreatureController controller, EquipmentDecorator equip, Vector2 dir)
    {

    }

    #endregion

    #endregion


    #region 모노비헤이비어 API

    private void Start()
    {
        
    }

    #endregion
}
