using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum GameState { AWAKE, PAUSE, PLAY, GAMEOVER }

//Singleton Controller
public class GameCore : MonoBehaviour
{

    public static GameCore Instance { get; private set; }

    [HideInInspector]
    public GameState gameState;

    public GameObject player;
    public Transform cameraSystemTransform;
    public Transform parallaxSystemTransform;
    public Transform worldManager;
    private Transform boss;

    public Vector3 cameraPositionOffset;
    public float camSize { get; private set; }
    public float minCamSize, maxCamSize;
    public Vector3 worldModuleScale;

    public float worldConstructorSpawnToSpawnDistance;//Position between every spawn. Must be constant

    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public EnemyManager enemyController;

    private Vector3 drawPointSpawnPos;//Position to spawn draw points

    public const int DAY_LAYER = 8;
    public const int NIGHT_LAYER = 9;

    //Data saving
    private bool saveDataOnce;

    //Current stage/zone
    public WorldConstructor.Stage currentStage;

    //Power ups state
    [HideInInspector]
    public bool barrier, doubleJump, revive, paintBoost, staminaBoost, reviveFirstFrame;

    //Score
    public int finalScore;

    //Bullet explosion particle system
    public GameObject bulletExplosion;

    //Drawpoints management
    public int maxDrawpoints = 50;
    Queue <GameObject> drawPointsSpawned;

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
    void Start()
    {
        //DontDestroyOnLoad(this);
        if (DataManager.Instance != null)
        {
            currentStage = (WorldConstructor.Stage)DataManager.Instance.currentSpawnPoint;
            minCamSize = 6;
            maxCamSize = 10;

            camSize = minCamSize;

            reviveFirstFrame = false;

            drawPointsSpawned = new Queue<GameObject>();

            worldConstructorSpawnToSpawnDistance *= worldModuleScale.x;

            gameState = GameState.AWAKE;
            playerController = player.GetComponent<PlayerController>();

            enemyController = transform.GetComponent<EnemyManager>();

            playerController.savedCheckpoints = DataManager.Instance.playerData.unlockedSpawnPoints;

            worldManager.GetChild(0).transform.position = new Vector3(worldConstructorSpawnToSpawnDistance, 0, 0);

            saveDataOnce = true;

            barrier = DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.BARRIER];
            revive = DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.REVIVE];
            doubleJump = DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.DOUBLE_JUMP];
            paintBoost = DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.PAINT_BOOST];
            staminaBoost = DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.STAMINA_BOOST];

            DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.BARRIER] = false;
            DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.REVIVE] = revive;
            DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.DOUBLE_JUMP] = false;
            DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.PAINT_BOOST] = false;
            DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.STAMINA_BOOST] = false;

            DataManager.Instance.SaveData();

            SceneManager.SetActiveScene(CoreSceneManager.Instance.currentScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.AWAKE:
                {
                    if (!Instance.barrier)
                    {
                        Instance.playerController.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        Instance.playerController.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                    }
                }
                break;
            case GameState.PAUSE:
                {

                }
                break;
            case GameState.PLAY:
                {
                    UpdateCameraSize();
                    UpdateCameraTransform();
                    UpdateParallaxTransform();
                    UpdateWorldManager();

#if UNITY_ANDROID
                    if (Input.touchCount > 0)
                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                            SpawnMaskPoints();
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
                    if (!EventSystem.current.IsPointerOverGameObject())
                        SpawnMaskPoints();
#endif


                    OverlapOtherWorld();
                }
                break;
            case GameState.GAMEOVER:
                {
                    
                    if (saveDataOnce)
                    {
                        finalScore += playerController.distanceSinceStart;
                        DataManager.Instance.playerData.activePowerUps[(int)DataManager.PowerUpID.REVIVE] = revive;
                        DataManager.Instance.playerData.unlockedSpawnPoints = playerController.savedCheckpoints;
                        DataManager.Instance.playerData.energy += System.Convert.ToUInt32(finalScore);
                        DataManager.Instance.SaveData();
                        saveDataOnce = false;
                    }
                    
                }
                break;
            default:
                break;
        }

    }

    public void SetBossRef(Transform bossref)
    {
        boss = bossref;
    }

    public Transform GetBossRef()
    {
        return boss;
    }

    void SpawnMaskPoints()
    {
        if (InputManager.Instance.DrawTouch(ref drawPointSpawnPos) == true && player.GetComponent<PlayerController>().GetCurrentPower() > 0)
        {

            InstantiateSpawnPoint();

            player.GetComponent<PlayerController>().UsePower(1);
        }
    }

    void UpdateCameraSize()
    {
        camSize = Mathf.Lerp(camSize, AuxLib.Map(playerController.rBody.velocity.x, playerController.minSpeed, playerController.maxSpeed, minCamSize, maxCamSize), Time.deltaTime * 4f);
    }

    void InstantiateSpawnPoint()
    {
        if (paintBoost)
        {
            GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPointBig", typeof(GameObject)), drawPointSpawnPos, Quaternion.Euler(0, 180, 0)) as GameObject;
            drawPointsSpawned.Enqueue(newPoint);
            //Destroy(newPoint, Time.deltaTime * 120);         
        }

        else
        {
            GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_DrawPoint", typeof(GameObject)), drawPointSpawnPos, Quaternion.Euler(0, 180, 0)) as GameObject;
            drawPointsSpawned.Enqueue(newPoint);
            //Destroy(newPoint, Time.deltaTime * 120);
        }

        if(drawPointsSpawned.Count >= maxDrawpoints)
        {
            Destroy(drawPointsSpawned.Dequeue());
        }

    }

    void OverlapOtherWorld()
    {
        if (Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y, -100), Vector3.forward, out hit, 1000))
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
        cameraSystemTransform.position = Vector3.Lerp(cameraSystemTransform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0) + cameraPositionOffset, Time.deltaTime * 4f);

        if (cameraSystemTransform.position.y + Camera.main.orthographicSize * 0.5f > (parallaxSystemTransform.localScale.y * 0.5f) - Camera.main.orthographicSize * 0.5f)
        {
            cameraSystemTransform.position = new Vector3(cameraSystemTransform.position.x, (parallaxSystemTransform.localScale.y * 0.5f) - Camera.main.orthographicSize, 0);
        }

        else if (cameraSystemTransform.position.y - Camera.main.orthographicSize * 0.5f < (-parallaxSystemTransform.localScale.y * 0.5f) + Camera.main.orthographicSize * 0.5f)
        {
            cameraSystemTransform.position = new Vector3(cameraSystemTransform.position.x, (-parallaxSystemTransform.localScale.y * 0.5f) + Camera.main.orthographicSize, 0);
        }
    }

    void UpdateParallaxTransform()
    {
        parallaxSystemTransform.position = new Vector3(cameraSystemTransform.position.x, 0, 0);
    }

    void UpdateWorldManager()
    {
        worldManager.position = new Vector3(cameraSystemTransform.position.x, 0, 0);
    }
}