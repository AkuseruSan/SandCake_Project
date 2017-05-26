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
        speed = 10;

        loadTime = 1;

        timeCounter = loadTime;

        transform.forward = Vector3.left;
        
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

                    transform.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                break;
            case State.SHOOT:
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                break;
            case State.TRANSFORM:
                {
                    transform.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                break;
            default:
                break;
        }
    }
    private void OnColisionEnter(Collision col)
    {
        if (col.transform.tag == "Depth")
        {
            state = State.TRANSFORM;
        }
    }

    private void OnColisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Depth")
        {
            state = State.TRANSFORM;
        }
    }

}
