using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Class only called by UI elements events.
public class InputManagerUI : MonoBehaviour {

    public static InputManagerUI Instance {get; private set;}

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

    }

	public void JumpButton()
    {
        GameCore.Instance.playerController.Jump();
    }
}
