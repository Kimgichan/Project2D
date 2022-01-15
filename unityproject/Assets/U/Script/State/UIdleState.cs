using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private UCharacter myCharacter;

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
