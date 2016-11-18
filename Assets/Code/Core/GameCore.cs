using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

    public Transform playerTransform;
    private Vector3 drawPointSpawnPos;//Position to spawn draw points

    public List<GameObject> drawPoints = new List<GameObject>();

    private const int DAY_LAYER = 8;
    private const int NIGHT_LAYER = 9;

    RaycastHit hit;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(InputManager.DrawTouch(ref drawPointSpawnPos) == true)
        {
            GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPoint", typeof(GameObject)), drawPointSpawnPos, Quaternion.Euler(0,0,0)) as GameObject;
            drawPoints.Add(newPoint);
        }

        OverlapOtherWorld();
    }

    void OverlapOtherWorld()
    {
        if(Physics.Raycast(new Vector3(playerTransform.transform.position.x, playerTransform.position.y, -100), Vector3.forward, out hit, 1000))
        {
            if (hit.transform.gameObject.tag == "Depth")
            {
                playerTransform.gameObject.layer = DAY_LAYER;
            }

        }

        else playerTransform.gameObject.layer = NIGHT_LAYER;

    }
}
