using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IState
{
    private UCharacter myCharacter;

    bool isStateaChange = true;

    public bool getStateChange()
    {
        return isStateaChange;
    }

    public string getStringState()
    {
        return "Follow";
    }

    public FollowState(UCharacter _character)
    {
        SetParent(_character);
    }

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;

        foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(1,1),0.4f))
        {

        }


    }

    // Update is called once per frame
    public void Update()
    {
        myCharacter.transform.position = Vector3.MoveTowards(myCharacter.transform.position, myCharacter.GetotherColliderVector, 1.0f * Time.deltaTime);
        myCharacter.GetSpumPrefabs.PlayAnimation(1);
    }
}
