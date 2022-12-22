using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleMove : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float charge;

    public float Charge
    {
        get { return charge; }
        set
        {
            charge += Time.deltaTime;
            if (charge > 5)
                charge = 5;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        if (gameObject.activeSelf)
        {
            charge -= 2*Time.deltaTime;
        }
            
    }


    public void OnSpaceHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (charge > 0)
                gameObject.SetActive(true);
        }
        if (context.canceled)
        {
            gameObject.SetActive(false);
        }

    }
}
