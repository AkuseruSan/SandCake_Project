using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

    private enum State { IDDLE, LOAD, LOADING, AIM, ATTACK, DIE }
    private State state;

    public GameObject lance;

    private Queue<GameObject> lances;

    public Transform[] lanceSpawnPoints;

    private GameObject player;
    private PlayerController playerController;

    private float distanceToPlayer;
    private static int MAX_LANCES;
    private int lancesCounter;

    private float loadingSpeed;
    private float loadingCounter;
	// Use this for initialization
	void Start () {

        MAX_LANCES = 5;
        distanceToPlayer = 5;
        loadingSpeed = 1;

        state = State.IDDLE;

        player = GameCore.Instance.player;
        playerController = GameCore.Instance.playerController;

        Debug.Log(player.name);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(player.transform.position.x + distanceToPlayer, player.transform.position.y, player.transform.position.z);

        switch (state)
        {
            case State.IDDLE:
                {
                    //Reset all states.
                    lancesCounter = MAX_LANCES;

                    state = State.LOAD;
                }
                break;
            case State.LOAD:
                {
                    loadingCounter = loadingSpeed;
                    state = State.LOADING;
                }
                break;
            case State.LOADING:
                {
                    loadingCounter -= Time.deltaTime;

                    if (loadingCounter <= 0)
                    {

                        lancesCounter--;

                        if (lancesCounter <= 0) state = State.AIM;
                        else state = State.LOAD;
                    }
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

    void SpawnLance()
    {
        GameObject g = Instantiate(lance, lanceSpawnPoints[lancesCounter - 1].position, Quaternion.identity, this.transform);
        g.GetComponent<LanceBehaviour>().target = GameCore.Instance.player.transform;
    }
}
