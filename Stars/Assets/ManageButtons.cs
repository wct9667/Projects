using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{

    [SerializeField] private Canvas enableAllContButton;

    [SerializeField] private Canvas disableAllConstButton;
    // Start is called before the first frame update
    void Start()
    {
        //disableAllConstButton.SetActive(false);
        //enableAllContButton.SetActive(true);
    }

    public void OnEnableConst()
    {
        disableAllConstButton.enabled = true;
        enableAllContButton.enabled = false;
    }

    public void OnDisableConst()
    {
        disableAllConstButton.enabled = false;
        enableAllContButton.enabled = true;
    }


}
