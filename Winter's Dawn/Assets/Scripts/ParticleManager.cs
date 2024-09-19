using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    
    [SerializeField] 
    private uint numberOfParticles = 1000;

    [SerializeField] private Collider spawnArea;
    private Bounds bounds;

    private List<GameObject> particles;
    [SerializeField] private GameObject particle;
    
    // Start is called before the first frame update
    void Start()
    {
        bounds = spawnArea.bounds;
        particles = new List<GameObject>();

        //create particles
        for (int i = 0; i < numberOfParticles; i++) 
        {
            float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
            GameObject temp = Instantiate(particle);
            temp.transform.position = new Vector3(offsetX, offsetY, -1);
	        particles.Add(temp);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject p in particles)
        {
            Vector3 newPos = p.transform.position;
            //wrap around the screen
            if (newPos.x > bounds.extents.x)
            {
                newPos.x = 0;
            }
            else if (newPos.x < -bounds.extents.x)
            {
                newPos.x = bounds.extents.x;
            }

            if (newPos.y < -bounds.extents.y || newPos.y > bounds.extents.y)
            {
                newPos.y = bounds.extents.y;
            }

            p.transform.position = newPos;
        }
    }

}
