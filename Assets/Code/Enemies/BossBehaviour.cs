using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

    private enum State { IDDLE, LOAD, AIM, ATTACK, DIE }
    private State state;

    public GameObject lance;

    private Queue<GameObject> lances;

    private GameObject player;
    private PlayerController playerController;

    private float distanceToPlayer;
    private static int MAX_LANCES;
    private int lancesCounter;
	// Use this for initialization
	void Start () {
        distanceToPlayer = 10;

        player = GameCore.Instance.player;
        playerController = GameCore.Instance.playerController;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(player.transform.position.x + distanceToPlayer, player.transform.position.y, player.transform.position.z);

        switch (state)
        {
            case State.IDDLE:
                {
                    state = State.LOAD;
                }
                break;
            case State.LOAD:
                {
                    LoadLances();
                    state = State.AIM;
                }
                break;
            case State.AIM:
                {
                    
                }
                break;
            case State.ATTACK:
                {

                }
                break;
            case State.DIE:
                {

                }
                break;
            default:
                break;
        }
    }

    void LoadLances()
    {
        lancesCounter = MAX_LANCES;

        StartCoroutine(LoadLance());
    }

    private IEnumerator LoadLance()
    {
        GameObject g = Instantiate(lance, transform.position, Quaternion.identity, this.transform);

        yield return new WaitForSeconds(1);

        lancesCounter--;
        if(lancesCounter <= 0)
        {
            
        }
        else
        {
            StartCoroutine(LoadLance());
        }
    }
}
