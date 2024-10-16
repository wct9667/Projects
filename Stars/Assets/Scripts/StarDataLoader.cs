using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct StarData
{
  public readonly float catalogNumber;
  public readonly double rightAscension;
  public readonly double declination;
  public readonly byte spectralType;
  public readonly byte spectralIndex;
  public readonly short magnitude;
  public readonly float raProperMotion;
  public readonly float decProperMotion;

  // Constructor (optional)
  public StarData(float catalogNumber, 
    double rightAscension, 
    double declination, 
    byte spectralType, 
    byte spectralIndex, 
    short magnitude, 
    float raProperMotion, 
    float decProperMotion)
  {
    this.catalogNumber = catalogNumber;
    this.rightAscension = rightAscension;
    this.declination = declination;
    this.spectralType = spectralType;
    this.spectralIndex = spectralIndex;
    this.magnitude = magnitude;
    this.raProperMotion = raProperMotion;
    this.decProperMotion = decProperMotion;
  }
}
public class StarDataLoader {
  
  public List<StarData> LoadData() {
    List<StarData> stars = new();
    // Open the binary file for reading.
    const string filename = "BSC5";
    TextAsset textAsset = Resources.Load(filename) as TextAsset;
    MemoryStream stream = new(textAsset.bytes);
    BinaryReader br = new(stream);
    // Read the header
    int sequence_offset = br.ReadInt32();
    int start_index = br.ReadInt32();
    int num_stars = -br.ReadInt32();
    int star_number_settings = br.ReadInt32();
    int proper_motion_included = br.ReadInt32();
    int num_magnitudes = br.ReadInt32();
    int star_data_size = br.ReadInt32();

    // Read one field at a time.
    for (int i = 0; i < num_stars; i++) {
      float catalogNumber = br.ReadSingle();
      double rightAscension = br.ReadDouble();
      // Angular distance from celestial equator.
      double declination = br.ReadDouble();
      byte spectralType = br.ReadByte();
      byte spectralIndex = br.ReadByte();
      short magnitude = br.ReadInt16();
      float raProperMotion = br.ReadSingle();
      float decProperMotion = br.ReadSingle();
      StarData star = new(catalogNumber, rightAscension, declination, spectralType, spectralIndex, magnitude, raProperMotion, decProperMotion);
      stars.Add(star);
    }

    return stars;
  }

}