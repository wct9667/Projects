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


        if (timeElapsed < 4) return;
        // Fade out
        if (timeElapsed < 12)
        {
            transform.localScale *= .95f;// + Time.deltaTime;
            
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

    private void OnDisable()
    {
        timeElapsed = 0;
        transform.localScale = startScale;
        disable = false;
    }
}

