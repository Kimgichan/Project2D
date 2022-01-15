using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void SetParent(UCharacter _character);
    void Update();
}
