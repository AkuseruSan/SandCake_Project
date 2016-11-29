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
    public Transform backgroundMainMenu;

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

    public void StartGame()
    {
        menuAnimator.SetTrigger("exit");
        GameCore.Instance.playerController.speed = 1.5f;
    }

	public void JumpButton()
    {
        GameCore.Instance.playerController.Jump();
    }
}
