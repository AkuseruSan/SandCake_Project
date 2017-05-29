using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

    private enum State { IDDLE, LOAD, LOADING, SHOOT, DIE }
    private State state;

    private float maxLife;
    public float life;

    public GameObject lancePrefab;
    public GameObject lanceExplosion;

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
        maxLife = 100;
        life = maxLife;

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
        if(DataManager.Instance.godMode)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                life -= 10;
            }
        }

        Debug.Log("BOSS STATE: " + state);
        transform.position = new Vector3(player.transform.position.x + distanceToPlayer, transform.position.y, transform.position.z);

        //transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + distanceToPlayer, transform.position.y, transform.position.z), Time.deltaTime * 25);
        if (life <= 0) state = State.DIE;

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
                    GameCore.Instance.gameState = GameState.GAME_COMPLETE;

                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }

    void SpawnLance()
    {
        GameObject g = Instantiate(lancePrefab, lanceSpawnPoints[lancesCounter - 1].position, Quaternion.identity, this.transform);
        g.GetComponent<LanceBehaviour>().Init(GameCore.Instance.player.transform, 20, 1, "BossLance");
        //g.GetComponent<LanceBehaviour>().target = GameCore.Instance.player.transform;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "PlayerLance")
        {
            Debug.Log("Ouch! >.<");
            life -= 1;

            StopCoroutine("DamageAnim");
            StartCoroutine(DamageAnim(0.1f));
            Instantiate(lanceExplosion, col.transform.position, Quaternion.identity);
            Destroy(col.gameObject);
        }
    }

    private IEnumerator DamageAnim(float sec)
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(sec);
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
