using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<string> enemyPrefabPaths;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void Spawn(string path, Vector3 startPos)
    {
        GameObject go;
        go = Instantiate(Resources.Load(path) as GameObject);
        go.transform.position = startPos;
    }
}
