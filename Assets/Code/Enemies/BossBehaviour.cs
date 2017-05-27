using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

    private enum State { IDDLE, LOAD, LOADING, SHOOT, DIE }
    private State state;

    public GameObject lance;

    private Queue<LanceBehaviour> lances;

    public Transform[] lanceSpawnPoints;

    private GameObject player;
    private PlayerController playerController;

    private float distanceToPlayer;
    private static int MAX_LANCES;
    private int lancesCounter;

    private float attackRotationCounter;

    private float loadingSpeed;
    private float loadingCounter;
	// Use this for initialization
	void Start () {

        MAX_LANCES = 5;
        distanceToPlayer = 10;
        loadingSpeed = 0.6f;

        attackRotationCounter = Random.Range(4, 10);

        state = State.IDDLE;

        player = GameCore.Instance.player;
        playerController = GameCore.Instance.playerController;

        Debug.Log(player.name);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("BOSS STATE: " + state);
        transform.position = new Vector3(player.transform.position.x + distanceToPlayer, transform.position.y, transform.position.z);

        switch (state)
        {
            case State.IDDLE:
                {
                    attackRotationCounter -= Time.deltaTime;
                    //Reset all states.
                    if(attackRotationCounter <= 0)
                    {
                        lancesCounter = MAX_LANCES;
                        state = State.LOAD;
                    }
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
                        SpawnLance();
                        lancesCounter--;

                        if (lancesCounter <= 0)
                        {
                            loadingCounter = loadingSpeed;
                            attackRotationCounter = Random.Range(4, 10);
                            state = State.IDDLE;
                        }
                        else state = State.LOAD;
                    }
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
        g.GetComponent<LanceBehaviour>().Init(GameCore.Instance.player.transform, 10, 1, "BossLance");
        //g.GetComponent<LanceBehaviour>().target = GameCore.Instance.player.transform;
    }
}
