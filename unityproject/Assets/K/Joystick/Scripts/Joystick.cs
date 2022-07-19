using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;


public class Joystick : MonoBehaviour
{
    //[SerializeField] [ColorUsage(true, true)] Color hdrColor;
    [SerializeField] Canvas canvas;
    //���� ��ũ�� �ʺ� ��
    float referenceScreenWidth = 2960f;
    float cameraflaneDistance = 1225f;

    public Vector2 input;

    [SerializeField] float handleRange;
    [SerializeField] float dissolveTimer;
    //���ϵ� 0 -> InputField; 1 -> BG;
    //BG�� ���ϵ� 0 -> Handle;
    EventTrigger eventTrigger;
    GameObject inputField;
    GameObject bg;
    GameObject handle;

    Material bgMaterial;
    Material handleMaterial;
    Material pointMaterial;

    //-0.1~0.85
    float percent;

    private EventTrigger.Entry entry_pointerDown;
    public EventTrigger.TriggerEvent pointerDownCallBack => entry_pointerDown.callback;

    private EventTrigger.Entry entry_pointerUp;
    public EventTrigger.TriggerEvent pointerUpCallBack => entry_pointerUp.callback;

    private EventTrigger.Entry entry_drag;
    public EventTrigger.TriggerEvent dragCallBack => entry_drag.callback;

    public bool checkStart { private set; get; }

    private void Awake()
    {
        checkStart = false;
    }

    void Start()
    {
        handleRange *= Screen.width / referenceScreenWidth;
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            handleRange *= canvas.planeDistance / cameraflaneDistance;
        }

        ////////////////////////////////////////////////////////////////>
        // Input �̺�Ʈ ����
        inputField = transform.GetChild(0).gameObject;

        bg = transform.GetChild(1).gameObject;
        bg.SetActive(false);

        handle = transform.GetChild(1).GetChild(0).gameObject;

        eventTrigger = inputField.AddComponent<EventTrigger>();


        entry_pointerDown = new EventTrigger.Entry();
        entry_pointerDown.eventID = EventTriggerType.PointerDown;
        eventTrigger.triggers.Add(entry_pointerDown);


        entry_pointerUp = new EventTrigger.Entry();
        entry_pointerUp.eventID = EventTriggerType.PointerUp;
        eventTrigger.triggers.Add(entry_pointerUp);


        entry_drag = new EventTrigger.Entry();
        entry_drag.eventID = EventTriggerType.Drag;
        eventTrigger.triggers.Add(entry_drag);

        CallBackReset();
        ////////////////////////////////////////////////////////////////>


        percent = -0.1f;

        bgMaterial = bg.GetComponent<RawImage>().material;
        handleMaterial = handle.GetComponent<RawImage>().material;
        pointMaterial = handle.transform.GetChild(0).GetComponent<RawImage>().material;
        SetDisolvePercent();

        checkStart = true;
    }

    public void CallBackReset()
    {
        entry_pointerDown.callback.AddListener(e =>
        {
            var eventData = e as PointerEventData;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out Vector3 worldPoint);

            bg.transform.position = worldPoint;
            bg.SetActive(true);

            //���߿� ���� �� ����, �������� ���� ������ ���� �־�� �ؼ�. �ڷ�ƾ���� ����
            //DOTween.To(() => percent = -0.1f, v => percent = v, 0.85f, dissolveTimer).OnUpdate(setPercentMat);

            StopAllCoroutines();
            StartCoroutine(DissolveCor(-0.1f, 0.85f));
        });

        entry_pointerUp.callback.AddListener(e =>
        {
            //���� ��������
            //DOTween.To(() => percent = 0.85f, v => percent = v, -0.1f, dissolveTimer).
            //OnUpdate(setPercentMat).
            //OnComplete(() => bg.SetActive(false));
            StopAllCoroutines();
            StartCoroutine(DissolveCor(0.85f, -0.1f, () => bg.SetActive(false)));
            handle.transform.position = bg.transform.position;
            input = Vector2.zero;
        });

        entry_drag.callback.AddListener(e =>
        {
            var eventData = e as PointerEventData;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out Vector3 worldPoint);

            var bgP = bg.transform.position;
            var dirV = (worldPoint - bgP);

            if (dirV.sqrMagnitude > handleRange * handleRange)
                dirV = dirV.normalized * handleRange;
            handle.transform.position = bgP + dirV;

            input = dirV / handleRange;
        });
    }

    IEnumerator DissolveCor(float start, float end, UnityAction callback = null)
    {
        var timer = 0f;
        var dis = end - start;

        while (true)
        {
            yield return null;

            timer += Time.deltaTime;
            if (timer > dissolveTimer)
            {
                percent = end;
                SetDisolvePercent();
                if (callback != null) callback();
                break;
            }
            percent = start + dis * timer / dissolveTimer;
            SetDisolvePercent();
        }
    }

    private void SetDisolvePercent()
    {
        bgMaterial.SetFloat("_dissolvePercent", percent);
        handleMaterial.SetFloat("_dissolvePercent", percent);
        pointMaterial.SetFloat("_dissolvePercent", percent);
    }
}
