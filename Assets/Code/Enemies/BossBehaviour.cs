using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

    private enum State { IDDLE, LOAD, ATTACK, DIE }
    private State state;

    public GameObject lance;

    private Queue<GameObject> lances;

    private GameObject player;
    private PlayerController playerController;
	// Use this for initialization
	void Start () {
        player = GameCore.Instance.player;
        playerController = GameCore.Instance.playerController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadLances()
    {

    }
}
