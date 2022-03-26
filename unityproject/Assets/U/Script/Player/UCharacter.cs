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

    public float        hp      = 10f;
    protected float     speed   = 3.0f;

    public Vector2          GetMoveVector           { get { return moveVector; } }
    public SPUM_Prefabs     GetSpumPrefabs          { get { return spumPrefabs; } }
    public Vector3          GetotherColliderVector  { get { return otherColliderVector; } }
    public float            GetSpeed                { get { return speed; } }

    public void hitHP(float _damage)
    {
       
        this.hp -= _damage;
        Debug.Log("실행됨 HP:"+hp);
    }

    protected virtual void Start()
    {
        //myState = new IdleState(this);
        //myState.SetParent(this);
    }

    protected void ChangeState(IState _changeState)
    {
        // 현재 상태와 변경할 상태가 같은 가?
        //if (myState.getStringState() == _changeState.getStringState())
        //{
           
        //}
        //else if (myState.getStateChange() == true)
        //{
        //    myState = _changeState;
        //}
        

    }

    protected virtual void Update()
    {
        // *SetParent를 계속 Update를 할필요가 있을 까? 필요할 때 꺼내 쓸 수 없나?
        //myState.SetParent(this);
        //myState.Update();
    }
}
