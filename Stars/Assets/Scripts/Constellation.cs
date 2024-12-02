using System;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    private bool disable = false;
    float timeElapsed = 0f;
    private Vector3 startScale = new Vector3(1,1,1);
    public void Disable()
    {
        disable = true;
    }

    private void Update()
    {
        FadeOutAndDisable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        timeElapsed = 0;
        transform.localScale = startScale;
        disable = false;
    }
    
    private void FadeOutAndDisable()
    {
        if (!disable) return;
        
        timeElapsed += Time.deltaTime;


       
        // Fall
        if (timeElapsed < 2)
        {
            transform.localScale *=  .75f;
            
            return;
        }

        timeElapsed = 0;
        transform.localScale = startScale;
        disable = false;
        gameObject.SetActive(false);
    }
}

