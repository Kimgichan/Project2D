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
        // 캐릭터 이동
        myCharacter.transform.Translate(myCharacter.GetMoveVector * myCharacter.GetSpeed * Time.deltaTime);
        // 애니메이션 재생
        myCharacter.GetSpumPrefabs.PlayAnimation(1);
    }
}
