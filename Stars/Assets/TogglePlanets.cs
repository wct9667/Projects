using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlanets : MonoBehaviour
{
    private bool isOn = false;
    private bool hamburgerOn = false;
    [SerializeField] private GameObject image;
    public void Onclick()
    {
        isOn = !isOn;
        if (!isOn) image.SetActive(false);
        
        else if(!hamburgerOn) image.SetActive(true);
    }
    
    public void OnclickOff()
    {
        isOn = false;
        image.SetActive(false);
        hamburgerOn = !hamburgerOn;
    }
}
