using UnityEngine;

public class Constellation : MonoBehaviour
{
    private bool disable = false;
    float timeElapsed = 0f;
    private Vector3 startScale;
    private Vector3 target;

    public Vector3 Target
    {
        set { target = value; }
    }
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
        // Fall
        if (timeElapsed < 12)
        {
            if (transform.position != target)
            {
                transform.position = Vector3.Lerp(transform.position, target, 1 * Time.deltaTime);
            }
            
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

