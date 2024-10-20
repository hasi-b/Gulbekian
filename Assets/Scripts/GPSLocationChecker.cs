using UnityEngine;
using System.Collections;
using UnityEngine.Android;
using TMPro;

public class GPSLocationChecker : MonoBehaviour
{
    public static GPSLocationChecker instance;
    // Desired radius in meters
    public float detectionRadius = 20f;
    [SerializeField]
    TextMeshProUGUI location;
    // Target location (real-world GPS coordinates)
    public double targetLatitude = 40.7128;  // Example: Latitude of New York City
    public double targetLongitude = -74.0060;  // Example: Longitude of New York City
    float playerLatitude;
    float playerLongitude;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Request location permissions
        StartCoroutine(RequestLocationPermission());
    }

    // Coroutine to request location permissions and start the location service
    IEnumerator RequestLocationPermission()
    {
        // For Android
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // Request permission
            Permission.RequestUserPermission(Permission.FineLocation);

            // Wait for user response
            yield return new WaitForSeconds(1f);

            // Check again if the permission was granted
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Debug.LogError("Location permission denied.");
                yield break;
            }
        }
#endif

        // Start the GPS location service
        yield return StartCoroutine(StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        // Check if the user has enabled GPS
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled.");
            yield break;
        }

        // Start the service
        Input.location.Start();

        // Wait until the service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait <= 0)
        {
            Debug.LogError("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }

        // Service is working
        Debug.Log("Location services started successfully");
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure GPS service is running and available
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the player's current GPS location
             playerLatitude = Input.location.lastData.latitude;
             playerLongitude = Input.location.lastData.longitude;
            location.SetText(" lat"+playerLatitude + " long " + playerLongitude);
            // Check if the player is within the radius of the target location
            if (IsPlayerWithinRadius( targetLatitude, targetLongitude, detectionRadius))
            {
                Debug.Log("You are here");
            }
        }
    }

    // Function to calculate distance between two GPS coordinates (Haversine formula)
    public bool IsPlayerWithinRadius(double targetLat, double targetLon, float radiusInMeters)
    {
        // Earth radius in meters
        const double R = 6371000;

        // Convert degrees to radians
        double lat1Rad = playerLatitude * Mathf.Deg2Rad;
        double lat2Rad = targetLat * Mathf.Deg2Rad;
        double deltaLat = (targetLat - playerLatitude) * Mathf.Deg2Rad;
        double deltaLon = (targetLon - playerLongitude) * Mathf.Deg2Rad;

        // Haversine formula
        double a = Mathf.Sin((float)deltaLat / 2) * Mathf.Sin((float)deltaLat / 2) +
                   Mathf.Cos((float)lat1Rad) * Mathf.Cos((float)lat2Rad) *
                   Mathf.Sin((float)deltaLon / 2) * Mathf.Sin((float)deltaLon / 2);
        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt(1 - (float)a));

        // Distance between the two points
        double distance = R * c;

        // Return true if within the radius
        return distance <= radiusInMeters;
    }
}
