using UnityEngine;
using System.Collections;

public class SpawnPointBehaviour : MonoBehaviour {

    private float rotSpd;
    private Vector3 rot;
	// Use this for initialization
	void Start () {
        rot = new Vector3(0, 0, 3);
	}

	// Update is called once per frame
	void Update () {

        transform.Rotate(rot);
	}
}
