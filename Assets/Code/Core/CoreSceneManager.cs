using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoreSceneManager : MonoBehaviour {

    enum SceneID { MENU = 1, GAME = 2 }
    public GameObject loadingScreen;

    AsyncOperation currentAsyncOperation;

    enum State
    {
        ON_WAIT, LOADING_MENU, LOADING_GAME, UNLOADING_MENU, UNLOADING_GAME
    };
    State state = State.ON_WAIT;

    // Use this for initialization
    void Start ()
    {
        SwitchScene(SceneID.MENU);
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (state)
        {
            case State.ON_WAIT:
                {

                }
                break;
            case State.LOADING_MENU:
                {
                    if(currentAsyncOperation.isDone)
                    {
                        state = State.UNLOADING_GAME;
                        currentAsyncOperation = SceneManager.UnloadSceneAsync((int)SceneID.GAME);
                    }
                }
                break;
            case State.LOADING_GAME:
                {

                }
                break;
            case State.UNLOADING_MENU:
                {

                }
                break;
            case State.UNLOADING_GAME:
                {
                    if(currentAsyncOperation.isDone)
                    {
                        //SceneManager.SetActiveScene
                    }

                }
                break;
            default:
                break;
        }
        //switch (state)
        //{
        //    case State.NoSceneLoaded:
        //        break;
        //    case State.LoadingGame:
        //        if (currentAsyncOperation.isDone)
        //        {
        //            loadingScreen.SetActive(false);
        //            state = State.GameLoaded;
        //        }
        //        break;
        //    case State.GameLoaded:
        //        break;
        //    case State.UnloadingGame:
        //        if (currentAsyncOperation.isDone)
        //        {
        //            state = State.NoSceneLoaded;
        //            if (nextScene != null)
        //            {
        //                LoadNextScene();
        //            }
        //            else
        //        }
        //        break;
        //    case State.LoadingMenu:
        //        break;
        //    case State.MenuLoaded:
        //        break;
        //    case State.UnloadingMenu:
        //        break;
        //    default:
        //        break;
        //}
    }
    
    void SwitchScene(SceneID sceneId)
    {
        switch (sceneId)
        {
            case SceneID.MENU:
                {
                    state = State.LOADING_MENU;
                    //loadingScreen.SetActive(true);
                    currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.MENU, LoadSceneMode.Additive);
                    currentAsyncOperation.allowSceneActivation = false;
                }
                break;
            case SceneID.GAME:
                {
                    state = State.LOADING_GAME;
                    //loadingScreen.SetActive(true);
                    currentAsyncOperation = SceneManager.LoadSceneAsync((int)SceneID.GAME, LoadSceneMode.Additive);
                    currentAsyncOperation.allowSceneActivation = false;
                }
                break;
            default:
                break;
        }
    }
}
