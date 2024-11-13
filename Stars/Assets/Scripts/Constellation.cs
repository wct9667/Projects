using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    private bool disable = false;
    float timeElapsed = 0f;
    private bool randomSelected = false;
    private Vector3 startScale;
    public void Disable()
    {
        disable = true;
    }

    private void Update()
    {
        FadeOutAndDisable();
    }

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        randomSelected = !randomSelected;
    }

    private void FadeOutAndDisable()
    {
        if (!disable) return;
        timeElapsed += Time.deltaTime;


        if (timeElapsed < 4) return;
        // Fade out
        if (timeElapsed < 8)
        {
            if(!randomSelected)
                transform.localScale *= 1.05f;// + Time.deltaTime;
            if(randomSelected)
                transform.localScale *= .95f;// + Time.deltaTime;
            return;
        }

        timeElapsed = 0;
        transform.localScale = startScale;
        disable = false;
        gameObject.SetActive(false);
    }
}

