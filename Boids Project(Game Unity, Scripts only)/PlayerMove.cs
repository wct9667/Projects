using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    SpriteRenderer renderer;


    [SerializeField]
    Sprite straight;

    [SerializeField]
    Vector3 mousePos;


    Vector3 vehiclePosition = Vector3.zero;

    Vector3 direction = Vector3.zero;
    public Vector3 Direction
    {
        get { return direction1.normalized; }
    }
    Vector3 direction1;

    Vector3 velocity = Vector3.zero;

    //width and height of the sprite
    float heightS;
    float widthS;


    void Start()
    {
        vehiclePosition = transform.position;
        widthS = (renderer.bounds.max.x - renderer.sprite.bounds.min.x)/2;
        heightS = (renderer.bounds.max.y - renderer.sprite.bounds.min.y)/2;

    }

    // Update is called once per frame
    void Update()
    {
        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction) ;
        }

        mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        if(Vector3.Distance(mousePos,vehiclePosition) > .2f)
        {
            direction = mousePos - vehiclePosition;
            direction1 = direction;
        }
        else
        {
            direction = Vector3.zero;
        }
        direction.Normalize();
        velocity = direction * speed * Time.deltaTime;

        vehiclePosition += velocity;

        //check position and wrap if needed
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didn't use the total height, instead used half and negated it to get the full
        float width = height * cam.aspect;
        width *= .9f;
        height *= .85f;

        //height check
        if (vehiclePosition.y + heightS >=  height)
            vehiclePosition.y = height -  heightS;
        
        else if (vehiclePosition.y - heightS < -height)
            vehiclePosition.y = -height + heightS;

        //width checks
        if (vehiclePosition.x + widthS >=  width)
            vehiclePosition.x = width - widthS;
        else if (vehiclePosition.x - widthS < -width)
            vehiclePosition.x = -width + widthS;

        transform.position = vehiclePosition;
    }



    /*uses mouse input 
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }*/





}
