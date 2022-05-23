using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDatabase", menuName = "Scriptable Object/EffectDatabase", order = int.MaxValue)]
public class EffectDatabase : ScriptableObject
{
    [System.Serializable]
    public class Node
    {
        public Enums.Effect kind;
        public Effect prefab;
    }
    [SerializeField] private List<Node> effects;
    private Dictionary<Enums.Effect, Effect> effectTable;

    public Effect this[Enums.Effect kind]
    {
        get => effectTable[kind];
    }

    private void OnEnable()
    {
        if(effects != null)
        {
            effectTable = new Dictionary<Enums.Effect, Effect>();
            for(int i = 0, icount = effects.Count; i<icount; i++)
            {
                effectTable.Add(effects[i].kind, effects[i].prefab);
            }
        }
    }
}
