using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    GameObject bomber;

    [SerializeField]
    Transform parent;

    [SerializeField]
    int startingEnemies;
    [SerializeField]
    int startingBombers;

    [SerializeField]
    Sprite sprite;

    [SerializeField]
    Sprite spriteBomber;

    float height;
    float width;


    public static EnemySpawner SharedInstance;

    //list of enemies
    List<GameObject>enemies = new List<GameObject>();

    //list of components
    List<EnemyShoot>EnemyShooting = new List<EnemyShoot>();
    List<SpriteRenderer>Renderers = new List<SpriteRenderer>();
    List<Animator> animators = new List<Animator>();
    List<Movement>movements = new List<Movement>();

    public List<Movement> Movements
    {
        get { return movements; }
        set { movements = value; }
    }
    public List<Animator> Animators
    {
        get { return animators; }
    }
    public List<GameObject> Enemies
    {
        get { return enemies; }
    }
    private void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        height = cam.orthographicSize; //didm't use the total height, instead used half and negated it to get the full
        width = height * cam.aspect;

        EnemySpawn(startingEnemies, 1, height, -width, width);
        ActivateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy(1, height, -width, width);
    }



    //method for spawning enemies
    public void EnemySpawn(int number, float heightmin, float heightmax, float widthMin, float widthMax)
    {
        GameObject tmp;
        for (int i = 0; i < number; i++)
        {
            tmp = GameObject.Instantiate(prefab, parent);
            tmp.SetActive(false);

            tmp.transform.position = new Vector3(Random.Range(widthMin, widthMax), heightmax + 2, 0);
            EnemyShooting.Add(tmp.GetComponent<EnemyShoot>());
            Renderers.Add(tmp.GetComponent<SpriteRenderer>());
            animators.Add(tmp.GetComponent<Animator>());
            movements.Add(tmp.GetComponent<Movement>());
            EnemyShooting[i].FireRate = Random.Range(.1f, .75f);

            enemies.Add(tmp);
        }
        for(int i = 0; i < startingBombers; i++)
        {
            tmp = GameObject.Instantiate(bomber, parent);
            tmp.SetActive(false);

            tmp.transform.position = new Vector3(Random.Range(widthMin, widthMax), heightmax + 2, 0);
            EnemyShooting.Add(tmp.GetComponent<EnemyShoot>());
            Renderers.Add(tmp.GetComponent<SpriteRenderer>());
            animators.Add(tmp.GetComponent<Animator>());
            movements.Add(tmp.GetComponent<BomberMovement>());


            enemies.Add(tmp);
        }
    }
    /// <summary>
    /// Activate the enemies
    /// </summary>
    void ActivateEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }

    /// <summary>
    /// Spawns a new enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy(float heightmin, float heightmax, float widthMin, float widthMax)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && !enemies[i].activeInHierarchy)
            {


                movements[i].VehiclePosition = new Vector3(Random.Range(widthMin, widthMax), heightmax + 5, 0);
                enemies[i].SetActive(true);
                if (movements[i] is BomberMovement)
                {
                    Renderers[i].sprite = spriteBomber;
                }
                else
                {
                    Renderers[i].sprite = sprite;
                }
            }
        }
    }

}
