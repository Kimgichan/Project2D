using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IState
{
    private UCharacter myCharacter;

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;

       
    }

    // Update is called once per frame
    public void Update()
    {
        myCharacter.transform.position = Vector3.MoveTowards(myCharacter.transform.position,myCharacter.GetboxColliderVector, 1.0f * Time.deltaTime);
        myCharacter.GetSpumPrefabs.PlayAnimation(1);
    }
}
