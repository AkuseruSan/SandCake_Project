using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour {

    protected int dmg;
    protected Transform target;
    protected float speed;
      //0 means on floor.
	// Use this for initialization
	protected void Start () {
        //Debug.Log("New Position: " + transform.position);
    }

}
