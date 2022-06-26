using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using NaughtyAttributes;

using EquipAttribute = EquipAttributePresetData.AttributeNode;
using WeaponSetting = Nodes.WeaponSetting;

[System.Serializable]
public class WeaponItem
{
    [ReadOnly]
    [SerializeField]private WeaponData weaponData;
    public WeaponData Weapon => weaponData;

    [ReadOnly]
    [SerializeField] private int reinforceCount;
    public int ReinforceCount => reinforceCount;

    [ReadOnly]
    [SerializeField] private EquipAttributePresetData attributeData;
    public EquipAttributePresetData AttributeState => attributeData;
    
    [ReadOnly]
    [SerializeField] private List<EquipAttribute> stats;
    public int AttributeStatCount => stats.Count;
    public EquipAttribute GetStat(int indx) => stats[indx];

    //저장 로드할 때 오고갈 데이터
    public WeaponSetting Data
    {
        get
        {
            var icount = stats.Count;
            var result = new WeaponSetting() { weaponTitle = weaponData.name, reinforceCount = reinforceCount, attributeTitle = attributeData.name, 
                statTitles = new List<string>(icount), statSteps = new List<int>(icount) };
            for(int i = 0; i<icount; i++)
            {
                result.statTitles.Add(stats[i].kind.ToString());
                result.statSteps.Add(stats[i].count);
            }
            return result;
        }
        set
        {
            weaponData = GameManager.Instance.GameDB.WeaponManager.GetWeaponData(value.weaponTitle);
            reinforceCount = value.reinforceCount;
            attributeData = GameManager.Instance.GameDB.WeaponManager.GetEquipAttributePresetData(weaponData.Kind, value.attributeTitle);

            var icount = value.statTitles.Count;
            stats = new List<EquipAttribute>(icount);
            for(int i = 0; i<icount; i++)
            {
                stats.Add(new EquipAttribute() { 
                    kind = (Enums.EquipAttribute)Enum.Parse(typeof(Enums.EquipAttribute), value.statTitles[i]), 
                    count = value.statSteps[i] });
            }
        }
    }


    /// <summary>
    /// 실제 인게임에서 노출되는(보여지는) 객체
    /// </summary>
    private IWeaponPrefab weaponObject;


    /// <summary>
    /// weaponObject를 할당하고 붙임
    /// </summary>
    /// <param name="rigging">의 자식으로 붙여짐</param>
    public void Create(Transform rigging)
    {
        Distory();

        weaponObject = GameObject.Instantiate(GameManager.Instance.GameDB.WeaponManager.GetWeaponPrefab(Weapon).GetTr().gameObject, rigging)
            .GetComponent<IWeaponPrefab>();
    }

    /// <summary>
    /// weaponObject를 삭제함
    /// </summary>
    public void Distory()
    {
        if (weaponObject != null)
        {
            GameObject.Destroy(weaponObject.GetTr().gameObject);
            weaponObject = null;
        }
    }


    ///// <summary>
    ///// coolTime은 OrderAction으로 정의할 수 있음.
    ///// </summary>
    ///// <param name="attackController"></param>
    ///// <param name="dir"></param>
    //public void Attack(CreatureController attackController, Vector2 dir)
    //{
    //    // 공격력
    //    if(weaponObject == null)
    //    {
    //        Debug.LogError("웨폰 아이템 weaponObject 값 null");
    //        return;
    //    }

    //    weaponObject.AttackAnim(attackController.Info.Speed,)
    //}

    //private void 

    /// <summary>
    /// timer : attackEvent를 언제 호출할 것인지, 그리고 모션의 속도<br/>
    /// attackEvent : timer가 끝나면 진행되는 공격 정의
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="attackEvent"></param>
    public void AttackAnim(float timer, UnityAction attackEvent)
    {
        if((object)weaponObject == null)
        {
            Debug.LogError("웨폰 아이템 weaponObject 값 null");
            return;
        }

        weaponObject.AttackAnim(timer, attackEvent);
    }
}
