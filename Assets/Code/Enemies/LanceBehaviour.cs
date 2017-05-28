using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceBehaviour : MonoBehaviour {

    public enum State { IDDLE, LOADING, SHOOT, TRANSFORM }
    public State state;

    public Transform target;

    private float speed;
    private float loadTime;
    private float timeCounter;
    // Use this for initialization
    void Start() {
        state = State.IDDLE;

        transform.forward = Vector3.left;
        
    }

    public void Init(Transform targ, float spd, float startLoadTime, string lanceTag)
    {
        transform.tag = lanceTag;
        target = targ;

        speed = spd;

        loadTime = startLoadTime;

        timeCounter = loadTime;
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.IDDLE:
                {
                    timeCounter -= Time.deltaTime;

                    transform.LookAt(new Vector3(target.position.x, target.position.y, transform.position.z));
                    transform.Rotate(0, -90, -180);

                    if(timeCounter <= 0)
                    {
                        timeCounter = loadTime;
                        state = State.LOADING;
                    }
                }
                break;
            case State.LOADING:
                {
                    timeCounter -= Time.deltaTime;

                    if(timeCounter <= 0)
                    {
                        transform.parent = null;
                        state = State.SHOOT;
                    }

                    transform.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                }
                break;
            case State.SHOOT:
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                break;
            case State.TRANSFORM:
                {
                    GameObject g = Instantiate(this.gameObject, transform.position, Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0, 0, 180)));

                    g.GetComponent<LanceBehaviour>().Init(GameCore.Instance.GetBossRef(), 25, 0, "PlayerLance");

                    g.transform.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    Debug.Log("Transformed");

                    //Destroy This
                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }
    //private void OnColisionEnter(Collision col)
    //{
    //    if (col.transform.tag == "Depth")
    //    {
    //        state = State.TRANSFORM;
    //    }
    //}

    //private void OnColisionEnter2D(Collision2D collision)
    //{
    //    if(collision.transform.tag == "Depth")
    //    {
    //        state = State.TRANSFORM;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Depth" && state == State.SHOOT)
        {
            state = State.TRANSFORM;
        }
    }

}
