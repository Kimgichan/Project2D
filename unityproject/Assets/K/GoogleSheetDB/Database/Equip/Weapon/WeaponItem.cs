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

    //���� �ε��� �� ���� ������
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
    /// ���� �ΰ��ӿ��� ����Ǵ�(��������) ��ü
    /// </summary>
    private IWeaponPrefab weaponObject;


    /// <summary>
    /// weaponObject�� �Ҵ��ϰ� ����
    /// </summary>
    /// <param name="rigging">�� �ڽ����� �ٿ���</param>
    public void Create(Transform rigging)
    {
        Distory();

        weaponObject = GameObject.Instantiate(GameManager.Instance.GameDB.WeaponManager.GetWeaponPrefab(Weapon).GetTr().gameObject, rigging)
            .GetComponent<IWeaponPrefab>();
    }

    /// <summary>
    /// weaponObject�� ������
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
    ///// coolTime�� OrderAction���� ������ �� ����.
    ///// </summary>
    ///// <param name="attackController"></param>
    ///// <param name="dir"></param>
    //public void Attack(CreatureController attackController, Vector2 dir)
    //{
    //    // ���ݷ�
    //    if(weaponObject == null)
    //    {
    //        Debug.LogError("���� ������ weaponObject �� null");
    //        return;
    //    }

    //    weaponObject.AttackAnim(attackController.Info.Speed,)
    //}

    //private void 

    /// <summary>
    /// timer : attackEvent�� ���� ȣ���� ������, �׸��� ����� �ӵ�<br/>
    /// attackEvent : timer�� ������ ����Ǵ� ���� ����
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="attackEvent"></param>
    public void AttackAnim(float timer, UnityAction attackEvent)
    {
        if((object)weaponObject == null)
        {
            Debug.LogError("���� ������ weaponObject �� null");
            return;
        }

        weaponObject.AttackAnim(timer, attackEvent);
    }
}
