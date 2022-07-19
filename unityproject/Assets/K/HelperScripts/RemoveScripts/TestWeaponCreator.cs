using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponCreator : MonoBehaviour
{
    [SerializeField] private CreatureController testController;
    [SerializeField] private WeaponData createWeapon;
    void Start()
    {
        if ((object)testController == null) return;


        var equipDecorator = testController.GetDecorator(Enums.Decorator.Equipment) as EquipmentDecorator;
        if ((object)equipDecorator == null) return;


        equipDecorator.CreateWeapon(createWeapon, testController.transform);
    }
}
