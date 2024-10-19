using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionRequest : MonoBehaviour
{
  
    void Start()
    {
        // Check if the user has already granted the camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // If not, request the camera permission
            Permission.RequestUserPermission(Permission.Camera);
        }

        // Check if the user has already granted write external storage permission
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // If not, request the write external storage permission
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }
}


