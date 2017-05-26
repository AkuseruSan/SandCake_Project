using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CoreSceneManager : MonoBehaviour {

    public enum SceneID { MASTER = 0, MENU = 1, GAME = 2 }

    public static CoreSceneManager Instance { get; private set; }

    public GameObject loadingScreen;

    public Scene currentScene;

    AsyncOperation currentAsyncOperation;

    enum State { ON_WAIT, LOADING, UNLOADING_MENU, UNLOADING_GAME, LOADED };
    State state;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        SetLoadingScreen(false);
        state = State.ON_WAIT;

        currentAsyncOperation = null;

        currentScene = SceneManager.GetActiveScene();

        SwitchScene(SceneID.MENU);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.ON_WAIT:
                {
                    if (DataManager.Instance.godMode)
                    {
                        //DEBUG
                        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchScene(SceneID.GAME);
                        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchScene(SceneID.MENU);
                    }
                }
                break;
            case State.LOADING:
                {
                    if(currentAsyncOperation.progress == 0.9f)
                    {
                        if (true)//End animation condition.
                        {
                            currentAsyncOperation.allowSceneActivation = true;

                            state = State.LOADED;
                        }
                    }
                }
                break;
            case State.UNLOADING_MENU:
                {
                    if (currentAsyncOperation.progress == 1)
                    {

                        currentAsyncOperation.allowSceneActivation = true;

                        StartCoroutine(LoadScene(SceneID.GAME));

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.GAME);

                        state = State.LOADING;
                        
                    }
                }
                break;
            case State.UNLOADING_GAME:
                {
                    if(currentAsyncOperation.progress == 1)
                    {

                        currentAsyncOperation.allowSceneActivation = true;

                        StartCoroutine(LoadScene(SceneID.MENU));

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.MENU);

                        state = State.LOADING;
                        
                    }
                }
                break;
            case State.LOADED:
                {
                    currentAsyncOperation = null;

                    SceneManager.SetActiveScene(currentScene);

                    SetLoadingScreen(false);
                    state = State.ON_WAIT;
                }
                break;
            default:
                {

                }
                break;
        }

        //Debug.Log("Core Scene Manager State: " + state);
    }

    public void SwitchScene(SceneID sceneID)
    {
        switch (sceneID)
        {
            case SceneID.MASTER:
                {
                    //Debug.Log("[Master Scene can not be reloaded]");
                }
                break;
            case SceneID.MENU:
                {
                    if(currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.GAME))
                    {
                        SetLoadingScreen(true);

                        StartCoroutine(UnloadScene(SceneID.GAME));

                        state = State.UNLOADING_GAME;
                    }
                    else if(currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MASTER))
                    {
                        SetLoadingScreen(true);

                        StartCoroutine(LoadScene(SceneID.MENU));

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.MENU);

                        state = State.LOADING;
                    }
                    else Debug.Log("[INVALID SCENE TO SWITCH TO]");
                }
                break;
            case SceneID.GAME:
                {
                    if (currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MENU))
                    {
                        SetLoadingScreen(true);

                        StartCoroutine(UnloadScene(SceneID.MENU));

                        state = State.UNLOADING_MENU;
                    }
                    else if (currentScene == SceneManager.GetSceneByBuildIndex((int)SceneID.MASTER))
                    {
                        SetLoadingScreen(true);

                        StartCoroutine(LoadScene(SceneID.GAME));

                        currentScene = SceneManager.GetSceneByBuildIndex((int)SceneID.GAME);

                        state = State.LOADING;
                    }
                    else Debug.Log("[INVALID SCENE TO SWITCH TO]");
                }
                break;
            default:
                {

                }
                break;
        }
    }

    private IEnumerator LoadScene(SceneID id)
    {
        currentAsyncOperation = SceneManager.LoadSceneAsync((int)id, LoadSceneMode.Additive);
        currentAsyncOperation.allowSceneActivation = false;

        yield return currentAsyncOperation;
    }

    private IEnumerator UnloadScene(SceneID id)
    {
        currentAsyncOperation = SceneManager.UnloadSceneAsync((int)id);

        yield return currentAsyncOperation;
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
