using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{

    [SerializeField] private GameObject enableAllContButton;

    [SerializeField] private GameObject disableAllConstButton;
    // Start is called before the first frame update
    void Start()
    {
        disableAllConstButton.SetActive(false);
        enableAllContButton.SetActive(true);
    }

    public void OnEnableConst()
    {
        disableAllConstButton.SetActive(true);
        enableAllContButton.SetActive(false);
    }

    public void OnDisableConst()
    {
        disableAllConstButton.SetActive(false);
        enableAllContButton.SetActive(true);
    }


}
