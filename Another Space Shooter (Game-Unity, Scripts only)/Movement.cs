using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected float speed = 1f;

    protected Vector3 vehiclePosition = Vector3.zero;
    public Vector3 VehiclePosition
    {
        get { return vehiclePosition; }
        set { vehiclePosition = value; }
    }

    protected Vector3 direction = Vector3.zero;

    
    protected Vector3 velocity = Vector3.zero;

    //height and width of the screen
    protected float height;
    protected float width;

    //height and width of the sprite
    protected float widthS;
    protected float heightS;

    protected double time; 
    
    protected bool colliding = false;
    protected  bool collidingPlayer  = false;

    public bool Colliding
    {
        get { return colliding; }
        set { colliding = value; }
    }
    public bool CollidingPlayer
    {
        get { return collidingPlayer; }
        set { collidingPlayer = value; }
    }

    protected void Start()
    {
        time = 0;
        vehiclePosition = transform.position;
        widthS = (GetComponent<SpriteRenderer>().bounds.max.x - GetComponent<SpriteRenderer>().bounds.min.x) / 2;
        heightS = (GetComponent<SpriteRenderer>().bounds.max.y - GetComponent<SpriteRenderer>().bounds.min.y) / 2;

        
    }

    // Update is called once per frame
   protected virtual void Update()
    {
        //check position and wrap if needed
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didm't use the total height, instead used half and negated it to get the full
        float width = height * cam.aspect;


        //only call once per 3 seconds
        if (time >= Random.Range(1, 5))
        {
            ChangeDirection(width);
            MoveDown(height);
            time = 0;
        }
        time += Time.deltaTime;

        velocity = direction * speed * Time.deltaTime;

        vehiclePosition += velocity;



        //width checks
        if (vehiclePosition.x + widthS >= width)
            vehiclePosition.x = width - widthS;
        else if (vehiclePosition.x - widthS < -width)
            vehiclePosition.x = -width + widthS;

        //check height, and always reverse direction at a certain height
        if (vehiclePosition.y - heightS <= 0)
        {
            vehiclePosition.y = 0 + heightS;
        }
        if (transform.position.y + heightS >= height * 11/12)
        {
            direction.y = -1;
        }

        transform.position = vehiclePosition;
    }


    //method for randomly changing direction, this is for the enemies to move around on the screen
    //it will be public, as there will be a another manager that will apply this to all of the enemies
    public virtual void ChangeDirection(float width)
    {
        //if statements for the movement, random
        if(vehiclePosition.x == 0)
        {
            direction.x = Random.Range(-10, 11);
        }
            if (vehiclePosition.x > 0 + width/3 && vehiclePosition.x < 0 - width/3)
                direction.x = Random.Range(-10, 11);
            else if (vehiclePosition.x > 0)
                direction.x = Random.Range(-11, 10);
            else if (vehiclePosition.x < 0 - width / 2)
                direction.x =  Random.Range(-10, 11);
           /* else if (vehiclePosition.x > 0 + width * 8/9)
                direction.x = Random.Range(-11, 1);
            else if (vehiclePosition.x < 0 - width * 8/9)
                direction.x = Random.Range(1, 11);*/

        //normalize
        direction.Normalize();

    }
    public virtual void MoveDown(float height)
    {
        if(transform.position.y + heightS >= height)
        {
            direction.y = -1;
        }
        else
        {
            direction.y = Random.Range(-1, 2);
        }
        direction.Normalize();
    }
}