using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class DeviceCameraControl : MonoBehaviour
    {
    public static DeviceCameraControl Instance { get; private set; }
        //public Texture2D[] referenceImage;    // Reference image to compare against
        public RawImage cameraFeed;         // UI element to display the camera feed
        public TextMeshProUGUI  resultText;             // Text element to show comparison results

        private WebCamTexture webCamTexture;
    // To access the device's camera
    [SerializeField]
        private Texture2D capturedImage;      // Captured image from the camera
        private bool isCameraRunning = false; // To track if the camera is running
        [SerializeField]    
        TMP_InputField inputText;
        [SerializeField]
        AspectRatioFitter aspectRatioFitter;
        [SerializeField]
        float threshold = 0.5f;
    [SerializeField]
    Texture2D dummyImg;
        [SerializeField]
        GameObject LinearCameraCapturebutton;
        [SerializeField]
        GameObject LinearCheckPIcturebutton;
        [SerializeField]
        GameObject restartScene;
        [SerializeField]
        GameObject camButton;
    [Space(10)] // Adds 10 units of vertical space in the Inspector (you can adjust this value)
    [Header("UI Elements")]
    [SerializeField]
    GameObject gameBGPanel;
    [SerializeField]
    GameObject gameCameraPanel;
    [SerializeField]
    GameObject captureButton;
    [SerializeField]
    GameObject submitButton;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
        {
            // Key control: Press 'Y' to start the camera
            if (Input.GetKeyDown(KeyCode.Y) && !isCameraRunning)
            {
                StartCamera();
            }

            // Key control: Press 'U' to capture an image
            if (Input.GetKeyDown(KeyCode.U) && isCameraRunning)
            {
                CaptureImage();
                
            }

            if(Input.GetKeyDown(KeyCode.Space) && isCameraRunning)
        {
            //CaptureReferenceImage();
            
        }

            // Key control: Press 'I' to compare the captured image with the reference image
            if (Input.GetKeyDown(KeyCode.I) && capturedImage != null)
            {
            
            //CompareCapturedImage();
            }

            // Key control: Press 'R' to stop the camera
            if (Input.GetKeyDown(KeyCode.R) && isCameraRunning)
            {
                StopCamera();
            }

        
            if (!isCameraRunning)
            {
                return;
            }

            float ratio = (float)webCamTexture.width / (float)webCamTexture.height;
            aspectRatioFitter.aspectRatio = ratio;
            float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
            cameraFeed.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
            int orient = -webCamTexture.videoRotationAngle;
            cameraFeed.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
        
    }

        // Method to start the device camera
        public void StartCamera()
        {
        cameraFeed.texture = null;
            if (WebCamTexture.devices.Length > 0)
            {
                webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name,1080, 1920);
                cameraFeed.texture = webCamTexture;
                webCamTexture.Play();
                isCameraRunning = true;
                
            //resultText.SetText  ("Camera started!");
                Debug.Log("Camera started!");
            }
            else
            {
                Debug.LogError("No device camera found");
            resultText.SetText("No device camera found");
            
            }
        }

    public void setThreshold()
    {

        threshold =float.Parse( inputText.text);
    }
        // Method to capture the current frame from the camera feed
        //public void CaptureReferenceImage()
        //{
        //    if (webCamTexture != null && webCamTexture.isPlaying)
        //    {
        //    // Capture the current frame from the camera feed
        //    referenceImage[0] = new Texture2D(webCamTexture.width, webCamTexture.height);
        //    referenceImage[0].SetPixels(webCamTexture.GetPixels());
        //        referenceImage[0].Apply();
        //    resultText.SetText("Image captured!");
           
        //        Debug.Log("Image captured!");
        //    StopCamera();
        //}
        //    else
        //    {
        //        Debug.LogWarning("Camera is not running");
        //    resultText.SetText("Camera is not running");
            
        //    }
        //}

    public void CaptureImage ()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            // Capture the current frame from the camera feed
            capturedImage = new Texture2D(webCamTexture.width, webCamTexture.height);
            capturedImage.SetPixels(webCamTexture.GetPixels());
            capturedImage.Apply();
            cameraFeed.texture = capturedImage;
            resultText.SetText("Image captured!");
            StopCamera();
            Debug.Log("Image captured!");
            PuzzleCapturedPhoto();
        }
        else
        {
            Debug.LogWarning("Camera is not running");
            resultText.SetText("Camera is not running");

        }
    }

    // Method to compare the captured image with the reference image
   public bool CompareCapturedImage(List<Texture2D> referenceImages)
        {
        bool isSame = false;
        Debug.Log("Checking started");
            if (capturedImage == null)
            {
                Debug.LogWarning("No image captured yet");
                resultText.text = "No image captured yet";
                return false;
            }

           // capturedImage = dummyImg;
        for (int i = 0; i < referenceImages.Count; i++)
        {
            Texture2D resizedCapturedImage = ResizeTexture(capturedImage, referenceImages[i].width, referenceImages[i].height);

            // Compare the captured image with the reference image using SSIM
            float ssimValue = CalculateSSIM(referenceImages[i], resizedCapturedImage);
            Debug.Log("value : " + ssimValue);
            // Display the result
            if (ssimValue >= threshold)
            {
                isSame = true;

            }
        }
            

        
        // Resize the captured image to match the reference image size
        if (isSame)
        {
            Debug.Log("Yes - Images are similar (SSIM > " + threshold + " )");
            resultText.SetText("Yes - Images are similar (SSIM > " + threshold + " )");
        }
        else
        {
            resultText.SetText("No - Images are not similar (SSIM <=" + threshold + ")");
            Debug.Log("No - Images are not similar (SSIM <=" + threshold + ")");
        }
            return isSame;
        }

        // Method to stop the camera
        public void StopCamera()
        {
            if (webCamTexture != null && webCamTexture.isPlaying)
            {
                webCamTexture.Stop();
                isCameraRunning = false;
                resultText.SetText( "Camera stopped!");
                Debug.Log("Camera stopped!");
            }
        }

        // Resize the captured image to match the reference image dimensions
        Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
            Graphics.Blit(source, rt);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D result = new Texture2D(targetWidth, targetHeight);
            result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            result.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);

            return result;
        }

        // SSIM calculation (same as before)
        float CalculateSSIM(Texture2D img1, Texture2D img2)
        {
            if (img1.width != img2.width || img1.height != img2.height)
            {
                Debug.Log("Images are not the same size");
                return 0;
            }
            
            float meanX = 0, meanY = 0, varianceX = 0, varianceY = 0, covarianceXY = 0;
            int width = img1.width, height = img1.height;
            float C1 = 0.01f * 0.01f;
            float C2 = 0.03f * 0.03f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float pixelX = img1.GetPixel(x, y).grayscale;
                    float pixelY = img2.GetPixel(x, y).grayscale;

                    meanX += pixelX;
                    meanY += pixelY;

                    varianceX += pixelX * pixelX;
                    varianceY += pixelY * pixelY;
                    covarianceXY += pixelX * pixelY;
                }
            }

            meanX /= (width * height);
            meanY /= (width * height);
            varianceX = varianceX / (width * height) - meanX * meanX;
            varianceY = varianceY / (width * height) - meanY * meanY;
            covarianceXY = covarianceXY / (width * height) - meanX * meanY;

            float numerator = (2 * meanX * meanY + C1) * (2 * covarianceXY + C2);
            float denominator = (meanX * meanX + meanY * meanY + C1) * (varianceX + varianceY + C2);

            return numerator / denominator;
        }




        public void StartCamInLinearMode()
        {
            StartCamera();
            LinearCameraCapturebutton.SetActive(true);
            camButton.SetActive(false);
        }

        public void capturePhoto()
        {
            CaptureImage();
            LinearCameraCapturebutton.SetActive(false);
            LinearCheckPIcturebutton.SetActive(true);
        }

        public void PuzzleStartCamera()
        {
            gameBGPanel.SetActive(false);
            gameCameraPanel.SetActive(true);
            StartCamera();

        }

        public void PuzzleNextLevel()
        {
            gameBGPanel.SetActive(true);
            gameCameraPanel.SetActive(false);
            captureButton.SetActive(true);
            submitButton.SetActive(false);
    }

        public void PuzzleCapturedPhoto()
        {
            captureButton.SetActive(false);
            submitButton.SetActive(true);
        }

    }


