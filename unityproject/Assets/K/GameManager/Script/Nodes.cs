using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� Ŭ����(����Ÿ ����� Ŭ����), ����ü ����
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
