using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


//Singleton Controller
public class GameCore : MonoBehaviour {

    public static GameCore Instance { get; private set; }

    public GameObject player;
    public Transform cameraSystemTransform;
    public Transform parallaxSystemTransform;

    public Vector3 cameraPositionOffset;
    public float camSize;

    public float worldConstructorOffsetX;//Position between every spawn.

    [HideInInspector]
    public C_PlayerController playerController;

    public List<WorldModuleData> worldModules;

    private Vector3 drawPointSpawnPos;//Position to spawn draw points

    private bool isPaused;

    public bool IsPaused()
    {
        return isPaused;
    }

    public const int DAY_LAYER = 8;
    public const int NIGHT_LAYER = 9;

    RaycastHit hit;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        isPaused = false;
    }

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
        playerController = player.GetComponent<C_PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCameraTransform();
        UpdateParallaxTransform();
        SpawnMaskPoints();
        OverlapOtherWorld();
    }

    void SpawnMaskPoints()
    {
        if (InputManager.Instance.DrawTouch(ref drawPointSpawnPos) == true)
        {
            if (Physics.Raycast(new Vector3(drawPointSpawnPos.x, drawPointSpawnPos.y, -100), Vector3.forward, out hit, 1000))
            {
                if (hit.transform.gameObject.tag != "Depth")
                {
                    InstantiateSpawnPoint();
                }
            }

            else
            {
                InstantiateSpawnPoint();
            }
        }
    }

    void InstantiateSpawnPoint()
    {
        GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPoint", typeof(GameObject)), drawPointSpawnPos, Quaternion.Euler(0, 180, 0)) as GameObject;
    }

    void OverlapOtherWorld()
    {
        if(Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y, -100), Vector3.forward, out hit, 1000))
        {
            if (hit.transform.gameObject.tag == "Depth")
            {
                player.gameObject.layer = DAY_LAYER;

            }

            else player.gameObject.layer = NIGHT_LAYER;
        }

        else player.gameObject.layer = NIGHT_LAYER;

    }

    void UpdateCameraTransform()
    {
        cameraSystemTransform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0) + cameraPositionOffset;
    }

    void UpdateParallaxTransform()
    {
        if (cameraSystemTransform.position.y - (Camera.main.orthographicSize * 0.5f) > parallaxSystemTransform.position.y + (Camera.main.orthographicSize * 0.5f))
        {
            parallaxSystemTransform.position = new Vector3(cameraSystemTransform.position.x, cameraSystemTransform.position.y - (Camera.main.orthographicSize), 0);
        }

        else if(cameraSystemTransform.position.y + (Camera.main.orthographicSize * 0.5f) < parallaxSystemTransform.position.y - (Camera.main.orthographicSize * 0.5f))
        {
            parallaxSystemTransform.position = new Vector3(cameraSystemTransform.position.x, cameraSystemTransform.position.y + (Camera.main.orthographicSize), 0);
        }

        else parallaxSystemTransform.position = new Vector3(cameraSystemTransform.position.x, parallaxSystemTransform.position.y, 0);
    }

}
