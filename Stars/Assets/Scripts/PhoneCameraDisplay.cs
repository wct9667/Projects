using System;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCameraDisplay : MonoBehaviour
{
  private bool camAvailable;
  private WebCamTexture backCam;
  private Texture defaultBackground;

  public RawImage background;
  public AspectRatioFitter fit;

  private void Start()
  {
    //fallback background
    defaultBackground = background.texture;
    
    //look for cameras
    WebCamDevice[] devices = WebCamTexture.devices;
    
    //no camera
    if (devices.Length == 0)
    {
      Debug.Log("No Camera detected");
      camAvailable = false;
      return;
    }
    
    //cause of multiple cameras
    for (int i = 0; i < devices.Length; i++)
    {
      if (devices[i].isFrontFacing) continue;
      
      backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
    }

    if (backCam == null) {Debug.Log("no phone"); return;}
    
    backCam.Play();
    background.texture = backCam;

    camAvailable = true;
    

  }

  private void Update()
  {
    if (!camAvailable) return;

    //get aspect ratio
    float ratio = (float)backCam.width / (float)backCam.height;
    fit.aspectRatio = ratio;
    
    //set scale of texture
    float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
    background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

    //orientation of the video
    int orient = -backCam.videoRotationAngle;
    background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

  }
}
