using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 서브 클래스(데이타 참고용 클래스), 구조체 모음
/// </summary>
public class Nodes
{
    [System.Serializable]
    public struct WeaponSetting
    {
        public string weaponTitle;
        public int reinforceCount;
        public string attributeTitle;
        public List<string> statTitles;
        public List<int> statSteps;
    }
}
