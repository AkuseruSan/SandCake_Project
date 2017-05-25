using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceBehaviour : MonoBehaviour {

    private enum State { IDDLE, SHOOT }
    private State state;

    public Transform target;

    private float speed;
    // Use this for initialization
    void Start() {
        speed = 1;
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.IDDLE:
                {
                    transform.LookAt(target.position);
                }
                break;
            case State.SHOOT:
                {
                    transform.Translate(transform.forward * speed);
                }
                break;
            default:
                break;
        }
    }


}
