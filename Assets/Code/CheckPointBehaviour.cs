using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour {

    public Animator checkPointLight;
    public ParticleSystem activateCheckPoint;
    private bool checkPointActive;

    // Use this for initialization
    void Start () {
        checkPointLight = GetComponent<Animator>();
        checkPointLight.SetBool("CheckpointActive", false);
        checkPointActive = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameCore.Instance.savedCheckpoints < GameCore.Instance.checkpointsSurpassed)
        {

            checkPointActive = true;
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !checkPointActive)
        {
            checkPointLight.SetBool("CheckpointActive", true);
            activateCheckPoint.Play();
        } 
    }
}
