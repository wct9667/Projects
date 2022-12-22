using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Pool;
using Unity.VisualScripting;

public class EnemyShoot : MonoBehaviour
{

    float fireRate; // firerate per second

    float time;


    //property for firerate, cannot exceed 1 for the firerate
    public float FireRate
    {
        get { return fireRate; }
        set
        {
            if (fireRate >= 1)
                value = 1;
            fireRate = value;
        }
    }

    private void Awake()
    {
        time = 0;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1/fireRate)
        {
            //shoot/make the projectile
            GameObject projectile = ObjectPooler.SharedInstance.GetProjectile();
            if (projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.GetComponent<BulletMovement>().TypeBullet = BulletType.enemy;
                projectile.GetComponent<Renderer>().material.color = Color.white;
                projectile.SetActive(true);
            }

            time = 0;

        }

    }
}
