using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

    public Transform playerTransform;
    private Vector3 drawPointSpawnPos;//Position to spawn draw points

    public List<GameObject> drawPoints = new List<GameObject>();

    private const int DAY_LAYER = 8;
    private const int NIGHT_LAYER = 9;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(InputManager.DrawTouch(ref drawPointSpawnPos) == true)
        {
            GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPoint", typeof(GameObject)) as GameObject);
            drawPoints.Add(newPoint);
        }

        OverlapOtherWorld();
    }

    void OverlapOtherWorld()
    {

        foreach (GameObject p in drawPoints)
        {
            Debug.Log(Vector2.Distance(p.transform.position, playerTransform.position));
            if (Vector2.Distance(p.transform.position, playerTransform.position) < 2)
            {
                playerTransform.gameObject.layer = DAY_LAYER;
            }
        }

        playerTransform.gameObject.layer = NIGHT_LAYER;
    }
}
