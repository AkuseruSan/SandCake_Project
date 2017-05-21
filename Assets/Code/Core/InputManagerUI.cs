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
    public Transform pausePanel;
    public Transform actualScore;

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
        gameMusic = GameObject.FindObjectOfType<AudioSource>();

        menuAnimator = mainMenuPanel.GetComponent<Animator>();
        optionsAwakeAnimator = optionsAwake.GetComponent<Animator>();
        endMenuAnimator = endMenu.GetComponent<Animator>();
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
                    finalScore.GetComponent<Text>().text = "SCORE: " + GameCore.Instance.finalScore;
                }
                break;
            default:
                break;
        }

        score.GetComponent<Text>().text = "SCORE: " + GameCore.Instance.finalScore;
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
        
        if (pausePanel.gameObject.activeInHierarchy == false)
        {
            GameCore.Instance.gameState = GameState.PAUSE;
            pausePanel.gameObject.SetActive(true);
            actualScore.GetComponent<Text>().text = "SCORE: " + GameCore.Instance.playerController.distanceSinceStart;
            Time.timeScale = 0;
        }

        else
        {
            GameCore.Instance.gameState = GameState.PLAY;
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void BackToMainMenu()
    {
        CoreSceneManager.Instance.SwitchScene(CoreSceneManager.SceneID.MENU);
        Time.timeScale = 1;
    }

    public void ChangeVolume(float sliderVolume)
    {
        gameMusic.volume = sliderVolume;
    }

}
