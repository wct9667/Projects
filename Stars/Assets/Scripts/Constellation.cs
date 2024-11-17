using UnityEngine;

public class Constellation : MonoBehaviour
{
    private bool disable = false;
    float timeElapsed = 0f;
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

    public void DisableInstant()
    {
        timeElapsed = 0;
        transform.localScale = startScale;
        disable = false;
        gameObject.SetActive(false);
    }

}

