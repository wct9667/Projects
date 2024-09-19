using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float xSpeed;

    private float ySpeed;

    private float time;

    private float windFactor = 1;
    
    private Transform spawnZone;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        xSpeed = Random.Range(0, .1f);
        ySpeed = -Random.Range(0, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        float updateTime = Time.deltaTime;
        time += Time.deltaTime;
        Vector3 newPos = transform.position;

        if (time >= 10)
        {
            newPos.x += windFactor * xSpeed * updateTime;

            if (xSpeed > 0)
            {
                xSpeed *= -1;
            }

            if (time > 15)
            {
                windFactor -= 4 * updateTime;
            }
            else
            {
                windFactor += 4 * updateTime;
            }

            if (time > 20)
            {
                time = 0;
                windFactor = 1;
            }
        }

        newPos.x += xSpeed * updateTime;
        newPos.y += ySpeed * updateTime;
        
        transform.position = newPos;
    }

}
