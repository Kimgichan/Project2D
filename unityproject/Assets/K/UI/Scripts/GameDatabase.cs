using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Scriptable Object/GameDatabase", order = int.MaxValue)]
public class GameDatabase : ScriptableObject
{
    [SerializeField] private List<ColorNode> palette;
    private Dictionary<string, Color> paletteDic;
    public Color GetColor(string key) => paletteDic[key];

    private void OnEnable()
    {
        if (palette != null)
        {
            paletteDic = new Dictionary<string, Color>();
            foreach (var node in palette)
                paletteDic.Add(node.key, node.value);
        }
    }

    [System.Serializable]
    public class ColorNode
    {
        public string key;
        public Color value;
    }
}
