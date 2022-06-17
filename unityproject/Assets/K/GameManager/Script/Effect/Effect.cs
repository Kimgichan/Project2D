using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Effect : MonoBehaviour
{
    [SerializeField] protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    //Effect를 상속받은 오브젝트는 SetActive(false)보다는 Push를 사용할 것
    public virtual void Push() {}
}
