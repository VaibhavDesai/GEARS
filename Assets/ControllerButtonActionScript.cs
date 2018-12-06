using UnityEngine;
using HoloToolkit.Unity.InputModule;

using UnityEngine.XR.WSA.Input;
using UnityEngine.SceneManagement;

using System.Collections;
using HoloToolkit.Unity.InputModule.Tests;

public class ControllerButtonActionScript : AttachToController
{
    bool isPressed = false;
    public GameObject tranferGameObject;

    private AsyncOperation sceneAsync;

    [SerializeField]
    private PopupMenu popupMenu = null;


    protected override void OnAttachToController()
    {
        // Subscribe to input now that we're parented under the controller
        InteractionManager.InteractionSourcePressed += InteractionSourcePressed;
        InteractionManager.InteractionSourcePressed += ShowPopup;
    }

    protected override void OnDetachFromController()
    {
        // Unsubscribe from input now that we've detached from the controller
        InteractionManager.InteractionSourcePressed -= InteractionSourcePressed;
        InteractionManager.InteractionSourcePressed -= ShowPopup;
    }

    private void InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {

        Debug.Log("isPressed " + obj.pressType);

        if (obj.pressType == InteractionSourcePressType.Touchpad)
        {
            isPressed = true;
            if (obj.state.source.handedness == InteractionSourceHandedness.Left)
            {
                Debug.Log("inside the last if");
                //ShowPopup();

            }
            /*
            isPressed = true;
            if (obj.state.source.handedness == InteractionSourceHandedness.Left)
            {

                Scene m_Scene = SceneManager.GetActiveScene();
                Debug.Log("the scene here is " + m_Scene.name);

                if (m_Scene.name != "Scene1")
                {
                    //StartCoroutine(loadScene("testing/Scenes/Scene1"));

                }
            }
            */
        }

        if (obj.pressType == InteractionSourcePressType.Menu && !isPressed)
        {
            isPressed = true;

            // If both are true, Visible is set to true and the touchpad position is assigned to selectorPosition. 

            Scene m_Scene = SceneManager.GetActiveScene();
            Debug.Log("the scene here in menu is " + m_Scene.name);
            if (m_Scene.name != "Scene1")
            {
                StartCoroutine(loadScene("testing/Scenes/Scene1"));

            }
        }


        if (obj.pressType == InteractionSourcePressType.Select)
        {
            
        }


    }

    private void ShowPopup(InteractionSourcePressedEventArgs obj)
    {

        if (obj.pressType == InteractionSourcePressType.Touchpad)
        {
            Debug.Log("inside show pop up");
            if (popupMenu != null)
            {
                Debug.Log("In show popup Menu");
                if (popupMenu.CurrentPopupState == PopupMenu.PopupState.Closed)
                {
                    popupMenu.Show();

                    StartCoroutine(WaitForPopupToClose());
                }
            }
            else { 
            Debug.Log("pop up Menu is not null");
            }
        }
    }

    private IEnumerator WaitForPopupToClose()
    {
        if (popupMenu)
        {
            while (popupMenu.CurrentPopupState == PopupMenu.PopupState.Open)
            {

                yield return null;
            }

            Debug.Log("jkhkll");
        }


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
            isPressed = false;
        }
    }

    void OnFinishedLoadingAllScene(string index)
    {
        Debug.Log("Done Loading Scene");

        enableScene(index);
        Debug.Log("Scene Activated!");
    }

}
