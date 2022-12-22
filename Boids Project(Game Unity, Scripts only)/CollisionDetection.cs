using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    //methods for collision Circle
    
    public bool CircleCollision(GameObject object1, GameObject object2, SpriteInfo info, SpriteInfo info2)
    { 
        float x = info.Position.x - info2.Position.x;
        float y = info.Position.y - info2.Position.y;
        //get the radii, and the distances between the centers, use that to determine collision
        float distance = (x* x) + (y * y);
        
        if (distance < (info.Radius+  info2.Radius) * (info.Radius + info2.Radius))
            return true;
        
        return false;
    }

    public bool AABBCollision(GameObject object1, GameObject object2, SpriteInfo info, SpriteInfo info2)
    {

        //if to check collision
        if (info.MinX < info2.MaxX && info.MaxX > info2.MinX && info.MaxY > info2.MinY && info.MinY < info2.MaxY)
            return true;
        return false;
    }
}
