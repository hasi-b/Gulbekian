using UnityEngine;
using TMPro; // Use this if you're using TextMeshPro

public class TextBounce : MonoBehaviour
{
    public float scaleAmount = 0.05f; // Amount to scale up and down
    public float bounceSpeed = 1.0f; // Speed of the bounce effect

    private Vector3 originalScale;
    private float time;

    void Start()
    {
        // Store the original scale of the text
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calculate the scale based on sine wave for smooth bouncing
        time += Time.deltaTime * bounceSpeed;
        float scale = 1 + Mathf.Sin(time) * scaleAmount;

        // Apply the new scale while keeping the original position
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
