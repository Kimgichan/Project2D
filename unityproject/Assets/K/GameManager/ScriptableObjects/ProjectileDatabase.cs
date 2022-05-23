using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileDatabase", menuName = "Scriptable Object/ProjectileDatabase", order = int.MaxValue)]
public class ProjectileDatabase : ScriptableObject
{
    [System.Serializable]
    public class Node
    {
        public Enums.Projectile kind;
        public Projectile prefab;
    }
    [SerializeField] private List<Node> projectiles;
    private Dictionary<Enums.Projectile, Projectile> projectileTable;

    public Projectile this[Enums.Projectile kind]
    {
        get => projectileTable[kind];
    }

    private void OnEnable()
    {
        if(projectiles != null)
        {
            projectileTable = new Dictionary<Enums.Projectile, Projectile>();
            for(int i = 0, icount = projectiles.Count; i<icount; i++)
            {
                projectileTable.Add(projectiles[i].kind, projectiles[i].prefab);
            }
        }
    }
}
