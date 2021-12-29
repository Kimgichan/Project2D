using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    //차일드 0 -> InputField; 1 -> BG;
    //BG의 차일드 0 -> Handle;
    EventTrigger eventTrigger;
    GameObject inputField;
    GameObject bg;
    GameObject handle;

    // Start is called before the first frame update
    void Start()
    {
        inputField = transform.GetChild(0).gameObject;
        bg = transform.GetChild(1).gameObject;
        handle = transform.GetChild(1).GetChild(0).gameObject;

        eventTrigger = inputField.AddComponent<EventTrigger>();

        var entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener(e => {
        
        });
        eventTrigger.triggers.Add(entry_PointerDown);


    }
}
