using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPointController : MonoBehaviour {

    public float rayDistance;
    public int[] rayLayerID;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null && hit.collider.tag == "Terrain")
        {
            Debug.Log("Found: " + hit.transform.name);
        }
    }
}
