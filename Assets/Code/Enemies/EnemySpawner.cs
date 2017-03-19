using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButton(0))
        {
            Spawn();
        }	
	}

    void Spawn()
    {
        GameObject go;
        go = Instantiate(Resources.Load("Prefabs/Enemies/BaseEnemy") as GameObject);
        go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
