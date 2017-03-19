using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = AuxLib.SetPositionOnRaycastHit2D(transform, "Terrain", Vector2.down, 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
