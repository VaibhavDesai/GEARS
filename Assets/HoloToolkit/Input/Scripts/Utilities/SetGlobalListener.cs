// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// Register this game object on the InputManager as a global listener.
    /// </summary>
    public class SetGlobalListener : MonoBehaviour
    {
        void Start()
        {
            Scene m_Scene = SceneManager.GetActiveScene();
            if (m_Scene.name.Equals("Scene1"))
            {
                var transferedGameObject = GameObject.Find("MixedRealityCamera");
                var CameraParentGameObject = GameObject.Find("MixedRealityCameraParent");
                transferedGameObject.transform.parent = CameraParentGameObject.transform;
            }

        }

        private void OnEnable()
        {
            InputManager.Instance.AddGlobalListener(gameObject);
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.RemoveGlobalListener(gameObject);
            }
        }
    }
}
