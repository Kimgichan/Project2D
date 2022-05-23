using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectPool : MonoBehaviour
{
    [SerializeField] private float lifeTimer;
    private float currentLifeTime;

    private Dictionary<Enums.Effect, Queue<Effect>> effectManager;
    private List<Enums.Effect> lifeCycleList;
    private IEnumerator lifeCycleCor;

    // Start is called before the first frame update
    void Start()
    {
        effectManager = new Dictionary<Enums.Effect, Queue<Effect>>();
        lifeCycleList = new List<Enums.Effect>();
    }
    #region 각 Effect마다 매개변수 유형 지정해 줄 것
    public Effect Pop(Enums.Effect effectKind, in PickUpEcho.ParametersNode parameters)
    {
        return MainPop(effectKind, parameters);
    }
    #endregion
    private Effect MainPop(Enums.Effect effectKind, object parameters)
    {
        Effect effect;
        currentLifeTime = lifeTimer;
        if(effectManager.TryGetValue(effectKind, out Queue<Effect> q))
        {
            effect = q.Dequeue();
            effect.gameObject.transform.parent = null;
            effect.gameObject.SetActive(true);
            effect.Show(parameters);
            if(q.Count <= 0)
            {
                effectManager.Remove(effectKind);
                lifeCycleList.Remove(effectKind);
            }
        }
        else
        {
            effect = Instantiate(GameManager.Instance.GameDB.EffectDB[effectKind]);
            effect.Show(parameters);
        }
        return effect;
    }
    public void Push(Effect effect)
    {
        effect.transform.SetParent(transform);
        if(effectManager.TryGetValue(effect.Kind, out Queue<Effect> q))
        {
            currentLifeTime = lifeTimer;
            q.Enqueue(effect);
        }
        else
        {
            var newQ = new Queue<Effect>();
            newQ.Enqueue(effect);
            effectManager.Add(effect.Kind, newQ);
            lifeCycleList.Add(effect.Kind);

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

                for(int i = 0, icount = lifeCycleList.Count; i < icount;)
                {
                    var q = effectManager[lifeCycleList[i]];

                    if(q.Count > 0)
                    {
                        Destroy(q.Dequeue().gameObject);
                    }

                    if(q.Count > 0)
                    {
                        i += 1;
                    }
                    else
                    {
                        effectManager.Remove(lifeCycleList[i]);
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
