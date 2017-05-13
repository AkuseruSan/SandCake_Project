using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldConstructor : MonoBehaviour {

    private float lastX;

    private float flowerSpawnCtr;//Counter = ctr
    private float treeSpawnCounter;

    //Stores an array with a lenght of the number of zones
    private uint[] zonesNumber;

    Queue<uint> current;

    //Current stage/zone
    public Stage currentStage;

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

        EnqueuerSystem();
    }
    // Use this for initialization
    void Start () {
        lastX = transform.position.x;
        
    }

    void Update()
    {
        SpawnFlowers();
        SpawnTrees();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        EnqueuerSystem();

        if (transform.position.x - lastX >= GameCore.Instance.worldConstructorSpawnToSpawnDistance)
        {
            lastX = transform.position.x;
            SpawnWorldModule();
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
                treeSpawnCounter = 0;
            }

            else if (spawnPositionCheckR.y <= spawnPosition.y - 0.2)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/StaminaFlower_INT"), spawnPositionCheckR, Quaternion.identity) as GameObject;
            }

            else if (spawnPositionCheckL.y <= spawnPosition.y - 0.2)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/StaminaFlower_INT"), spawnPositionCheckL, Quaternion.identity) as GameObject;
            }

            else
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/StaminaFlower_INT"), spawnPosition, Quaternion.identity) as GameObject;
            }

        }

        flowerSpawnCtr -= Time.deltaTime;
    }

    void SpawnTrees()
    {
        if (treeSpawnCounter <= 0)
        {
            treeSpawnCounter = Random.Range(1, 6);

            Vector3 spawnPosition = AuxLib.SetPositionOnRaycastHit2D(new Vector3(transform.position.x - 10, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckR = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x + 2, 20, 0), "Terrain", Vector2.down, 1);
            Vector3 spawnPositionCheckL = AuxLib.SetPositionOnRaycastHit2D(new Vector3(spawnPosition.x - 2, 20, 0), "Terrain", Vector2.down, 1);

            if (spawnPosition.y >= 20)
            {
                treeSpawnCounter = 0;
            }

            else if (spawnPositionCheckR.y <= spawnPosition.y - 0.2)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Assets/Tree_00"), spawnPositionCheckR, Quaternion.identity) as GameObject;
            }

            else if(spawnPositionCheckL.y <= spawnPosition.y - 0.2)
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

    void EnqueuerSystem()
    {
        //Automatically adds new modules to the queue if not full
        while (current.Count < ListOfIndex[(int)currentStage].Count / 2)
        {

            int selectedIndex = Random.Range(0, (ListOfIndex[(int)currentStage].Count));

            // ListOfIndex = List with all the modules ID's
            // currentStage = Representation of the current zone
            // selectedIndex = Random number to select a modlue inside the corresponding zone
            //ListOfIndex[(int)currentStage][selectedIndex] = ID of a module
            WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)currentStage][selectedIndex]];

            //First module added || in case of empty queue
            if (previousModule == null || current.Count <= 0)
            {
                previousModule = currentModule;
                current.Enqueue(ListOfIndex[(int)currentStage][selectedIndex]);
                moduleCounter++;
            }

            else
            {
                //In case there's the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
                if (!current.Contains(ListOfIndex[(int)currentStage][selectedIndex])
                && previousModule.endConnection == currentModule.beginConnection && previousModule.type != currentModule.type)
                {
                    previousModule = currentModule;
                    current.Enqueue(ListOfIndex[(int)currentStage][selectedIndex]);
                    moduleCounter++;
                }
                //In case there isn't the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
                else if (!current.Contains(ListOfIndex[(int)currentStage][selectedIndex])
                && previousModule.endConnection == currentModule.beginConnection)
                {
                    previousModule = currentModule;
                    current.Enqueue(ListOfIndex[(int)currentStage][selectedIndex]);
                    moduleCounter++;
                }
            }
        }
    }

}
