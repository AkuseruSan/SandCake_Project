using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Class only called by UI elements events.
public class InputManagerUI : MonoBehaviour {

    private GameCore core;

    void Start()
    {
        core = GetComponent<GameCore>();
    }

	public void JumpButton()
    {
        core.playerController.Jump();
    }
}
