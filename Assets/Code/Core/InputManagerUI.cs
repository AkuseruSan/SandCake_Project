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
    public Transform finalScore;
    public Transform optionsAwake;
    public Transform staminaBarValue;
    public Transform hud;
    public Transform endMenu;

    public Slider volume;
    public AudioSource gameMusic;

    private Animator menuAnimator;
    private Animator optionsAwakeAnimator;
    private Animator hudDisplay;
    private Animator endMenuAnimator;


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
        endMenuAnimator = endMenu.GetComponent<Animator>();
        gameMusic = GameObject.FindObjectOfType<AudioSource>();
        hudDisplay = hud.GetComponent<Animator>();

        endMenuAnimator.SetBool("Show", false);
        hudDisplay.SetBool("Playing", false);
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
                    hudDisplay.SetBool("Playing", true);
                    staminaBarValue.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, AuxLib.Map(GameCore.Instance.playerController.GetCurrentPower(), 0, GameCore.Instance.playerController.maxPower, 0, 256 * 2));
                }
                break;
            case GameState.GAMEOVER:
                {
                    
                    endMenuAnimator.SetBool("Show", true);
                    finalScore.GetComponent<Text>().text = "SCORE: " + GameCore.Instance.playerController.distanceSinceStart;
                }
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

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeVolume(float sliderVolume)
    {
        gameMusic.volume = sliderVolume;
    }

}
