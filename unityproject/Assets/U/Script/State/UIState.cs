using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    string getStringState();
    void SetParent(UCharacter _character);
    void Update();
}
