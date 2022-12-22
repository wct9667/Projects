using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberMovement : Movement
{
    //change the logic for the change direction and move down

    public override void MoveDown(float height)
    {
        direction.y = -1;
    }
    protected override void Update()
    {
        //check position and wrap if needed
        Camera cam = Camera.main;
        float height = cam.orthographicSize; //didm't use the total height, instead used half and negated it to get the full
        float width = height * cam.aspect;


        //only call once per 3 seconds
        if (time >= Random.Range(1, 2))
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
        if (vehiclePosition.y - heightS <= -height)
        {
            vehiclePosition.y = height + 10;
        }

        transform.position = vehiclePosition;
    }
}
