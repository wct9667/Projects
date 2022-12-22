using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteInfo : MonoBehaviour
{
    //fields
    SpriteRenderer renderer;
    float maxX;
    float maxY;
    float minX;
    float minY;
    float width;
    float height;
    Vector3 spriteSize;
    Vector3 positon;
    float radius;


    //properties
    public float MaxX
    {
        get { return maxX; }
    }
    public float MaxY
    {
        get { return maxY; }
    }
    public float MinX
    {
        get { return minX; }
    }
    public float MinY
    {
        get { return minY; }
    }
    public float Width
    {
        get { return width; }
    }
    public float Height
    {
        get { return height; }
    }
    public Vector3 SpriteSize
    {
        get { return spriteSize; }
    }
    public Vector3 Position
    {
        get { return positon;}
    }

    public float Radius
    {
        get{return radius;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the fields
        renderer = GetComponent<SpriteRenderer>();
        maxX = renderer.bounds.max.x;
        maxY = renderer.bounds.max.y;
        minX = renderer.bounds.min.x;
        minY = renderer.bounds.min.y;
        height = maxY - minY;
        width = maxX - minX;
        positon = renderer.bounds.center;
        spriteSize = renderer.bounds.size;
        radius = height/2;
    }

    // Update is called once per frame
    void Update()
    {
        //update them
        maxX = renderer.bounds.max.x;
        maxY = renderer.bounds.max.y;
        minX = renderer.bounds.min.x;
        minY = renderer.bounds.min.y;
        height = maxY - minY;
        width = maxX - minX;
        positon = renderer.bounds.center;
        spriteSize = renderer.bounds.size;
    }
}
