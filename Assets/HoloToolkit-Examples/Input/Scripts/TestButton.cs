// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
namespace HoloToolkit.Unity.InputModule.Tests
{
    /// <summary>
    /// Test button that can be added to any object to make it gaze interactable and 
    /// receive pressed and released events.
    /// This class is an example of how an animated button can be created using the input module and Unity.
    /// </summary>
    public class TestButton : MonoBehaviour, IInputClickHandler, IFocusable
    {

        private AsyncOperation sceneAsync;
        public GameObject tranferGameObject;


        public Transform ToolTip;
        public Renderer ToolTipRenderer;

        private float toolTipTimer = 0.0f;
        public float ToolTipFadeTime = 0.25f;
        public float ToolTipDelayTime = 0.5f;

        [SerializeField]
        protected Animator ButtonAnimator;

        private int focusedButtonId;
        private int selectedButtonId;
        private int dehydrateButtonId;
        private int stayFocusedButtonId;
        private int colorPropertyId;

        public delegate void ActivateDelegate(TestButton source);
        public event ActivateDelegate Activated;

        public bool EnableActivation = true;

        private AnimatorControllerParameter[] animatorHashes;
        private Material cachedToolTipMaterial;
        private NetworkManager m_networkManager;

        private bool focused;
        public bool Focused
        {
            get { return focused; }
            set
            {
                if (focused != value)
                {
                    focused = value;
                    UpdateButtonAnimation();
                }
            }
        }

        private bool stayFocused;
        public bool StayFocused
        {
            get { return stayFocused; }
            set
            {
                if (stayFocused != value)
                {
                    stayFocused = value;
                    UpdateButtonAnimation();
                }
            }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    UpdateButtonAnimation();
                }
            }
        }

        protected virtual void Awake()
        {
            if (focusedButtonId == 0)
            {
                focusedButtonId = Animator.StringToHash("Focused");
            }

            if (selectedButtonId == 0)
            {
                selectedButtonId = Animator.StringToHash("Selected");
            }

            if (dehydrateButtonId == 0)
            {
                dehydrateButtonId = Animator.StringToHash("Dehydrate");
            }

            if (stayFocusedButtonId == 0)
            {
                stayFocusedButtonId = Animator.StringToHash("StayFocused");
            }

            if (colorPropertyId == 0)
            {
                colorPropertyId = Shader.PropertyToID("_Color");
            }
        }

        protected virtual void OnEnable()
        {
            Focused = false;

            // Set the initial alpha
            if (ToolTipRenderer != null)
            {
                cachedToolTipMaterial = ToolTipRenderer.material;

                Color tipColor = cachedToolTipMaterial.GetColor(colorPropertyId);
                tipColor.a = 0.0f;
                cachedToolTipMaterial.SetColor(colorPropertyId, tipColor);
                toolTipTimer = 0.0f;
            }

            UpdateVisuals();
            UpdateButtonAnimation();
        }

        private void Update()
        {
            if (ToolTipRenderer != null && (Focused && toolTipTimer < ToolTipFadeTime) || (!Focused && toolTipTimer > 0.0f))
            {
                // Calculate the new time delta
                toolTipTimer = toolTipTimer + (Focused ? Time.deltaTime : -Time.deltaTime);

                // Stop the timer if it exceeds the limit.  Clamp doesn't work here since time can be outside the normal range in some situations
                if (Focused && toolTipTimer > ToolTipFadeTime)
                {
                    toolTipTimer = ToolTipFadeTime;
                }
                else if (!Focused && toolTipTimer < 0.0f)
                {
                    toolTipTimer = 0.0f;
                }

                // Update the new opacity
                if (ToolTipRenderer != null)
                {
                    Color tipColor = cachedToolTipMaterial.GetColor(colorPropertyId);
                    tipColor.a = Mathf.Clamp(toolTipTimer, 0, ToolTipFadeTime) / ToolTipFadeTime;
                    cachedToolTipMaterial.SetColor(colorPropertyId, tipColor);
                }
            }
        }

        public void DehydrateButton()
        {
            if (ButtonAnimator != null && ButtonAnimator.isInitialized)
            {
                if (animatorHashes == null)
                {
                    animatorHashes = ButtonAnimator.parameters;
                }

                for (int i = 0; i < animatorHashes.Length; i++)
                {
                    if (animatorHashes[i].nameHash == dehydrateButtonId)
                    {
                        ButtonAnimator.SetTrigger(dehydrateButtonId);
                    }
                }
            }
        }

        // Child classes can override to update button visuals
        protected virtual void UpdateVisuals()
        {
        }

        private void UpdateButtonAnimation()
        {
            if (ButtonAnimator != null && ButtonAnimator.gameObject.activeInHierarchy)
            {
                if (animatorHashes == null)
                {
                    animatorHashes = ButtonAnimator.parameters;
                }

                for (int i = 0; i < animatorHashes.Length; i++)
                {
                    if (animatorHashes[i].nameHash == focusedButtonId)
                    {
                        ButtonAnimator.SetBool(focusedButtonId, Focused);
                    }

                    if (animatorHashes[i].nameHash == selectedButtonId)
                    {
                        ButtonAnimator.SetBool(selectedButtonId, Selected);
                    }

                    if (animatorHashes[i].nameHash == stayFocusedButtonId)
                    {
                        ButtonAnimator.SetBool(stayFocusedButtonId, StayFocused);
                    }
                }
            }
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (!EnableActivation)
            {
                return;
            }

            Selected = !Selected;

            if (Activated != null)
            {
                Activated(this);
            }

            if (gameObject.name == "button_play_pause")
            {
                GameObject button = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
                TextMesh button_text = button.GetComponent<TextMesh>();
                var dummy = PlayerPrefs.GetInt("play_pause_flag") == 1 ? (button_text.text = "Play") : (button_text.text = "Pause");
                PlayerPrefs.SetInt("play_pause_flag", PlayerPrefs.GetInt("play_pause_flag") == 1 ? 0 : 1);
            }

            if (gameObject.name == "button_back")
            {
                Scene m_Scene = SceneManager.GetActiveScene();
                Debug.Log("the scene here in menu is " + m_Scene.name);
                if (m_Scene.name != "Scene1")
                {
                    StartCoroutine(loadScene("testing/Scenes/Scene1"));

                }
            }

            if (gameObject.name == "button_share")
            {
                //m_networkManager = gameObject.GetComponent<NetworkManager>();
                //m_networkManager.StartHost();
            }

            eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
        }

        public void OnFocusEnter()
        {
            Focused = true;

            // The first time the button is focused and the timer hasn't started, start the timer in a delayed mode
            if (Focused && toolTipTimer == 0.0f)
            {
                toolTipTimer = -ToolTipDelayTime;
            }

            UpdateVisuals();
        }

        public void OnFocusExit()
        {
            Focused = false;

            UpdateVisuals();
        }

        private void OnDestroy()
        {
            DestroyImmediate(cachedToolTipMaterial);
        }


        IEnumerator loadScene(string index)
        {
            AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            scene.allowSceneActivation = false;
            sceneAsync = scene;

            //Wait until we are done loading the scene
            while (scene.progress < 0.9f)
            {
                //Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
                yield return null;
            }

            OnFinishedLoadingAllScene(index);
        }

        void enableScene(string index)
        {
            //Activate the Scene
            sceneAsync.allowSceneActivation = true;


            Scene sceneToLoad = SceneManager.GetSceneByName(index);
            if (sceneToLoad.IsValid())
            {
                Debug.Log("Scene is Valid");
                SceneManager.MoveGameObjectToScene(tranferGameObject, sceneToLoad);
                SceneManager.SetActiveScene(sceneToLoad);
            }
        }

        void OnFinishedLoadingAllScene(string index)
        {
            Debug.Log("Done Loading Scene");

            enableScene(index);
            Debug.Log("Scene Activated!");
        }
    }
}