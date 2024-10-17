using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float catalogNumber;
    public double rightAscension;
    public double declination;
    public byte spectralType;
    public byte spectralIndex;
    public short magnitude;
    public float raProperMotion;
    public float decProperMotion;

    public float size;

    public Color color;

    public Vector3 position;

    private Renderer starRenderer;
    private MaterialPropertyBlock propertyBlock;

    private GameObject constellation;
    

    void Start()
    {
      ApplyMaterialPropertyBlock();
    }

    void ApplyMaterialPropertyBlock()
    {
      starRenderer = GetComponent<Renderer>();
      
      // Create a new MaterialPropertyBlock
      MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

      // Set the color property on the MaterialPropertyBlock
      propertyBlock.SetColor("_Color", color);

      // Apply the MaterialPropertyBlock to the renderer
      starRenderer.SetPropertyBlock(propertyBlock);
    }
    /// <summary>
    /// Setup the Star Class
    /// </summary>
    /// <param name="catalog_number"></param>
    /// <param name="right_ascension"></param>
    /// <param name="declination"></param>
    /// <param name="spectral_type"></param>
    /// <param name="spectral_index"></param>
    /// <param name="magnitude"></param>
    /// <param name="ra_proper_motion"></param>
    /// <param name="dec_proper_motion"></param>
    public void SetupValues(float catalog_number, double right_ascension, double declination, byte spectral_type,
      byte spectral_index, short magnitude, float ra_proper_motion, float dec_proper_motion) {
      catalogNumber = catalog_number;
      // Save the location parameters.
      rightAscension = right_ascension;
      this.declination = declination;
      raProperMotion = ra_proper_motion;
      decProperMotion = dec_proper_motion;
      name = $"HR {catalogNumber}";
      // Set the position
      position = GetBasePosition();
      // Set the Colour.
      color = SetColour(spectral_type, spectral_index);
      // Set the Size.
      size = 1.5f * SetSize(magnitude);
    }
    
    public Vector3 GetBasePosition() {
      // Place stars on a cylinder using 2D trigonometry.
      double x = System.Math.Cos(rightAscension);
      double y = System.Math.Sin(declination);
      double z = System.Math.Sin(rightAscension);

      // Pull in ends to make the sphere
      // Work out y-adjacent and use this to scale (as on unit sphere)
      double y_cos = System.Math.Cos(declination);
      x *= y_cos;
      z *= y_cos;

      // Return as float
      return new((float)x, (float)y, (float)z);
    }

    /// <summary>
    /// Sets the color of a star based on a spectral type and index
    /// </summary>
    /// <param name="spectral_type"></param>
    /// <param name="spectral_index"></param>
    /// <returns></returns>
    private Color SetColour(byte spectral_type, byte spectral_index) {
      Color IntColour(int r, int g, int b) {
        return new Color(r / 255f, g / 255f, b / 255f);
      }
      // OBAFGKM colours from: https://arxiv.org/pdf/2101.06254.pdf
      Color[] col = new Color[8];
      col[0] = IntColour(0x5c, 0x7c, 0xff); 
      col[1] = IntColour(0x5d, 0x7e, 0xff); 
      col[2] = IntColour(0x79, 0x96, 0xff); 
      col[3] = IntColour(0xb8, 0xc5, 0xff); 
      col[4] = IntColour(0xff, 0xef, 0xed); 
      col[5] = IntColour(0xff, 0xde, 0xc0); 
      col[6] = IntColour(0xff, 0xa2, 0x5a); 
      col[7] = IntColour(0xff, 0x7d, 0x24); 

      int col_idx = -1;
      if (spectral_type == 'O') {
        col_idx = 0;
      } else if (spectral_type == 'B') {
        col_idx = 1;
      } else if (spectral_type == 'A') {
        col_idx = 2;
      } else if (spectral_type == 'F') {
        col_idx = 3;
      } else if (spectral_type == 'G') {
        col_idx = 4;
      } else if (spectral_type == 'K') {
        col_idx = 5;
      } else if (spectral_type == 'M') {
        col_idx = 6;
      }

      // If unknown, make white.
      if (col_idx == -1) {
        return Color.white;
      }

      // Map second part 0 -> 0, 10 -> 100
      float percent = (spectral_index - 0x30) / 10.0f;
      return Color.Lerp(col[col_idx], col[col_idx + 1], percent);
    }

    private float SetSize(short magnitude) {
      // Linear isn't factually accurate, but the effect is sufficient.
      return 1 - Mathf.InverseLerp(-146, 796, magnitude);
    }

    public void SetConstellation(GameObject constellation)
    {
      this.constellation = constellation;
      constellation.SetActive(false);
    }

    public void ActivateConstellation()
    {
      if (!constellation) return;
      constellation.SetActive(true);
    }
    public void DeactivateConstellation()
    {
      if (!constellation) return;
      constellation.SetActive(false);
    }
}
