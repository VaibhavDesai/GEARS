using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMyScene : MonoBehaviour {

    
    public GameObject tranferGameObject;
    private AsyncOperation sceneAsync;

    void Start()
    {
        StartCoroutine(loadScene("testing/Scenes/Scene1"));
    }

    IEnumerator loadScene(string index)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        //Wait until we are done loading the scene
        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
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

