using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(PhysicsObject))]
public abstract class Agent : MonoBehaviour
{
    //fields 
    [SerializeField]
    protected float maxSpeed  = 2f;//max speed
    
    [SerializeField]
    protected float maxForce = 2f;//max force


    [SerializeField]
    protected PhysicsObject physicsObject; // physics object

    //player
    protected GameObject player;

    [SerializeField]
    protected AgentMngr agentMngr;

    [SerializeField]
    protected float avoidTime = 3f;
    [SerializeField]
    protected float avoidRadius = 1f;

    public void Init(AgentMngr manager)
    {
        agentMngr = manager;
    }


    [SerializeField]
     protected SpriteRenderer renderer;

     protected Vector3 totalForce;

    public GameObject Player
    {
        set
        {
            if(player == null)
            {
                player = value; 
            }
        }
    }



    Vector3 worldSize;

    // Start is called before the first frame update
    void Start()
    {
      //  worldSize.y = Camera.main.orthographicSize;
       // worldSize.x = Camera.main.aspect * worldSize.y;
        physicsObject = GetComponent<PhysicsObject>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSteeringForces();
        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);
    }

    //
    protected abstract void CalculateSteeringForces();

    //function to  obtain the seeking force
    public Vector3 Seek(Vector3 targetPos)
    {
        //calc v
        Vector3 desiredVelocity = targetPos - transform.position;


        //set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //calculate the steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        return seekingForce;
    }

    //using a gameobject
    public Vector3 Seek(GameObject target)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = target.transform.position - transform.position;
        if(Vector3.Distance(transform.position, target.transform.position)< .5f)
        {
            return Vector3.zero;
        }
        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        
        return seekingForce;
    }

    public Vector3 Pursuit(GameObject target)
    {
        Vector3 futurePos = target.transform.position + target.GetComponent<PhysicsObject>().Velocity * 2; //will fix this later to not have to use get
        return Seek(futurePos);
    }

    //uses gameobject, and a radius of detectiom
    public virtual Vector3 Flee(GameObject target)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = physicsObject.Position - target.transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce;
    }
    ///other steering methods
    ///


    //find closest specified object
    protected GameObject FindClosest(List<GameObject> objects)
    {
        float prevDis = Mathf.Infinity;
        GameObject obj = null;
        for (int i = 0; i < objects.Count; i++)
        {
            float distance = Vector3.Distance(objects[i].transform.position, physicsObject.Position);
            if (distance < prevDis)
            {
                obj = objects[i];
                prevDis = distance;
            }
        }
        return obj;
    }


    //separate, makrs sure two objects are not constantly touching
    protected Vector3 Separate(List<GameObject> agents, float separation)
    {

        Vector3 sum = Vector3.zero;
        int count = 0; 

        foreach(GameObject agent in agents)
        {
            float distance = Vector3.Distance(physicsObject.Position, agent.transform.position);
            if(distance < separation && distance > 0)
            {
                Vector3 diff = physicsObject.Position - agent.transform.position;
                diff.Normalize();

                //calculate the average of all vectors
                sum += diff;
                count++;
            }
        }
        if(count > 0)
        {
            sum = (sum/ count);
        }

        //scale my the max speed
        sum *= maxSpeed;

        Vector3 steer = sum - physicsObject.Velocity;

        return steer;
    }
   /* public Vector3 StayInBounds()
    {
        worldSize *= .9f;
        //check if out of bounds
        if (transform.position.x >= worldSize.x || transform.position.x <= -worldSize.x || transform.position.y >= worldSize.y || transform.position.y <= -worldSize.y)
        {
            return Seek(Vector3.zero);
        }
        else
        {
            return Vector3.zero;
        }
    }*/
    /// <summary>
    /// Keeps the agent within the screen window
    /// </summary>
    protected Vector3 StayInBounds(float intensity)
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        float width = height * cam.aspect;
        height *= .9f;
        width *= .9f;
        Vector3 steer = Vector3.zero;

        int count = 0;
        if (transform.position.x + physicsObject.Velocity.magnitude * 2 >= width)
        {
            count++;
            Vector3 desired = new Vector3(-maxSpeed, physicsObject.Velocity.y, 0);
            steer = desired- physicsObject.Velocity;
            
        }
        else if (transform.position.x - physicsObject.Velocity.magnitude * 2 <= -width)
        {
            count++;
            Vector3 desired = new Vector3(maxSpeed, physicsObject.Velocity.y, 0);
            steer = desired - physicsObject.Velocity;

        }
        if (transform.position.y+ physicsObject.Velocity.magnitude * 2 >= height)
        {
            count++;
            Vector3 desired = new Vector3(physicsObject.Velocity.x,-maxSpeed, 0);
            steer = desired - physicsObject.Velocity;
            
        }
        else if (transform.position.y- physicsObject.Velocity.magnitude*2 <= -height)
        {
            count++;
            Vector3 desired = new Vector3(physicsObject.Velocity.x, maxSpeed, 0);
            steer = desired - physicsObject.Velocity;



        }
        if(count > 0)
        return steer * intensity/count;

        else
        {
            return steer * intensity;
        }
    }

    //arrive behavior
    protected Vector3 arrive(GameObject target)
    {

        Vector3 desired = target.transform.position - physicsObject.Position;
        Vector3 steer;
        //The distance is the magnitude of the vector pointing from location to target.
        float d = desired.magnitude;
        desired.Normalize();
        if (d < 1)
        {
            float m = d / 3;
            desired *= m;
        }
        else
        {
            desired *= maxSpeed;
        }

        steer = desired - physicsObject.Velocity;
        return steer;
    }


    //alignment
    protected Vector3 Alignment(List<GameObject> agents)
    {
        /*
        Vector3 totalVelocity = Vector3.zero;
        int total = 0;
        //loop for each agent of same type
        foreach(GameObject agent in agents)
        {
           float distance =  Vector3.Distance(agent.transform.position, transform.position);
            if (distance < .5f)
            {
                totalVelocity += agent.GetComponent<PhysicsObject>().Velocity;//will optimize at some point
                total++;

            }//figure out a good value
        }
        if (total > 0)
            return totalVelocity.normalized / total;
        else*/
            return Vector3.zero;
    }

    protected List<GameObject> tempList = new List<GameObject>();
    //obstacle avoidance
    protected Vector3 AvoidObstacle()
    {
        tempList.Clear();
        Vector3 avoidForce = Vector3.zero;
        Vector3 VTo0 = Vector3.zero;
        float rightDot, forawrdDot;
        Vector3 futurePos = physicsObject.Position + physicsObject.Velocity * (avoidTime);
        float futureSqrDist = Vector3.SqrMagnitude(futurePos - physicsObject.Position);
        futureSqrDist += avoidRadius;

        foreach (GameObject obstacle in agentMngr.Obstacles)
        {
            VTo0 = obstacle.transform.position - physicsObject.Position;
            forawrdDot = Vector3.Dot(VTo0,physicsObject.Velocity.normalized);
            if(forawrdDot >= 0 && forawrdDot * forawrdDot <= futureSqrDist)//not to far in front
            {
                rightDot = Vector3.Dot(VTo0, transform.right);

                if(Mathf.Abs(rightDot) < avoidRadius)
                {
                    tempList.Add(obstacle);
                    if(rightDot < 0)
                    {

                        //turn right, scale with forwardDot
                        if (forawrdDot > 0)
                            avoidForce += transform.right * maxSpeed * (1f / forawrdDot);
                    }
                    else
                    {
                        //turn left
                        if (forawrdDot > 0)
                            avoidForce += -transform.right * maxSpeed * (1f / forawrdDot);
                    }
                }
                
            }
            //check if obstcle is in front


        }
        return avoidForce;
    }

 

}
