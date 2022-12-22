using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //time
    float time;


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }


    /// <summary>
    /// fires when the space is pressed(or left click, every 2 seconds
    /// </summary>
    public void OnShoot()
    {

       if (time >= .8f)
        {
            //shoot/make the projectile
            GameObject projectile = ObjectPooler.SharedInstance.GetProjectile();
            if (projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.GetComponent<BulletMovement>().TypeBullet = BulletType.player;
                //the more the player fires, the faster the bullets
                projectile.SetActive(true);
            }

            time = 0;

        }
    }

}
