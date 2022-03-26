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
        Debug.Log("����� HP:"+hp);
    }

    protected virtual void Start()
    {
        //myState = new IdleState(this);
        //myState.SetParent(this);
    }

    protected void ChangeState(IState _changeState)
    {
        // ���� ���¿� ������ ���°� ���� ��?
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
        // *SetParent�� ��� Update�� ���ʿ䰡 ���� ��? �ʿ��� �� ���� �� �� ����?
        //myState.SetParent(this);
        //myState.Update();
    }
}
