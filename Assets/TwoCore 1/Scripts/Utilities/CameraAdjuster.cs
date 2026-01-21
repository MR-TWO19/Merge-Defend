using UnityEditor;
using UnityEngine;

namespace TwoCore
{
    [RequireComponent(typeof(Camera))]
    public class CameraAdjuster : MonoBehaviour
    {
        [SerializeField] private float baseWidth = 750f;
        [SerializeField] private float baseHeight = 1334f;
        [SerializeField] private float baseOrthoSize = 5f;
        [SerializeField] private float baseFOV = 60f;

        private Camera cam;
        private int lastScreenWidth;
        private int lastScreenHeight;

        void Start()
        {
            cam = GetComponent<Camera>();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            AdjustCamera();
        }

        void Update()
        {
            if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
            {
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                AdjustCamera();
            }
        }

        void AdjustCamera()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float targetAspect = baseWidth / baseHeight;

            if (cam.orthographic)
            {
                if (screenAspect >= targetAspect)
                {
                    cam.orthographicSize = baseOrthoSize;
                }
                else
                {
                    cam.orthographicSize = baseOrthoSize * (targetAspect / screenAspect);
                }
            }
            else
            {
                if (screenAspect >= targetAspect)
                {
                    cam.fieldOfView = baseFOV;
                }
                else
                {
                    float scale = targetAspect / screenAspect;
                    cam.fieldOfView = baseFOV * scale;
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CameraAdjuster))]
    public class CameraAdjusterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var script = (CameraAdjuster)target;
            var cam = script.GetComponent<Camera>();

            EditorGUILayout.LabelField("Size Scene", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("baseWidth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("baseHeight"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Parameter Camera", EditorStyles.boldLabel);

            if (cam.orthographic)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("baseOrthoSize"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("baseFOV"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}