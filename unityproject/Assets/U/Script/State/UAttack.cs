using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState: IState
{
    private UCharacter myCharacter;

    bool isStateaChange = true;

    public bool getStateChange()
    {
        return isStateaChange;
    }

    public string getStringState()
    {
        return "Attack";
    }

    public AttackState(UCharacter _character)
    {
        SetParent(_character);
    }

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;

    }

    // Update is called once per frame
    public void Update()
    {
        myCharacter.GetSpumPrefabs.PlayAnimation(5);
    }
}
