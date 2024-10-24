using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwapIcon : MonoBehaviour
{
    private bool optionsEnabledController = false;

    [SerializeField] private Sprite optionsDisabled;
    [SerializeField] private Sprite optionsEnabled;

    [SerializeField] private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        optionsEnabledController = false;
        image.sprite = optionsDisabled;
    }

    public void SwapIcons()
    {
        optionsEnabledController = !optionsEnabledController;

        if (optionsEnabledController) image.sprite = optionsEnabled;
        else image.sprite = optionsDisabled;
    }
}
