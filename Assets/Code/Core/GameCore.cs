using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

    public GameObject player;

    public Transform cameraSystemTransform;
    public Vector3 cameraPositionOffset;

    [HideInInspector]
    public C_PlayerController playerController;

    private Vector3 drawPointSpawnPos;//Position to spawn draw points


    private const int DAY_LAYER = 8;
    private const int NIGHT_LAYER = 9;

    RaycastHit hit;
    // Use this for initialization
    void Start () {
        playerController = player.GetComponent<C_PlayerController>();

    }
	
	// Update is called once per frame
	void Update () {
        UpdateCameraTransform();
        SpawnMaskPoints();
        OverlapOtherWorld();
    }

    void SpawnMaskPoints()
    {
        if (InputManager.DrawTouch(ref drawPointSpawnPos) == true)
        {
            GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPoint", typeof(GameObject)), drawPointSpawnPos, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }

    void OverlapOtherWorld()
    {
        if(Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y, -100), Vector3.forward, out hit, 1000))
        {
            if (hit.transform.gameObject.tag == "Depth")
            {
                player.gameObject.layer = DAY_LAYER;
            }

        }

        else player.gameObject.layer = NIGHT_LAYER;

    }

    void UpdateCameraTransform()
    {
        cameraSystemTransform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0) + cameraPositionOffset;
    }

}
