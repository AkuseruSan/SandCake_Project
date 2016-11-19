using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
