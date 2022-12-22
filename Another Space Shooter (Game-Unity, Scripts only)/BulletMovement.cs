using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum BulletType { player, enemy, none }
public class BulletMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 5f;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    Vector3 direction = Vector3.zero;

    Vector3 velocity = Vector3.zero;

    private BulletType bulletType;

    //height and width of the screen
    float height;
    float width;

    public BulletType TypeBullet
    {
        get { return bulletType; }
        set
        {
            bulletType = value;
        }
    }
    void Start()
    {
        //check the bullet type and assign a direction based on that type
        if (bulletType == BulletType.enemy)
            direction.y = -1;
        else if (bulletType == BulletType.player)
            direction.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //check the bullet type and assign a direction based on that type
        if (bulletType == BulletType.enemy)
        {
            direction.y = -1;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
           
        
        else if (bulletType == BulletType.player)
        {
            direction.y = 1;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
            

        velocity = direction * speed * Time.deltaTime;

        transform.position += velocity;

        //check position
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didm't use the total height, instead used half and negated it to get the full

        //height checks
        //height checks
        if (transform.position.y >= height)
        {
            gameObject.SetActive(false);
            bulletType = BulletType.none;
        }
        else if (transform.position.y <= -height)
        {
            gameObject.SetActive(false);
            bulletType = BulletType.none;
        }
            


    }
}

