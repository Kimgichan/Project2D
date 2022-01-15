using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class GunAim : MonoBehaviour
{
    [Header("에임 조이스틱")]
    [SerializeField] private Joystick aimJoystick;

    [Header("에임선")]
    [SerializeField] private GameObject aimPoint;
    private Material aimPointMat;
    [SerializeField] private float aimRotMaxSpeed;
    private float aimRotSpeed;

    [Header("시야 범위")]
    [SerializeField] private GameObject targetRange;
    private Material targetRangeMat;
    [SerializeField] private float lineSpeed;
    [SerializeField] private float recovoryTimer; // 시야 길이가 maxScale까지 차오르는 속도
    [SerializeField] private float maxDegree;


    private TweenerCore<float, float, FloatOptions> rangeTweenCore;
    private IEnumerator aimRotCor;

    private IEnumerator Start()
    {
        aimPointMat = aimPoint.GetComponent<SpriteRenderer>().material;
        targetRangeMat = targetRange.GetComponent<SpriteRenderer>().material;

        if(aimJoystick != null)
        {
            while (!aimJoystick.checkStart)
            {
                yield return null;
            }

            targetRangeMat.SetFloat("_length", 0f);
            targetRangeMat.SetFloat("_degree", 0f);

            //처음에는 이 함수를 호출할 필요가 없지만 재사용할 때는 반드시 필요한 함수. 그런 의미에서 형식적인 실행
            //즉 지금 이 함수를 호출하지 않아도 정상적으로 작동함.
            aimJoystick.CallBackReset();

            //조이스틱이 눌렸을 때
            aimJoystick.pointerDownCallBack.AddListener((e) =>
            {
                gameObject.SetActive(true);

                if (rangeTweenCore != null)
                {
                    rangeTweenCore.Kill();
                }

                var startValue = targetRangeMat.GetFloat("_length");
                rangeTweenCore = DOTween.To(() => startValue, x => startValue = x, 0.5f, recovoryTimer).OnUpdate(() => 
                {
                    targetRangeMat.SetFloat("_length", startValue);
                    aimRotSpeed = aimRotMaxSpeed * startValue * 2f;
                    targetRangeMat.SetFloat("_degree", maxDegree * startValue * 2f);
                });

                if(aimRotCor == null)
                {
                    aimRotCor = AimRotCor();
                    StartCoroutine(aimRotCor);
                }
            });

            //조이스틱을 땠을 때
            aimJoystick.pointerUpCallBack.AddListener((e) =>
            {
                if(rangeTweenCore != null)
                {
                    rangeTweenCore.Kill();
                }

                var startValue = targetRangeMat.GetFloat("_length");
                rangeTweenCore = DOTween.To(() => startValue, x => startValue = x, 0f, recovoryTimer).OnUpdate(() =>
                {
                    targetRangeMat.SetFloat("_length", startValue);
                    aimRotSpeed = aimRotMaxSpeed * startValue * 2f;
                    targetRangeMat.SetFloat("_degree", maxDegree * startValue * 2f);
                }).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    rangeTweenCore = null;
                });
            });

            //조이스틱을 드래그 할 때
            aimJoystick.dragCallBack.AddListener((e) =>
            {
                if (aimJoystick.input == Vector2.zero) return;

                var rot = transform.eulerAngles;
                var aimDir = aimJoystick.input.normalized;
                var aimDir3 = new Vector3(aimDir.x, aimDir.y, 0f);
                rot.z = Quaternion.FromToRotation(Vector3.up, aimDir3).eulerAngles.z;
                transform.eulerAngles = rot;
            });

            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        targetRangeMat?.SetFloat("_sineMove", targetRangeMat.GetFloat("_sineMove") + (-lineSpeed) * Time.deltaTime);
    }

    private IEnumerator AimRotCor()
    {
        while (true)
        {
            var angle = aimPoint.transform.eulerAngles;
            angle.z -= aimRotSpeed * Time.deltaTime;
            aimPoint.transform.eulerAngles = angle;
            yield return null;
        }
    }

    private void OnDisable()
    {
        aimRotCor = null;
    }
}
