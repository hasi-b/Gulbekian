using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgaeComparison : MonoBehaviour
{
    public Texture2D referenceImage;
    [SerializeField]
    private Texture2D capturedImage;
    public Camera gameCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Capture the picture from the game camera
            //capturedImage = CaptureImageFromCamera(gameCamera);

            // Compare using SSIM
            float ssimValue = CalculateSSIM(referenceImage, capturedImage);

            if (ssimValue > 0.5f)
            {
                Debug.Log("Yes - Images are similar (SSIM > 0.5)");
            }
            else
            {
                Debug.Log("No - Images are not similar (SSIM <= 0.5)");
            }
        }
    }

    Texture2D CaptureImageFromCamera(Camera cam)
    {
        RenderTexture renderTex = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = renderTex;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = renderTex;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTex);
        return screenshot;
    }

    float CalculateSSIM(Texture2D img1, Texture2D img2)
    {
        // Ensure images are the same size
        if (img1.width != img2.width || img1.height != img2.height)
        {
            Debug.Log("Images are not the same size");
            return 0;
        }

        float meanX = 0, meanY = 0, varianceX = 0, varianceY = 0, covarianceXY = 0;
        int width = img1.width, height = img1.height;
        float C1 = 0.01f * 0.01f; // Small constant to stabilize the division
        float C2 = 0.03f * 0.03f;

        // Loop through pixels
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float pixelX = img1.GetPixel(x, y).grayscale;
                float pixelY = img2.GetPixel(x, y).grayscale;

                // Means
                meanX += pixelX;
                meanY += pixelY;

                // Variances and covariance
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

        // SSIM formula
        float numerator = (2 * meanX * meanY + C1) * (2 * covarianceXY + C2);
        float denominator = (meanX * meanX + meanY * meanY + C1) * (varianceX + varianceY + C2);

        return numerator / denominator;
    }
}
