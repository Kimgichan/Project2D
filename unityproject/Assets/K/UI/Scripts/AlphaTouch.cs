using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaTouch : MonoBehaviour
{
    [SerializeField] Image alphaTouch;
    void Start()
    {
        alphaTouch.alphaHitTestMinimumThreshold = 0.1f;
    }
}
