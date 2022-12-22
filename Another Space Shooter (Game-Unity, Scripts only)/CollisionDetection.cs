using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methods for collision Circle

    public bool CircleCollision(GameObject object1, GameObject object2)
    {
        SpriteInfo info = object1.GetComponent<SpriteInfo>();
        SpriteInfo info2 = object2.GetComponent<SpriteInfo>();

        float x = info.Position.x - info2.Position.x;
        float y = info.Position.y - info2.Position.y;
        //get the radii, and the distances between the centers, use that to determine collision
        float distance = (x* x) + (y * y);
        
        if (distance < (info.Radius+  info2.Radius) * (info.Radius + info2.Radius))
            return true;
        
        return false;
    }
}
