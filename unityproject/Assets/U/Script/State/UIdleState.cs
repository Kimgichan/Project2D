using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    bool isStateaChange = true;

    public bool getStateChange()
    {
        return isStateaChange;
    }

    public IdleState(UCharacter _character)
    {
        SetParent(_character);
    }

    private UCharacter myCharacter;

    public string getStringState()
    {
        return "Idle";
    }

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;
    }

    // Update is called once per frame
    public void Update()
    {
        // 애니메이션 재생
        myCharacter.GetSpumPrefabs.PlayAnimation(0);
    }
}
