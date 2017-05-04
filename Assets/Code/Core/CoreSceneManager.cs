using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoreSceneManager : MonoBehaviour {

    enum SceneID { MASTER = 0, MENU = 1, GAME = 2 }
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

        currentScene = SceneManager.GetActiveScene();
        SwitchScene(SceneID.MENU);
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
                    if (Input.GetKeyDown(KeyCode.H)) SwitchScene(SceneID.MENU);
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

                        if (true)
                        {

                            currentAsyncOperation.allowSceneActivation = true;
                            Debug.Log("Current Active Scene: " + SceneManager.SetActiveScene(currentScene));

                            state = State.LOADED;
                        }
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


        switch (sceneID)
        {
            case SceneID.MENU:
                {
                    if (currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.GAME))
                    {
                        SetLoadingScreen(true);

                        currentAsyncOperation = SceneManager.UnloadSceneAsync(currentScene);
                        currentAsyncOperation.allowSceneActivation = false;

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.MENU);

                        state = State.UNLOADING_GAME;
                    }
                    else if(currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MASTER))
                    {
                        SetLoadingScreen(true);

                        currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.MENU, LoadSceneMode.Additive);
                        currentAsyncOperation.allowSceneActivation = false;

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.MENU);

                        state = State.LOADING;
                    }
                }
                break;
            case SceneID.GAME:
                {
                    if (currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MENU))
                    {
                        SetLoadingScreen(true);

                        currentAsyncOperation = SceneManager.UnloadSceneAsync(currentScene);
                        currentAsyncOperation.allowSceneActivation = false;

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.GAME);

                        state = State.UNLOADING_MENU;
                    }
                    else if (currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MASTER))
                    {
                        SetLoadingScreen(true);

                        currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.GAME, LoadSceneMode.Additive);
                        currentAsyncOperation.allowSceneActivation = false;

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.GAME);

                        state = State.LOADING;
                    }
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
