using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{

    bool camVailable;
    WebCamTexture backCam;
    Texture defaultBackground;
    [SerializeField]
    RawImage displayImage;
    [SerializeField]
    AspectRatioFitter aspectRatioFitter;
    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = displayImage.texture;

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length < 1)
        {
            Debug.Log("No Camera available");
            camVailable = false;
            return;
        }

        for (int i = 0; i>devices.Length; i++) {

           if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name,Screen.width,Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("no cam");
            return;
        }
        backCam.Play();
        displayImage.texture = backCam;
        camVailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camVailable)
        {
            return;
        }

        float ratio = (float)backCam.width /(float) backCam.height;
        aspectRatioFitter.aspectRatio = ratio;
        float scaleY = backCam.videoVerticallyMirrored ? -1f: 1f;
        displayImage.rectTransform.localScale = new Vector3(1f,scaleY,1f);
        int orient = -backCam.videoRotationAngle;
        displayImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
    }
}
