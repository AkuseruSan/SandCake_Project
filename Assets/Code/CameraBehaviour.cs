using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    Camera cam;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        cam.orthographicSize = GameCore.Instance.camSize;
	}
}
