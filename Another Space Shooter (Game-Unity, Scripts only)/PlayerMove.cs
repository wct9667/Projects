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
    Sprite spriteRight;

    [SerializeField]
    Sprite straight;

    [SerializeField]
    Sprite shield;

    [SerializeField]
    int health;

    public int Health
    {
        get { return health; }
        set { health = health - 1; }
    }


    Vector3 vehiclePosition = Vector3.zero;

    Vector3 direction = Vector3.zero;

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
        


        velocity = direction * speed * Time.deltaTime;

        vehiclePosition += velocity;

        //check position and wrap if needed
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didn't use the total height, instead used half and negated it to get the full
        float width = height * cam.aspect;

        //height checks
        if (vehiclePosition.y + heightS >= height/3)
            vehiclePosition.y = height/3 - heightS;
        else if (vehiclePosition.y - heightS < -height)
            vehiclePosition.y = -height + heightS;

        //width checks
        if (vehiclePosition.x + widthS >= width)
            vehiclePosition.x = width - widthS;
        else if (vehiclePosition.x - widthS < -width)
            vehiclePosition.x = -width + widthS;

        transform.position = vehiclePosition;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    ///chnages the sprite based on the directionn, will be used to reset after an animation.
    public void ChangeSprite()
    {
        if (direction.x == 0)
        {
            renderer.sprite = straight;
            transform.transform.localScale = new Vector3(1, 1, 1);
        }
        if (direction.x > 0)
        {
            renderer.sprite = spriteRight;
            transform.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            renderer.sprite = spriteRight;
            transform.transform.localScale = new Vector3(-1, 1, 1);
        }
    }



}
