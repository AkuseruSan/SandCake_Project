using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldConstructor : MonoBehaviour {

    public enum Stage { Z_1, Z_2, Z_3, Z_4, Z_5, Z_BOSS, Z_CHECKPOINT };

    private float lastX;

    private float flowerSpawnCtr;//Counter = ctr
    private float treeSpawnCounter;

    public int moduleBufferNum = 2;

    //Stores an array with a lenght of the number of zones
    private uint[] zonesNumber;

    Queue<uint> current;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    //List available in the editor to manually add modules
    public List<WorldModuleData> WorldModuleList;
    //List separated in zones with each of the modules ID's
    private List<List<uint>> ListOfIndex;
    //Dictionary with all modules and keys(ID's)
    public Dictionary<uint, WorldModuleData> WorldModuleDictionary;
    //Previous module added in queue
    private WorldModuleData previousModule;

    //Number of modules spawned
    public int moduleCounter;

    //Max Zone Modules
    public List<int> moduleLimit;

    //First start bool
    private bool firsTime;

    //Boss object
    public GameObject bossPrefab;

    //Counter of revive time
    [HideInInspector]
    public float reviveCounter = 1f;
    private bool spawnOnce;

    void Awake()
    {
        //Gets the total amount of zones in the enum Stage
        int enumLength = System.Enum.GetNames(typeof(Stage)).Length;

        //Stores the total number of zones
        zonesNumber = new uint[enumLength];

        //Stores all the indices of the modules in a list of lists
        ListOfIndex = new List<List<uint>>();

        for (int i = 0; i < enumLength; ++i)
        {
            ListOfIndex.Add(new List<uint>());
        }

        WorldModuleDictionary = new Dictionary<uint, WorldModuleData>();

        for (int i = 0; i < WorldModuleList.Count; ++i)
        {
            //Gives an ID to the current module depending on the zone
            WorldModuleList[i].SetID((int)zonesNumber[(int)WorldModuleList[i].stage]);
            ListOfIndex[(int)WorldModuleList[i].stage].Add(WorldModuleList[i].GetID());
            zonesNumber[(int)WorldModuleList[i].stage]++;

            //Puts the ID of the module as key and the module as value
            WorldModuleDictionary.Add(WorldModuleList[i].GetID(), WorldModuleList[i]);
        }

        //Queue start
        current = new Queue<uint>();
        firsTime = true;

    }
    // Use this for initialization
    void Start () {
        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.currentSpawnPoint == (uint)Stage.Z_BOSS)
            {
                SpawnBoss();
            }

            lastX = transform.position.x;
            spawnOnce = false;
        }
    }

    void Update()
    {
        if (GameCore.Instance != null)
        {
            SpawnFlowers();
            SpawnTrees();
            if (GameCore.Instance.playerController.spawnGiantSun)
            {
                if (!spawnOnce)
                {
                    Vector3 spawnPos = new Vector3(GameCore.Instance.playerController.transform.position.x, GameCore.Instance.playerController.transform.position.y, -5);
                    SpawnGiantSun(spawnPos);
                    spawnOnce = true;
                }

                if (reviveCounter <= 0)
                {
                    GameCore.Instance.playerController.spawnGiantSun = false;
                    Time.timeScale = 1f;
                }

                if (GameCore.Instance.reviveFirstFrame)
                {
                    reviveCounter -= Time.deltaTime;
                    GameCore.Instance.reviveFirstFrame = false;
                }
                else
                {
                    reviveCounter -= Time.deltaTime * 1000f;
                }

            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (GameCore.Instance != null)
        {
            StartCoroutine(EnqueuerSystem());

            if (transform.position.x - lastX >= GameCore.Instance.worldConstructorSpawnToSpawnDistance)
            {
                lastX = transform.position.x;
                SpawnWorldModule();
            }
        }
    }

    void SpawnFlowers()
    {
        if (flowerSpawnCtr <= 0)
        {
            flowerSpawnCtr = Random.Range(1, 3);

            Vector3 spawnPosition = AuxLib.SetPositionOnRaycastHit2D(new Vector3(transform.position.x - 10, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckR = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x + 2, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckL = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x - 2, 20, 0), "Terrain", Vector2.down, 1);

            if (spawnPosition.y >= 20)
            {
                flowerSpawnCtr = 0;
            }

            else if (spawnPositionCheckR.y <= spawnPosition.y - 0.4)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/Flower"), spawnPositionCheckR, Quaternion.identity) as GameObject;
            }

            else if (spawnPositionCheckL.y <= spawnPosition.y - 0.4)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/Flower"), spawnPositionCheckL, Quaternion.identity) as GameObject;
            }

            else
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/Flower"), spawnPosition, Quaternion.identity) as GameObject;
            }

        }

        flowerSpawnCtr -= Time.deltaTime;
    }

    void SpawnTrees()
    {
        if (treeSpawnCounter <= 0 && GameCore.Instance.currentStage != Stage.Z_BOSS)
        {
            treeSpawnCounter = Random.Range(1, 6);

            Vector3 spawnPosition = AuxLib.SetPositionOnRaycastHit2D(new Vector3(transform.position.x - 10, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckR = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x + 2, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckL = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x - 2, 20, 0), "Terrain", Vector2.down, 1);

            if (spawnPosition.y >= 20)
            {
                treeSpawnCounter = 0;
            }

            else if (spawnPositionCheckR.y <= spawnPosition.y - 0.4)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Assets/Tree_00"), spawnPositionCheckR, Quaternion.identity) as GameObject;
            }

            else if(spawnPositionCheckL.y <= spawnPosition.y - 0.4)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Assets/Tree_00"), spawnPositionCheckL, Quaternion.identity) as GameObject;
            }

            else
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Assets/Tree_00"), spawnPosition, Quaternion.identity) as GameObject;
            }
            

        }

        treeSpawnCounter -= Time.deltaTime;
    }

    void SpawnWorldModule()
    {
        GameObject go = Instantiate(WorldModuleDictionary[current.Dequeue()].module as GameObject);
        go.transform.position = transform.position;
        go.transform.localScale = GameCore.Instance.worldModuleScale;
    }

    IEnumerator EnqueuerSystem()
    {
        //Automatically adds new modules to the queue if not full
        if(GameCore.Instance.currentStage == Stage.Z_BOSS)
        {
            if(current.Count < 5)
            {
                Debug.Log("Here Comes that bauss, oh shit waddup");
                current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][0]);
            }            
        }

        else {

            while (current.Count < moduleBufferNum)
            {

                int selectedIndex = Random.Range(0, (ListOfIndex[(int)GameCore.Instance.currentStage].Count));

                // ListOfIndex = List with all the modules ID's
                // GameCore.Instance.currentStage = Representation of the current zone
                // selectedIndex = Random number to select a modlue inside the corresponding zone
                // ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex] = ID of a module
                WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex]];

                //First module added || in case of empty queue
                if (previousModule == null)
                {
                    if (firsTime)
                    {
                        for (int i = 0; i < ListOfIndex[(int)GameCore.Instance.currentStage].Count; ++i)
                        {
                            currentModule = WorldModuleDictionary[ListOfIndex[(int)GameCore.Instance.currentStage][i]];
                            if (currentModule.beginConnection == WorldModuleConnect.MIDDLE && firsTime)
                            {
                                previousModule = currentModule;
                                current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][i]);
                                moduleCounter++;
                                firsTime = false;
                            }
                        }
                    }

                    else
                    {
                        previousModule = currentModule;
                        current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex]);
                        moduleCounter++;
                    }

                }

                else
                {

                    //In case there's the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
                    if (!current.Contains(ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex])
                    && previousModule.endConnection == currentModule.beginConnection && previousModule.type != currentModule.type
                    && GameCore.Instance.currentStage != Stage.Z_BOSS)
                    {
                        previousModule = currentModule;
                        current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex]);
                        moduleCounter++;
                    }
                    //In case there isn't the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
                    else if (!current.Contains(ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex])
                    && previousModule.endConnection == currentModule.beginConnection
                    && GameCore.Instance.currentStage != Stage.Z_BOSS)
                    {
                        previousModule = currentModule;
                        current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][selectedIndex]);
                        moduleCounter++;
                    }

                    //Spawn CheckPoints
                    if (moduleCounter == moduleLimit[(int)GameCore.Instance.currentStage] && GameCore.Instance.currentStage != Stage.Z_BOSS)
                    {
                        for (int i = 0; i < ListOfIndex[(int)Stage.Z_CHECKPOINT].Count; ++i)
                        {
                            currentModule = WorldModuleDictionary[ListOfIndex[(int)Stage.Z_CHECKPOINT][i]];
                            if (previousModule.endConnection == currentModule.beginConnection)
                            {
                                previousModule = currentModule;
                                current.Enqueue(ListOfIndex[(int)Stage.Z_CHECKPOINT][i]);
                                GameCore.Instance.currentStage++;
                                if (GameCore.Instance.currentStage == Stage.Z_BOSS)
                                {
                                    SpawnBoss();
                                }
                                moduleCounter = 0;
                            }
                        }
                    }

                    else if (GameCore.Instance.currentStage == Stage.Z_BOSS)
                    {
                        Debug.Log("Here Comes that bauss, oh shit waddup");
                        current.Enqueue(ListOfIndex[(int)GameCore.Instance.currentStage][0]);
                    }

                }
            }
        }
        yield return null;
    }
    public void SpawnGiantSun(Vector3 playerPosition)
    {
        GameObject newPoint = Instantiate(Resources.Load("Prefabs/P_GiantSun", typeof(GameObject)), playerPosition, Quaternion.Euler(0, 180, 0)) as GameObject;
    }

    public void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab) as GameObject;
        GameCore.Instance.SetBossRef(boss.transform);
    }
}
