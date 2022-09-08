using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;


public class HUDDecorator : MonoBehaviour
{
    #region 변수 목록
    [SerializeField] private Canvas controllerHUD;
    [SerializeField] private bool showHP;
    [ReadOnly] [SerializeField] private HP_Bar_Base hpBar;
    #endregion

    #region 프로퍼티 목록
    public Canvas HUD
    {
        get
        {
            return controllerHUD;
        }
    }
    #endregion

    #region 함수 목록
    public void ShowHP(CreatureController controller)
    {
        if (!showHP || controllerHUD == null) return;

        if (hpBar == null)
        {
            hpBar = GameManager.Instance.EffectManager.Pop(Enums.Effect.HP_Bar_Base) as HP_Bar_Base;
            if (hpBar == null)
            {
                Debug.LogError("hpBar를 Pop하지 못했음");
                return;
            }

            hpBar.gameObject.SetActive(true);
            hpBar.transform.SetParent(controllerHUD.transform);
            hpBar.Play(controller, () => hpBar = null);
        }
        else
        {
            hpBar.gameObject.SetActive(true);
            hpBar.Play(controller);
        }
    }
    #endregion
}
