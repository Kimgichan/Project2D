using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderAction = IController.Order;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private float lifeTimer;
    private float currentLifeTime;


    private Dictionary<Enums.Projectile, Queue<Projectile>> projectileManager;
    private List<Enums.Projectile> lifeCycleList;
    private IEnumerator lifeCycleCor;

    void Start()
    {
        projectileManager = new Dictionary<Enums.Projectile, Queue<Projectile>>();
        lifeCycleList = new List<Enums.Projectile>();
    }

    public Projectile Pop(Enums.Projectile projectileKind, IController attackController, Vector3 pos, Vector2 force, OrderAction[] sendEvent)
    {
        currentLifeTime = lifeTimer;
        Projectile projectile;

        if (projectileManager.TryGetValue(projectileKind, out Queue<Projectile> q))
        {
            projectile = q.Dequeue();
            projectile.gameObject.transform.parent = null;
            projectile.gameObject.SetActive(true);
            projectile.Shot(attackController, pos, force, sendEvent);
            if(q.Count <= 0)
            {
                projectileManager.Remove(projectileKind);
                lifeCycleList.Remove(projectileKind);
            }
        }
        else
        {
            //Instantiate(GameManager.Instance.GameDB.ProjectileDB[projectileKind]).Shot(attackController, pos, force, sendEvent);
            projectile = Instantiate(GameManager.Instance.GameDB.ProjectileDB[projectileKind]);
            projectile.Shot(attackController, pos, force, sendEvent);
        }

        return projectile;
    }

    public void Push(Projectile projectile)
    {
        projectile.transform.SetParent(transform);
        if (projectileManager.TryGetValue(projectile.Kind, out Queue<Projectile> q))
        {
            currentLifeTime = lifeTimer;
            q.Enqueue(projectile);
        }
        else
        {
            var newQ = new Queue<Projectile>();
            newQ.Enqueue(projectile);
            projectileManager.Add(projectile.Kind, newQ);
            lifeCycleList.Add(projectile.Kind);

            if(lifeCycleCor != null)
            {
                StopCoroutine(lifeCycleCor);
            }
            lifeCycleCor = LifeCycleCor();
            StartCoroutine(lifeCycleCor);
        }
    }

    private void OnEnable()
    {
        if (lifeCycleCor != null)
            StartCoroutine(lifeCycleCor);
    }

    private IEnumerator LifeCycleCor()
    {
        currentLifeTime = lifeTimer;
        yield return null;

        while(lifeCycleList.Count > 0)
        {
            currentLifeTime -= Time.deltaTime;
            if(currentLifeTime < 0)
            {
                currentLifeTime = lifeTimer;

                for(int i = 0, icount = lifeCycleList.Count; i<icount;)
                {
                    //Destroy(projectileManager[lifeCycleList[i]].Dequeue().gameObject);
                    var q = projectileManager[lifeCycleList[i]];

                    if (q.Count > 0)
                    {
                        Destroy(q.Dequeue().gameObject);
                    }

                    if(q.Count > 0)
                    {
                        i += 1;
                    }
                    else
                    {
                        projectileManager.Remove(lifeCycleList[i]);
                        lifeCycleList.RemoveAt(i);
                        icount -= 1;
                    }
                }
            }
            yield return null;
        }

        lifeCycleCor = null;
    }
}
