using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Effect : MonoBehaviour
{
    protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    public virtual void Show(ObjectController requireController, in Vector3 pos, in Vector2 force, List<UnityAction<ObjectController>> sendEvents) {}

    //Effect�� ��ӹ��� ������Ʈ�� SetActive(false)���ٴ� Push�� ����� ��
    public virtual void Push() {}
}
