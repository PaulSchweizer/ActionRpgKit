using UnityEngine;

/// <summary>
/// Keep the Camera focused on the Player.</summary>
[ExecuteInEditMode]
public class CameraRig : MonoBehaviour
{
    /// <summary>
    /// Static reference, so the CameraRig is kept unique.</summary>
    public static CameraRig Instance;

    /// <summary>
    /// The camera.</summary>
    public Camera Camera;

    /// <summary>
    /// The target for the camera to look at.</summary>
    public Transform Target;

    /// <summary>
    /// Keep the CameraRig a singleton.</summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Aim at the target and process the mouse wheel zoom in and out.</summary>
    public void Update()
    {
        if (Target == null)
        {
            return;
        }
        transform.position = Target.position;
        Camera.transform.LookAt(Target);
    }
}
