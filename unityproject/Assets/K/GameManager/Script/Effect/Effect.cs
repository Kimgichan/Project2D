using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Effect : MonoBehaviour
{
    [SerializeField] protected Enums.Effect effectKind;
    public Enums.Effect Kind => effectKind;

    //Effect�� ��ӹ��� ������Ʈ�� SetActive(false)���ٴ� Push�� ����� ��
    public virtual void Push() {}
}
