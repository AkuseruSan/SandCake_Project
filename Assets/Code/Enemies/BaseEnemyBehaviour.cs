using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour {

    protected int dmg;
    protected Transform target;
    protected float speed;
    protected float spawnHeight = 5;//0 means on floor.
	// Use this for initialization
	protected void Start () {
        transform.position = AuxLib.SetPositionOnRaycastHit2D(transform.position, "Terrain", Vector2.down, spawnHeight);
        //Debug.Log("Floor Position Tracked");
    }

}
