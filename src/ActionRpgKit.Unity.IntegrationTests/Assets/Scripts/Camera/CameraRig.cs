using UnityEngine;

/// <summary>
/// Keep the Camera focused on the Player.</summary>
[ExecuteInEditMode]
public class CameraRig : MonoBehaviour
{
    /// <summary>
    /// Static reference, so the CameraRig is kept unique.</summary>
    public static CameraRig _cameraRig;

    /// <summary>
    /// The camera.</summary>
    public Camera _camera;

    /// <summary>
    /// The target for the camera to look at.</summary>
    public Transform _target;

    //--------------------------------------------------------------------------
    // mono methods
    //--------------------------------------------------------------------------

    /// <summary>
    /// Keep the CameraRig a singleton.</summary>
    void Awake()
    {
        if (_cameraRig == null)
        {
            _cameraRig = this;
        }
        else if (_cameraRig != null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Aim at the target and process the mouse wheel zoom in and out.</summary>
    void Update()
    {
        if (_target == null)
        {
            return;
        }
        transform.position = _target.position;
        _camera.transform.LookAt(_target);
    }
}
