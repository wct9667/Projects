using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    //fields
    Vector3 direction = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Vector3 position = Vector3.zero;


    //properties
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public Vector3 Position
    {
        get { return position; }
    }
    public Vector3 Direction
    {
        get { return direction; }
    }


    [SerializeField]
    float mass = 1f;

    [SerializeField]
    bool applyFriction;

    [SerializeField]
    float frictionCo;

    [SerializeField]
    bool applyGravity;

    [SerializeField]
    Vector3 gravityStrength = Vector3.down;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != Vector3.zero)
        {
            if(velocity.magnitude > .25)
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        }


        //Bounce();
        if (applyGravity)
        {
            ApplyGravity();
        }
        if (applyFriction)
        {
           ApplyFriction();
        }
        velocity += acceleration * Time.deltaTime;

        position += velocity * Time.deltaTime;

        direction = velocity.normalized;
        transform.position = position;

        acceleration = Vector3.zero;

    }

    void ApplyFriction()
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * frictionCo;
        ApplyForce(friction);

    }

    void ApplyGravity()
    {
        acceleration += gravityStrength;
    }

    void Bounce()
    {
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        if (position.y >= height || position.y <= -height)
            velocity.y = -velocity.y;
        if (position.x >= width || position.x <= -width)
            velocity.x = -velocity.x;
    }

    /// <summary>
    /// Applies a force to the object
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }
}
