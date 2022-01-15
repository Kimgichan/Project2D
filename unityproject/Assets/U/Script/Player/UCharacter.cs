using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UCharacter : MonoBehaviour
{
    protected IState        myState;
    protected Vector2       moveVector;
    protected SPUM_Prefabs  spumPrefabs;
    protected BoxCollider2D boxCollider2D;
    protected Vector3       boxColliderVector;

    public float        hp      = 10f;
    protected float     speed   = 3.0f;

    public Vector2          GetMoveVector           { get { return moveVector; } }
    public SPUM_Prefabs     GetSpumPrefabs          { get { return spumPrefabs; } }
    public Vector3          GetboxColliderVector    { get { return boxColliderVector; } }
    public float            GetSpeed                { get { return speed; } }

    protected virtual void Start()
    {
        Debug.Log("hello");
    }

    protected void ChangeState(IState _changeState)
    {
        myState = _changeState;
    }

    protected virtual void Update()
    {
        // *SetParent�� ��� Update�� ���ʿ䰡 ���� ��? �ʿ��� �� ���� �� �� ����?
        myState.SetParent(this);
        myState.Update();
    }
}
