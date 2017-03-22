using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour {

    protected Transform target;
    protected float speed;
    protected float spawnHeight;//0 means on floor.
	// Use this for initialization
	protected void Awake () {
        transform.position = AuxLib.SetPositionOnRaycastHit2D(transform.gameObject, "Terrain", Vector2.down, spawnHeight);
	}
}
