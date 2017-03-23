using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenBulletBehaviour : MonoBehaviour {

    float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime, Space.Self);
	}

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
