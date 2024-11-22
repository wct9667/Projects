using UnityEngine;

public class CalibrateUIManager : MonoBehaviour
{
    [SerializeField] private Canvas child;
    

    // Start is called before the first frame update
    void Start()
    {
        child = GetComponentInChildren<Canvas>();
        child.enabled = false;
    }

    public void OnClick()
    {
        child.enabled = !child.enabled;
    }

    public void DisableUI()
    {
        child.enabled = false;
    }
}
