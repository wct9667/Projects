using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour {
  [Range(0, 100)]
  [SerializeField] private float starSizeMin = 0f;
  [Range(0, 100)]
  [SerializeField] private float starSizeMax = 5f;
  

  private List<Star> stars = new List<Star>();
  public List<Star> Stars
  {
    get { return stars; }
  }
  
  private List<MeshRenderer> starMeshRenderers;

  public List<MeshRenderer> StarMeshRenderers
  {
    get { return starMeshRenderers; }
  }
  
  private readonly int starFieldScale = 400;

  [SerializeField] private Star starPrefab;
  
  void Start()
  {
    GenerateStars();
  }

  private void GenerateStars()
  {
    // Read in the star data.
    StarDataLoader sdl = new();
    starMeshRenderers = new();
    List<StarData> starData = new();
    starData = sdl.LoadData();
    foreach (StarData data in starData) {
      
      // Create star game objects.
      Star star = Instantiate(starPrefab, Vector3.zero, Quaternion.identity);
      
      //setup values of star
      star.SetupValues(data.catalogNumber, 
        data.rightAscension, 
        data.declination, 
        data.spectralType, 
        data.spectralIndex, 
        data.magnitude, 
        data.raProperMotion, 
        data.decProperMotion);
      
      //setup star transform
      Transform starTransform = star.transform;
      starTransform.parent = transform;

      starTransform.localPosition = star.position * starFieldScale;
      starTransform.localScale = Vector3.one * Mathf.Lerp(starSizeMin, starSizeMax, star.size);
      starTransform.LookAt(transform.position);
      starTransform.Rotate(0, 180, 0);
      
      //setup material
      MeshRenderer starRenderer = star.GetComponent<MeshRenderer>();
      Material material = starRenderer.sharedMaterial;
      material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, star.size));

      //save data
      starMeshRenderers.Add(starRenderer);
      stars.Add(star);
    }
  }
  }