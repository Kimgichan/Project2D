using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Effect : MonoBehaviour
{
    protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    public virtual void Show(ObjectController requireController, in Vector3 pos, in Vector2 force, List<UnityAction<ObjectController>> sendEvents) {}

    //Effect를 상속받은 오브젝트는 SetActive(false)보다는 Push를 사용할 것
    public virtual void Push() {}
}
