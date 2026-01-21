using System;
using UnityEngine;

[ExecuteAlways]
public class CameraFacingBillboard : MonoBehaviour
{
    [SerializeField] bool selfFacing;
    [SerializeField] string cameraName = "";

    [NonSerialized] public new Camera camera;

    void OnEnable()
    {
        FindCamera();
    }

    void Update()
    {
        if (camera == null)
            FindCamera();

        if (camera != null)
        {
            if (selfFacing)
            {
                transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                                 camera.transform.rotation * Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(transform.parent.forward,
                                                             camera.transform.position - transform.position);
            }
        }
    }

    void FindCamera()
    {
        if (!string.IsNullOrEmpty(cameraName))
        {
            GameObject camObj = GameObject.Find(cameraName);
            if (camObj != null)
                camera = camObj.GetComponent<Camera>();
        }

        if (camera == null)
            camera = Camera.main;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Đảm bảo Update vẫn chạy trong Editor
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
    }
#endif
}