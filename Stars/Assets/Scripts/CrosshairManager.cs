using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairManager
    : MonoBehaviour
{
    [SerializeField] private bool enabledConst = false;
    [SerializeField] private bool enabledDraw = false;

    [SerializeField] private UnityEngine.UI.Image imageConst;
    [SerializeField] private UnityEngine.UI.Image imageDraw;

    public void EnableOrDisableConstCrossHair()
    {
        enabledConst = !enabledConst;
        imageConst.enabled = enabledConst;
        if (!enabledConst)
        {
            imageDraw.enabled = false;
            enabledDraw = false;
        }
    }
    
    public void EnableOrDisableConstCrossHairDraw()
    {
        if (!enabledConst) return;
        enabledDraw = !enabledDraw;
        imageDraw.enabled = enabledDraw;
    }

    private void Start()
    {
        imageConst.enabled = enabledConst;
        imageDraw.enabled = enabledDraw;
    }
}
