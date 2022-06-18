using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    protected Enums.Projectile projectileKind;
    public Enums.Projectile Kind => projectileKind;

    protected ObjectController attackController;

    protected List<UnityAction<ObjectController>> sendEvents;
    public virtual void Shot(ObjectController attackController, 
        Vector3 pos, Vector2 force, 
        List<UnityAction<ObjectController>> sendEvents = null)
    {}

    //Projectile�� ��ӹ��� ������Ʈ�� SetActive(false)���ٴ� Push�� ����� ��
    public virtual void Push() {}
}
