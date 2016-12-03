using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Class only called by UI elements events.
public class InputManagerUI : MonoBehaviour {


    public static InputManagerUI Instance {get; private set;}

    [Header("UI Transforms")]
    public Transform mainMenuPanel;
    public Transform score;

    private Animator menuAnimator;

    void Awake()
    {
        if( Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        menuAnimator = mainMenuPanel.GetComponent<Animator>();
    }

    void Update()
    {
        score.GetComponent<Text>().text = "DISTANCE: " + GameCore.Instance.playerController.distanceSinceStart;
    }

    public void StartGame()
    {
        menuAnimator.SetTrigger("exit");
        GameCore.Instance.gameState = GameState.PLAY;
    }

	public void JumpButton()
    {
        GameCore.Instance.playerController.Jump();
    }

    public void TogglePauseGame()
    {
        if(GameCore.Instance.gameState == GameState.PLAY)
        {
            GameCore.Instance.gameState = GameState.PAUSE;
        }
        else if(GameCore.Instance.gameState == GameState.PAUSE)
        {
            GameCore.Instance.gameState = GameState.PLAY;
        }
    }
}
