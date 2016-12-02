using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public enum GameState { AWAKE, PAUSE, PLAY }

//Singleton Controller
public class GameCore : MonoBehaviour {

    public static GameCore Instance { get; private set; }

    [HideInInspector]
    public GameState gameState;

    public GameObject player;
    public Transform cameraSystemTransform;
    public Transform parallaxSystemTransform;
    public Transform worldManager;

    public Vector3 cameraPositionOffset;
    public float camSize;

    public float worldConstructorSpawnToSpawnDistance;//Position between every spawn. Must be constant

    [HideInInspector]
    public PlayerController playerController;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    public List<WorldDictionaryList> worldModulesList;

    public Dictionary<WorldModuleType,List<WorldModuleData>> worldModules;

    private Vector3 drawPointSpawnPos;//Position to spawn draw points

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
    }

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);

        gameState = GameState.AWAKE;
        playerController = player.GetComponent<PlayerController>();

        worldModules = new Dictionary<WorldModuleType, List<WorldModuleData>>();

        InitializeWorldModules();

        worldManager.GetChild(0).transform.position = new Vector3(worldConstructorSpawnToSpawnDistance, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        switch (gameState)
        {
            case GameState.AWAKE:
                {
                    
                }
                break;
            case GameState.PAUSE:
                {

                }
                break;
            case GameState.PLAY:
                {
                    UpdateCameraTransform();
                    UpdateParallaxTransform();
                    UpdateWorldManager();

                    if(!EventSystem.current.IsPointerOverGameObject())
                        SpawnMaskPoints();

                    OverlapOtherWorld();
                }
                break;
            default:
                break;
        }

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

        if (cameraSystemTransform.position.y + Camera.main.orthographicSize * 0.5f > (parallaxSystemTransform.localScale.y * 0.5f) - Camera.main.orthographicSize * 0.5f)
        {
            cameraSystemTransform.position = new Vector3(player.transform.position.x + cameraPositionOffset.x, (parallaxSystemTransform.localScale.y * 0.5f) - Camera.main.orthographicSize, 0);
        }

        else if (cameraSystemTransform.position.y - Camera.main.orthographicSize * 0.5f < (-parallaxSystemTransform.localScale.y * 0.5f) + Camera.main.orthographicSize * 0.5f)
        {
            cameraSystemTransform.position = new Vector3(player.transform.position.x + cameraPositionOffset.x, (-parallaxSystemTransform.localScale.y * 0.5f) + Camera.main.orthographicSize, 0);
        }
    }

    void UpdateParallaxTransform()
    {
        parallaxSystemTransform.position = new Vector3(cameraSystemTransform.position.x, 0, 0);
    }

    void InitializeWorldModules()
    {

        foreach (WorldDictionaryList data in worldModulesList)
        {        
            foreach(WorldModuleData mod in data.worldModules)
            {
                if (!worldModules.ContainsKey(data.type))
                    worldModules.Add(data.type, new List<WorldModuleData>());

                worldModules[data.type].Add((new WorldModuleData(mod.beginConnection, mod.endConnection, mod.module)));
            }
        }
    }

    void UpdateWorldManager()
    {
        worldManager.position = new Vector3(cameraSystemTransform.position.x, 0, 0);
    }
}
