using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionManager : MonoBehaviour
{
    //references to the three objects in a list, and a reference to the vehicle
    [SerializeField]
    ObjectPooler projectileManager;
    [SerializeField]
    GameObject vehicle;

    private float score = 0;
    public float Score
    {
        get { return score; }
    }

    [SerializeField]
    GameObject bubble;

    SpriteRenderer spriteRendererPlayer;
    PlayerMove playerMove;
    public PlayerMove PlayerMove
    {
        get { return playerMove; }
    }

    [SerializeField]
    EnemySpawner enemySpawner;

    private List<GameObject> collidables;
    private List<GameObject> enemies;
    Animator animatorP;
    // Start is called before the first frame update
    void Start()
    {
        collidables = projectileManager.Projectiles;
        animatorP = vehicle.GetComponent<Animator>();
        animatorP.enabled = false;
        enemies = enemySpawner.GetComponent<EnemySpawner>().Enemies;
        spriteRendererPlayer = vehicle.GetComponent<SpriteRenderer>();
        playerMove = vehicle.GetComponent<PlayerMove>();
    }

    //Drawing circles for debugging purposes
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;



        Gizmos.DrawWireSphere(vehicle.GetComponent<SpriteInfo>().Position, vehicle.GetComponent<SpriteInfo>().Radius);
        foreach (GameObject obj in collidables)
        {
            if(obj.activeInHierarchy)
            Gizmos.DrawWireSphere(obj.GetComponent<SpriteInfo>().Position, obj.GetComponent<SpriteInfo>().Radius);

        }
        foreach (GameObject obj in enemies)
        {
            if (obj.activeInHierarchy)
                Gizmos.DrawWireSphere(obj.GetComponent<SpriteInfo>().Position, obj.GetComponent<SpriteInfo>().Radius);
        }
    }

    // Update is called once per frame
    void Update()
    {

        EnemyBulletCollision();
        PlayerBulletCollision();
        playerMove.ChangeSprite();

        if (!bubble.activeInHierarchy)
        {
            bubble.GetComponent<BubbleMove>().Charge = 0;
        }
        if(bubble.GetComponent<BubbleMove>().Charge < 0)
        {
            bubble.SetActive(false);
        }
    }


    //Method that will get called if the projectile is an enemy
    private void EnemyBulletCollision()
    {

            for (int i = 0; i < collidables.Count; i++)
            {
                //check active and if bullet is a enemy
                if (collidables[i].activeInHierarchy && projectileManager.BulletMovements[i].TypeBullet == BulletType.enemy)
                {
                    //check collision
                    if (GetComponent<CollisionDetection>().CircleCollision(collidables[i], vehicle))
                    {
                        //run what happens when they collide
                        collidables[i].SetActive(false);

                        if (!bubble.activeInHierarchy)
                        {
                            StartCoroutine(DelayPlay(.4f, vehicle, animatorP, "Explosion"));
                        }
                    }
                }
            }
    }
    /// <summary>
    /// plays an animation then sets the obj to false, no need to destroy, this one is tailored to the enemy
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="obj"></param>
    /// <param name="anim"></param>
    /// <returns></returns>
    IEnumerator DelaySet(float delay, GameObject obj, Animator anim, string animation)
    {
        anim.enabled = true;
        anim.Play(animation, 0, 0.0f);
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        anim.enabled = false;
    }

    //this does  work for both the player and enemy
    IEnumerator DelaySetMultiple(float delay, GameObject obj, GameObject obj2, Animator anim, Animator anim2, string animation, string animation2, Movement enemy)
    {
        obj2.GetComponent<SpriteRenderer>().color = Color.white;
        anim.enabled = true;
        anim2.enabled = true;
        anim.Play(animation, 0, 0.0f);
        anim2.Play(animation2, 0, 0.0f);
        yield return new WaitForSeconds(delay);
        if (!bubble.activeInHierarchy)
        {
            playerMove.Health = 0;
        }
        anim.enabled = false;
        anim2.enabled = false;
        obj.SetActive(false);
        obj.GetComponent<Movement>().Colliding = false;
        if (playerMove.Health <= 0)
        {
            obj2.SetActive(false);
        }
        enemy.CollidingPlayer = false;
    }
    IEnumerator DelayPlay(float delay, GameObject obj, Animator anim, string animation)
    {
        anim.enabled = true;
        anim.Play(animation, 0, 0.0f);
        yield return new WaitForSeconds(delay);
        playerMove.Health = 0;
        anim.enabled = false;
        if(playerMove.Health <= 0){
            obj.SetActive(false);
        }
    }
   
    private void PlayerBulletCollision()
    {
        Animator anim;
        for (int x = 0; x < enemies.Count; x++)           
        {
            if (enemies[x].activeInHierarchy)
            {
                Movement enemy = enemies[x].GetComponent<Movement>();
                anim = enemySpawner.Animators[x];

                //check collision with player
                if (GetComponent<CollisionDetection>().CircleCollision(enemies[x], vehicle))
                {
                    if (!enemy.CollidingPlayer)
                    {
                        score += 10;
                        enemy.CollidingPlayer = true;
                        //run what happens when they collide
                        StartCoroutine(DelaySetMultiple(.4f, enemies[x], vehicle, anim, animatorP, "ExplosionEnemy", "Explosion", enemy));
                    }

                }
                for (int i = 0; i < collidables.Count; i++)
                    {
                        if (collidables[i].activeInHierarchy && projectileManager.BulletMovements[i].TypeBullet == BulletType.player)
                        {
                            //have to get the component, then check collision
                            if (GetComponent<CollisionDetection>().CircleCollision(collidables[i], enemies[x]))
                            {
                                score += 10;
                            //Used a Coroutine to delay the enemy being set to false

                                collidables[i].SetActive(false);
                                enemy.Colliding = true;
                                StartCoroutine(DelaySet(.4f, enemies[x], anim,"ExplosionEnemy"));
                            }
                            else
                            {
                                enemy.Colliding= false;
                            }
                        }
                    }
                    if (!enemy.Colliding && !enemy.CollidingPlayer)
                    { 
                       anim.Play("EnemyRegular", 0, 0.0f);
                    }
                }
            }
            
        }
    
    }

