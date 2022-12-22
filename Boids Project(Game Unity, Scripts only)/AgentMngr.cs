using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AgentMngr : MonoBehaviour
{

    //list of agents, ally, properties as well, as the agents need them
    List<GameObject>allies = new List<GameObject>();
    public List<GameObject> Allies
    {
        get { return allies; }
    }
    List<SpriteInfo> alliesInfo = new List<SpriteInfo>();
    List<GameObject> attackers = new List<GameObject>();
    public List<GameObject> Attackers
    {
        get { return attackers; }
    }
    List<SpriteInfo> attackersInfo = new List<SpriteInfo>();
    [SerializeField]
    List<GameObject>obstacles = new List<GameObject>();
    List<SpriteInfo> bulletInfo = new List<SpriteInfo>();

    public GameObject Obstacle
    {
        set
        {
            obstacles.Add(value);
        }
    }

    [SerializeField]
    CollisionDetection detection;
    
    [SerializeField]
    GameObject allyPrefab;//prefab

    [SerializeField]
    GameObject attackerPrefab;

    [SerializeField]
    GameObject player;


    [SerializeField]
    int numberAllies;//spawn numbers

    [SerializeField]
    int numberAttackers;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    PlayerMove movement;

    public List<GameObject> Obstacles
    {
        get { return obstacles; }
    }
    public List<SpriteInfo> BulletInfo
    {
        get { return bulletInfo; }
    }

    float time = 0; //time

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize; 
        float width = height * cam.aspect;

        //spawn and set references to lists and player
        for (int i = 0; i < numberAllies; i++)
        {
            GameObject temp = Instantiate(allyPrefab, transform);
            temp.GetComponent<Agent>().Player = player;
            temp.transform.position = new Vector3(Random.Range(-width, width), Random.Range(-height,height));
            allies.Add(temp);
            temp.GetComponent<Agent>().Init(this);
            alliesInfo.Add(temp.GetComponent<SpriteInfo>());


        }
        for(int i = 0; i < numberAttackers; i++)
        {
            GameObject temp = Instantiate(attackerPrefab, transform);
            temp.GetComponent<Agent>().Player = player;
            temp.GetComponent<Agent>().Init(this);
            temp.transform.position = new Vector3(Random.Range(-width, width), Random.Range(-height, height));
            attackers.Add(temp);
            attackersInfo.Add(temp.GetComponent<SpriteInfo>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Collisions();
    }


    //makes and sets up obstacle
    public void FireObstacle()
    {
        if(time > 1)
        {
            time = 0;
            GameObject temp = Instantiate(bullet);
            temp.transform.position = movement.transform.position;


            temp.GetComponent<PhysicsObject>().ApplyForce(movement.Direction * 10);
            temp.transform.rotation *= Quaternion.LookRotation(Vector3.back, movement.Direction);
            //temp.transform.rotation *= Quaternion.Euler(0, 0, 180);
            obstacles.Add(temp);
            bulletInfo.Add(temp.GetComponent<SpriteInfo>());
        }

    }
    
    //deletes a projectile from the index
    void Delete(int i)
    {
        if (obstacles[i] != null)
        {
            GameObject temp = obstacles[i];
            obstacles.RemoveAt(i);
            bulletInfo.RemoveAt(i);
            Destroy(temp);
        }
    }

    //obstacle collisions
    private void Collisions()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didn't use the total height, instead used half and negated it to get the full
        float width = height * cam.aspect;
        //loop for each collidable
            for (int i = 0; i < obstacles.Count; i++)
            {
               
                if (obstacles.Count > 0)
                {
                    //loop through the allies
                    for (int j = 0; j < allies.Count; j++)
                    {
                        //check collision
                        if (detection.CircleCollision(allies[j], obstacles[i], alliesInfo[j], bulletInfo[i]))
                        {
                            //change the state
                            allies[j].GetComponent<Ally>().State = AllyState.fleeingPlayer;
                        }
                    }
                    for (int x = 0; x < attackers.Count; x++)
                    {
                        //check collision
                        if (detection.CircleCollision(attackers[x], obstacles[i], attackersInfo[x], bulletInfo[i]))
                        {
                            //change the state
                            attackers[x].GetComponent<Attacker>().State = AttackerState.fleeingPlayer;
                        }
                    }
                }
            //check if out of bounds, if so, delete
            if (bulletInfo[i].Position.x + bulletInfo[i].Radius / 2 > width)
            {
                Delete(i);

            }
            else if (bulletInfo[i].Position.x - bulletInfo[i].Radius / 2 < -width)
            {
                Delete(i);

            }
            else if (bulletInfo[i].Position.y + bulletInfo[i].Radius / 2 > height)
            {
                Delete(i);

            }
            else if (bulletInfo[i].Position.y - bulletInfo[i].Radius / 2 < -height)
            {
                Delete(i);

            }
        }

        
    }
}
