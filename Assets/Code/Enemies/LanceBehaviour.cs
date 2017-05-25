using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceBehaviour : MonoBehaviour {

    private enum State { IDDLE, SHOOT }
    private State state;
    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.IDDLE:
                {

                }
                break;
            case State.SHOOT:
                {

                }
                break;
            default:
                break;
        }
    }


}
