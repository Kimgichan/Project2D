using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum EquipKind { Bow, Sword, Wand, WeaponAll, EquipAll}
    public enum EquipAttribute { Dash, FireCount, Guide}

    public enum EquipState { Shot, Stop, Drop, Disalve} // Shot 공격 중, Stop 쿨탐, Drop 주울 수 있는 상태(화살, 창 등등)
}
