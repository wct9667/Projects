using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager
    : MonoBehaviour
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
