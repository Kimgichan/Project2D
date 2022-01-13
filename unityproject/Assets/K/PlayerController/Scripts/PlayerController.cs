using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick moveJoystick;
    [SerializeField] Joystick aimJoystick;
    private IController controller;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<IController>();
    }
}
