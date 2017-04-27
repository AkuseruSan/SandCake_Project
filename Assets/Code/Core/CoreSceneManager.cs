using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoreSceneManager : MonoBehaviour {

    enum SceneID { MENU = 1, GAME = 2 }
    public GameObject loadingScreen;

    Scene currentScene;

    AsyncOperation currentAsyncOperation;

    enum State { ON_WAIT, LOADING, UNLOADING_MENU, UNLOADING_GAME, LOADED };
    State state;

    // Use this for initialization
    void Start ()
    {
        SetLoadingScreen(false);
        state = State.ON_WAIT;
        SwitchScene(SceneID.MENU);

        currentScene = SceneManager.GetActiveScene();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (state)
        {
            case State.ON_WAIT:
                {
                    //DEBUG
                    if (Input.GetKeyDown(KeyCode.G)) SwitchScene(SceneID.GAME);
                    if (Input.GetKeyDown(KeyCode.M)) SwitchScene(SceneID.MENU);
                }
                break;
            case State.LOADING:
                {
                    Debug.Log("Loading Progress: " + currentAsyncOperation.progress);

                    if (currentAsyncOperation.progress == 0.9f)
                    {
                        //ANIMATION
                        //
                        //
                        //ON ANIMATION ENDED

                        state = State.LOADED;
                        currentAsyncOperation.allowSceneActivation = true;
                    }
                }
                break;
            case State.UNLOADING_MENU:
                {
                    if (currentAsyncOperation == null || currentAsyncOperation.isDone)
                    {
                        state = State.LOADING;
                        currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.GAME, LoadSceneMode.Additive);
                        currentAsyncOperation.allowSceneActivation = false;
                        currentScene = SceneManager.GetSceneByBuildIndex(2);
                    }
                }
                break;
            case State.UNLOADING_GAME:
                {
                    if (currentAsyncOperation == null || currentAsyncOperation.isDone)
                    {
                        state = State.LOADING;
                        currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.MENU, LoadSceneMode.Additive);
                        currentAsyncOperation.allowSceneActivation = false;
                        currentScene = SceneManager.GetSceneByBuildIndex(1);
                    }
                }
                break;
            case State.LOADED:
                {
                    SetLoadingScreen(false);
                    state = State.ON_WAIT;
                }
                break;
            default:
                break;
        }

        Debug.Log("Scene Manager State: " + state);
    }
    
    void SwitchScene(SceneID sceneID)
    {
        SetLoadingScreen(true);

        switch (sceneID)
        {
            case SceneID.MENU:
                {
                    state = State.UNLOADING_GAME;
                    
                    currentAsyncOperation = SceneManager.UnloadSceneAsync((int)SceneID.GAME);
                    currentAsyncOperation.allowSceneActivation = false;
                }
                break;
            case SceneID.GAME:
                {
                    state = State.UNLOADING_MENU;

                    currentAsyncOperation = SceneManager.UnloadSceneAsync((int)SceneID.MENU);
                    currentAsyncOperation.allowSceneActivation = false;
                }
                break;
            default:
                break;
        }
    }

    void SetLoadingScreen(bool state)
    {
        if(state == true)
        {
            loadingScreen.SetActive(true);
        }
        else
        {
            loadingScreen.SetActive(false);
        }
    }
}
