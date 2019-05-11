using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModPlayerSDK
{
    public enum CameraType
    {
        ThirdPerson,
        Scene
    }
    [ExecuteInEditMode]
    public class SceneSetup : MonoBehaviour
    {
        public static SceneSetup instance;

        public CameraType cameraType = CameraType.ThirdPerson;

        private void Awake()
        {
            if (Application.isPlaying)
                instance = this;

#if UNITY_EDITOR
            gameObject.name = "SCENE_SETUP";
            transform.SetAsFirstSibling();
#endif
        }
    }
}