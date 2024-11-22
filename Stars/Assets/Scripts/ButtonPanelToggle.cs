using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ButtonCanvasToggle : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    
    private bool canvasEnabled;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void ToggleCanvas()
    {
        canvasEnabled = !canvasEnabled;

        canvas.enabled = canvasEnabled;
    }
}
