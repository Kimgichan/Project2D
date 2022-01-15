using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState: IState
{
    private UCharacter myCharacter;

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;


    }

    // Update is called once per frame
    public void Update()
    {
        myCharacter.GetSpumPrefabs.PlayAnimation(4);
    }
}
