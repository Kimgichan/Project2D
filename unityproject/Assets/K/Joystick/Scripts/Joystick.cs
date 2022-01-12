using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    //기준 스크린 너비 값
    float referenceScreenWidth = 2960f;
    float cameraflaneDistance = 1225f;


    [SerializeField] float handleRange;
    [SerializeField] float dissolveTimer;
    //차일드 0 -> InputField; 1 -> BG;
    //BG의 차일드 0 -> Handle;
    EventTrigger eventTrigger;
    GameObject inputField;
    GameObject bg;
    GameObject handle;

    Material bgMaterial;
    Material handleMaterial;
    Material pointMaterial;

    //-0.1~0.85
    float percent;

    void Start()
    {
        handleRange *= Screen.width / referenceScreenWidth;
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            handleRange *= canvas.planeDistance / cameraflaneDistance;
        }

        ////////////////////////////////////////////////////////////////>
        // Input 이벤트 셋팅
        inputField = transform.GetChild(0).gameObject;

        bg = transform.GetChild(1).gameObject;
        bg.SetActive(false);

        handle = transform.GetChild(1).GetChild(0).gameObject;

        eventTrigger = inputField.AddComponent<EventTrigger>();


        var entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener(e => 
        {
            var eventData = e as PointerEventData;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out Vector3 worldPoint);

            bg.transform.position = worldPoint;
            bg.SetActive(true);

            //도중에 끊을 수 없고, 끊으려면 따로 변수를 갖고 있어야 해서. 코루틴으로 변경
            //DOTween.To(() => percent = -0.1f, v => percent = v, 0.85f, dissolveTimer).OnUpdate(setPercentMat);

            StopAllCoroutines();
            StartCoroutine(DissolveCor(-0.1f, 0.85f));
        });
        eventTrigger.triggers.Add(entry_PointerDown);


        var entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener(e =>
        {
            //위와 마찬가지
            //DOTween.To(() => percent = 0.85f, v => percent = v, -0.1f, dissolveTimer).
            //OnUpdate(setPercentMat).
            //OnComplete(() => bg.SetActive(false));
            StopAllCoroutines();
            StartCoroutine(DissolveCor(0.85f, -0.1f, () => bg.SetActive(false)));
        });
        eventTrigger.triggers.Add(entry_PointerUp);


        var entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener(e =>
        {
            var eventData = e as PointerEventData;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null, out Vector3 worldPoint);

            var bgP = bg.transform.position;
            var dirV = (worldPoint - bgP);

            if (dirV.sqrMagnitude > handleRange * handleRange)
                handle.transform.position = bgP + dirV.normalized * handleRange;
            else handle.transform.position = bgP + dirV;
        });
        eventTrigger.triggers.Add(entry_Drag);
        ////////////////////////////////////////////////////////////////>


        percent = -0.1f;

        bgMaterial = bg.GetComponent<RawImage>().material;
        handleMaterial = handle.GetComponent<RawImage>().material;
        pointMaterial = handle.transform.GetChild(0).GetComponent<RawImage>().material;
        SetDisolvePercent();
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
