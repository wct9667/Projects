using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairManager
    : MonoBehaviour
{
    [SerializeField] private bool enabled = false;

    [SerializeField] private UnityEngine.UI.Image image;

    public void EnableOrDisable()
    {
        enabled = !enabled;
        image.enabled = enabled;
    }

    private void Start()
    {
        image.enabled = enabled;
    }
}
