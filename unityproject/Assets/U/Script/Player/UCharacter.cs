using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UCharacter : MonoBehaviour
{
    protected IState        myState;
    protected string        state;
    protected Vector2       moveVector;
    protected SPUM_Prefabs  spumPrefabs;
    protected BoxCollider2D boxCollider2D;
    protected Vector3       otherColliderVector;

    public    Rigidbody2D   myRigidbody;

    public      float       hp          = 1.0f;
    protected   float       speed       = 3.0f;
    private     bool        hitDelay    = false;

    public Vector2          GetMoveVector           { get { return moveVector; } }
    public SPUM_Prefabs     GetSpumPrefabs          { get { return spumPrefabs; } }
    public Vector3          GetotherColliderVector  { get { return otherColliderVector; } }
    public float            GetSpeed                { get { return speed; } }

    public void hitHP(float _damage)
    {
        if (!hitDelay)
        {
            StartCoroutine(ArrowAttackDelay());
            hitDelay = true;
            this.hp -= _damage;
            
        }
    }

    protected virtual void Start()
    {
    }

    protected void ChangeState(IState _changeState)
    {
    }

    protected virtual void Update()
    {
    }

    IEnumerator ArrowAttackDelay()
    {
        yield return new WaitForSeconds(1);
        hitDelay = false;
    }
}
