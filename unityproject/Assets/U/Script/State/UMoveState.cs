using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    private UCharacter myCharacter;

    public void SetParent(UCharacter _character)
    {
        myCharacter = _character;
        
        
    }

    // Update is called once per frame
    public void Update()
    {
        // ĳ���� �̵�
        myCharacter.transform.Translate(myCharacter.GetMoveVector * myCharacter.GetSpeed * Time.deltaTime);
        // �ִϸ��̼� ���
        myCharacter.GetSpumPrefabs.PlayAnimation(1);
    }
}
