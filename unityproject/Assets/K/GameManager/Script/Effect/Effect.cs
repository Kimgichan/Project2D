using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Effect : MonoBehaviour
{
    protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    public virtual void Show(object parametersNode) {}

    //Effect�� ��ӹ��� ������Ʈ�� SetActive(false)���ٴ� Push�� ����� ��
    public virtual void Push() {}
}
