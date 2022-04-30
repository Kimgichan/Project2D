using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class BowModel : MonoBehaviour,   IWeaponPrefab
{
    [SerializeField] private Transform boneLeft;
    [SerializeField] private Transform boneRight;

    private bool anim = false;

    private void Start()
    {
        boneLeft.localRotation = Quaternion.identity;
        boneRight.localRotation = Quaternion.identity;
    }


    public float timer = 1f;


    // 사용 예시로 참고하면 될 듯
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        AttackAnim(timer, null);
    //    }
    //    else if (Input.GetKey(KeyCode.Z))
    //    {
    //        AttackAnim(timer, null);
    //    }
    //    else
    //    {
    //        StopAnim();
    //    }
    //}

    public bool AttackAnim (float timer, UnityAction endEvent)
    {
        if (anim) return false;

        anim = true;


        boneLeft.DOKill();
        boneRight.DOKill();


        boneLeft.localRotation = Quaternion.identity;
        boneLeft.DOLocalRotate(new Vector3(0f, 0f, 45f), timer * 0.8f, RotateMode.Fast).SetEase(Ease.OutCubic);


        boneRight.localRotation = Quaternion.identity;
        boneRight.DOLocalRotate(new Vector3(0f, 0f, -45f), timer * 0.8f, RotateMode.Fast).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            boneLeft.DOLocalRotate(Vector3.zero, timer * 0.2f, RotateMode.Fast).SetEase(Ease.Linear);


            boneRight.DOLocalRotate(Vector3.zero, timer * 0.2f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
            {
                boneLeft.DOKill();
                boneRight.DOKill();


                boneLeft.localRotation = Quaternion.identity;
                boneRight.localRotation = Quaternion.identity;
                anim = false;


                if (endEvent != null) endEvent();
            });
        });
        return true;
    }
    public void StopAnim()
    {
        anim = false;
        boneLeft.DOKill();
        boneRight.DOKill();


        boneRight.DOLocalRotate(Vector3.zero, 0.25f, RotateMode.Fast).SetEase(Ease.Linear);
        boneLeft.DOLocalRotate(Vector3.zero, 0.25f, RotateMode.Fast).SetEase(Ease.Linear);
    }
}
