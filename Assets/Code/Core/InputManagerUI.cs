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
    public Transform optionsAwake;
    public Transform staminaBarValue;

    public Slider volume;
    public AudioSource gameMusic;

    private Animator menuAnimator;
    private Animator optionsAwakeAnimator;

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
        optionsAwakeAnimator = optionsAwake.GetComponent<Animator>();
        gameMusic = GameObject.FindObjectOfType<AudioSource>();
    }

    void Update()
    {
        switch (GameCore.Instance.gameState)
        {
            case GameState.AWAKE:
                {
                    
                }
                break;
            case GameState.PAUSE:
                break;
            case GameState.PLAY:
                {
                    staminaBarValue.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, AuxLib.Map(GameCore.Instance.playerController.GetCurrentPower(), 0, GameCore.Instance.playerController.maxPower, 0, 256 * 2));
                }
                break;
            case GameState.GAMEOVER:
                break;
            default:
                break;
        }

        score.GetComponent<Text>().text = "DISTANCE: " + GameCore.Instance.playerController.distanceSinceStart;
    }

    public void OpenOptions()
    {
        optionsAwakeAnimator.SetTrigger("activate");
    }

    public void CloseOptions()
    {
        optionsAwakeAnimator.SetTrigger("deactivate");
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
            Time.timeScale = 0;
        }
        else if(GameCore.Instance.gameState == GameState.PAUSE)
        {
            GameCore.Instance.gameState = GameState.PLAY;
            Time.timeScale = 1;
        }
    }

    public void ChangeVolume(float sliderVolume)
    {
        gameMusic.volume = sliderVolume;
    }

}
