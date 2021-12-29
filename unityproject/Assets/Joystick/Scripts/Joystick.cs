using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    //기준 스크린 너비 값
    float referenceScreenWidth = 2960f;

    [SerializeField] float handleRange;
    //차일드 0 -> InputField; 1 -> BG;
    //BG의 차일드 0 -> Handle;
    EventTrigger eventTrigger;
    GameObject inputField;
    GameObject bg;
    GameObject handle;


    void Start()
    {
        handleRange *= Screen.width / referenceScreenWidth;

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

            bg.transform.position = eventData.position;
            bg.SetActive(true);
        });
        eventTrigger.triggers.Add(entry_PointerDown);


        var entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener(e =>
        {
            bg.SetActive(false);
        });
        eventTrigger.triggers.Add(entry_PointerUp);


        var entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener(e =>
        {
            var eventData = e as PointerEventData;

            var bgP = (Vector2)bg.transform.position;
            var dirV = (eventData.position - bgP);

            if (dirV.sqrMagnitude > handleRange * handleRange)
                handle.transform.position = bgP + dirV.normalized * handleRange;
            else handle.transform.position = bgP + dirV;
        });
        eventTrigger.triggers.Add(entry_Drag);
    }
}
