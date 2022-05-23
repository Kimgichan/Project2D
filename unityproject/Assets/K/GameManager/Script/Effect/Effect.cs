using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Effect : MonoBehaviour
{
    protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    public virtual void Show(object parametersNode) {}

    //Effect를 상속받은 오브젝트는 SetActive(false)보다는 Push를 사용할 것
    public virtual void Push() {}
}
