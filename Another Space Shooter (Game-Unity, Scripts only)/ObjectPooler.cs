using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    //projectile list
    public static ObjectPooler SharedInstance;
    List<GameObject>projectiles = new List<GameObject>();

    [SerializeField]
    GameObject projectile;

    //list of movement
    List<BulletMovement> bulletMovements = new List<BulletMovement>();

   private int size;

    public List<GameObject> Projectiles
    {
        get { return projectiles; }
    }
    public List<BulletMovement> BulletMovements
    {
        get { return bulletMovements; }
    }

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        size = 20;
        projectiles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < size; i++)
        {
            tmp = Instantiate(projectile);
            bulletMovements.Add(tmp.GetComponent<BulletMovement>());
            tmp.SetActive(false);
            projectiles.Add(tmp);
        }
    }


    public GameObject GetProjectile()
    {
        for (int i = 0; i < size; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                return projectiles[i];
            }
        }
        return null;
    }
}
