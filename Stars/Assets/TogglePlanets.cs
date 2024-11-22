using System;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlanets : MonoBehaviour
{
    private bool isOn = false;
    private bool hamburgerOn = false;
    [SerializeField] private GameObject image;
    [SerializeField] private InputManager input;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Onclick()
    {
        isOn = !isOn;
        if (!isOn) image.SetActive(false);
        
        else if(!hamburgerOn) image.SetActive(true);
        input.enabled = !isOn;
    }
    
    public void OnclickOff()
    {
        isOn = false;
        image.SetActive(false);
        hamburgerOn = !hamburgerOn;
        button.interactable = !hamburgerOn;
        input.enabled = true;
    }
}
