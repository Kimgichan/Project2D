using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
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
        // �ִϸ��̼� ���
        myCharacter.GetSpumPrefabs.PlayAnimation(0);
    }
}
