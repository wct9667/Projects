using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowManager : MonoBehaviour
{
    [SerializeField] private Button calibrateButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button locationButton;
    
    [SerializeField] private Canvas calibrateButtonCanvas;
    [SerializeField] private Canvas optionsButtonCanvas;
    [SerializeField] private Canvas locationButtonCanvas;

    private void Start()
    {
       calibrateButton.onClick.AddListener(EnableUICalibrate);
       optionsButton.onClick.AddListener(EnableUIOptions);
       locationButton.onClick.AddListener(EnableUILocation);
    }

    private void EnableUICalibrate()
    {
        optionsButtonCanvas.enabled = false;
        locationButtonCanvas.enabled = false;
    }
    
    private void EnableUIOptions()
    {
        calibrateButtonCanvas.enabled = false;
        locationButtonCanvas.enabled = false;
    }
    
    private void EnableUILocation()
    {
        optionsButtonCanvas.enabled = false;
        calibrateButtonCanvas.enabled = false;
    }
}
