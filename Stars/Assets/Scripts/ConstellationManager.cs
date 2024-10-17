using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ConstellationManager : MonoBehaviour
{
  [Header("Starfield Data")] 
  [SerializeField] private StarField _starField;

  [Header("DrawConstellations")] 
  [SerializeField] private VoidEventChannelSO drawConstellations;
  
  private List<Star> stars;
  private List<MeshRenderer> starMeshRenderers;
  private Dictionary<int, GameObject> constellationVisible = new();
  private bool constEnabled = false;
  
  void Start()
  {
    _starField = GetComponent<StarField>();
    stars = _starField.Stars;
    starMeshRenderers = _starField.StarMeshRenderers;
    CreateConstellations();
  }
  
  // A constellation is a tuple of the stars and the lines that join them.
  private readonly List<(int[], int[])> constellations = new() {
    // Orion
    (new int[] { 1948, 1903, 1852, 2004, 1713, 2061, 1790, 1907, 2124,
                 2199, 2135, 2047, 2159, 1543, 1544, 1570, 1552, 1567 },
     new int[] { 1713, 2004, 1713, 1852, 1852, 1790, 1852, 1903, 1903, 1948, 
                 1948, 2061, 1948, 2004, 1790, 1907, 1907, 2061, 2061, 2124, 
                 2124, 2199, 2199, 2135, 2199, 2159, 2159, 2047, 1790, 1543, 
                 1543, 1544, 1544, 1570, 1543, 1552, 1552, 1567, 2135, 2047 }),
    // Monceros
    (new int[] { 2970, 3188, 2714, 2356, 2227, 2506, 2298, 2385, 2456, 2479 },
     new int[] { 2970, 3188, 3188, 2714, 2714, 2356, 2356, 2227, 2714, 2506,
                 2506, 2298, 2298, 2385, 2385, 2456, 2479, 2506, 2479, 2385 }),
    // Gemini
    (new int[] { 2890, 2891, 2990, 2421, 2777, 2473, 2650, 2216, 2895,
                 2343, 2484, 2286, 2134, 2763, 2697, 2540, 2821, 2905, 2985},
     new int[] { 2890, 2697, 2990, 2905, 2697, 2473, 2905, 2777, 2777, 2650, 
                 2650, 2421, 2473, 2286, 2286, 2216, 2473, 2343, 2216, 2134, 
                 2763, 2484, 2763, 2777, 2697, 2540, 2697, 2821, 2821, 2905, 2905, 2985 }),
    // Cancer
    (new int[] {3475, 3449, 3461, 3572, 3249},
     new int[] {3475, 3449, 3449, 3461, 3461, 3572, 3461, 3249}),
    // Leo
    (new int[] { 3982, 4534, 4057, 4357, 3873, 4031, 4359, 3975, 4399, 4386, 3905, 3773, 3731 },
     new int[] { 4534, 4357, 4534, 4359, 4357, 4359, 4357, 4057, 4057, 4031, 
                 4057, 3975, 3975, 3982, 3975, 4359, 4359, 4399, 4399, 4386, 
                 4031, 3905, 3905, 3873, 3873, 3975, 3873, 3773, 3773, 3731, 3731, 3905 }),
    // Leo Minor
    (new int[] { 3800, 3974, 4100, 4247, 4090 },
     new int[] { 3800, 3974, 3974, 4100, 4100, 4247, 4247, 4090, 4090, 3974 }),
    // Lynx
    (new int[] { 3705, 3690, 3612, 3579, 3275, 2818, 2560, 2238 },
     new int[] { 3705, 3690, 3690, 3612, 3612, 3579, 3579, 3275, 3275, 2818, 
                 2818, 2560, 2560, 2238 }),
    // Ursa Major
    (new int[] { 3569, 3594, 3775, 3888, 3323, 3757, 4301, 4295, 4554, 4660, 
                 4905, 5054, 5191, 4518, 4335, 4069, 4033, 4377, 4375 },
     new int[] { 3569, 3594, 3594, 3775, 3775, 3888, 3888, 3323, 3323, 3757, 
                 3757, 3888, 3757, 4301, 4301, 4295, 4295, 3888, 4295, 4554, 
                 4554, 4660, 4660, 4301, 4660, 4905, 4905, 5054, 5054, 5191,
                 4554, 4518, 4518, 4335, 4335, 4069, 4069, 4033, 4518, 4377, 4377, 4375 }),
  };


  //enables if disables and the inverse
  public void EnableOrDisableConstellations()
  {
    if (!constEnabled)
    {
      EnableConstellations();
      constEnabled = true;
      return;
    }
    DisableConstellations();
    constEnabled = false;
  }
  
  //creates all the constellations from the list
  private void CreateConstellations()
  {
    for (int i = 0; i < constellations.Count; i++)
    {
      CreateConstellation(i);
      constellationVisible[i].SetActive(false);
    }
    
  }

  //enables constellations
  private void EnableConstellations()
  {
    for (int i = 0; i < constellationVisible.Count; i++)
    {
      constellationVisible[i].SetActive(true);
    }
  }
  
  //disables constellations
  public void DisableConstellations()
  {
    for (int i = 0; i < constellationVisible.Count; i++)
    {
      constellationVisible[i].SetActive(false);
    }
  }

  //creates a constellation (hardcoded)
  void CreateConstellation(int index) {
    int[] constellation = constellations[index].Item1;
    int[] lines = constellations[index].Item2;
    

    GameObject constellationHolder = new($"Constellation {index}");
    constellationHolder.transform.parent = transform;
    constellationVisible[index] = constellationHolder;
    
    // Change the colors of the stars
    foreach (int catalogNumber in constellation) {
      // Remember list is 0-up catalog numbers are 1-up.
      starMeshRenderers[catalogNumber - 1].material.color = Color.white;
      stars[catalogNumber -1].SetConstellation(constellationHolder);
    }
    

    // Draw the constellation lines.
    for (int i = 0; i < lines.Length; i += 2) {
      // Parent it to our constellation object so we can delete them all later.
      GameObject line = new("Line");
      line.transform.parent = constellationHolder.transform;
      
      LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

      lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
      // Disable useWorldSpace so it will track the parent game object.
      lineRenderer.useWorldSpace = false;
      Vector3 pos1 = stars[lines[i] - 1].transform.position;
      Vector3 pos2 = stars[lines[i + 1] - 1].transform.position;
      // Offset them so they don't occlude the stars, 3 chosen by trial and error.
      Vector3 dir = (pos2 - pos1).normalized * 3;
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, pos1 + dir);
      lineRenderer.SetPosition(1, pos2 - dir);
      lineRenderer.startWidth *= 3;
    }
  }
}
