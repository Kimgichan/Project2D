using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class GunAim : MonoBehaviour
{
    [Header("���� ���̽�ƽ")]
    [SerializeField] private Joystick aimJoystick;

    [Header("���Ӽ�")]
    [SerializeField] private GameObject aimPoint;
    private Material aimPointMat;
    [SerializeField] private float aimRotMaxSpeed;
    private float aimRotSpeed;

    [Header("�þ� ����")]
    [SerializeField] private GameObject targetRange;
    private Material targetRangeMat;
    [SerializeField] private float lineSpeed;
    [SerializeField] private float recovoryTimer; // �þ� ���̰� maxScale���� �������� �ӵ�
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

            //ó������ �� �Լ��� ȣ���� �ʿ䰡 ������ ������ ���� �ݵ�� �ʿ��� �Լ�. �׷� �ǹ̿��� �������� ����
            //�� ���� �� �Լ��� ȣ������ �ʾƵ� ���������� �۵���.
            aimJoystick.CallBackReset();

            //���̽�ƽ�� ������ ��
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

            //���̽�ƽ�� ���� ��
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

            //���̽�ƽ�� �巡�� �� ��
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
