//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum Stage { Z_1, Z_2, Z_3, Z_BOSS };

//public class SpawnManager : MonoBehaviour {

//    //Singleton declaration
//    public static SpawnManager Instance { get; private set; }

//    //Stores an array with a lenght of the number of zones
//    private uint[] zonesNumber;

//    Queue<uint> current;

//    //Current stage/zone
//    public Stage GameCore.currentStage;

//    [Space(20)]
//    [Header("[World Dictionary Lists]")]
//    //List available in the editor to manually add modules
//    public List<WorldModuleData> WorldModuleList;
//    //List separated in zones with each of the modules ID's
//    private List<List<uint>> ListOfIndex;
//    //Dictionary with all modules and keys(ID's)
//    public Dictionary<uint, WorldModuleData> WorldModuleDictionary;
//    //Previous module added in queue
//    private WorldModuleData previousModule;

//    public float worldConstructorSpawnToSpawnDistance;//Position between every spawn. Must be constant
//    //Number of modules spawned
//    public int moduleCounter;

//    void Awake()
//    {
//        //Gets the total amount of zones in the enum Stage
//        int enumLength = System.Enum.GetNames(typeof(Stage)).Length;

//        //Stores the total number of zones
//        zonesNumber = new uint[enumLength];

//        //Stores all the indices of the modules in a list of lists
//        ListOfIndex = new List<List<uint>>();

//        for (int i = 0; i < enumLength; ++i)
//        {
//           ListOfIndex.Add(new List<uint>());
//        }

//        WorldModuleDictionary = new Dictionary<uint, WorldModuleData>();

//        for (int i = 0; i < WorldModuleList.Count; ++i)
//        {
//            //Gives an ID to the current module depending on the zone
//            WorldModuleList[i].SetID((int)zonesNumber[(int)WorldModuleList[i].stage]);
//            ListOfIndex[(int)WorldModuleList[i].stage].Add(WorldModuleList[i].GetID());
//            zonesNumber[(int)WorldModuleList[i].stage]++;
            
//            //Puts the ID of the module as key and the module as value
//            WorldModuleDictionary.Add(WorldModuleList[i].GetID(), WorldModuleList[i]);
//        }

//        //Queue start
//        current = new Queue<uint>();

//        //-------------------- MODULE QUEUE ADDITION MANAGEMENT -----------------------//

//        while (current.Count < ListOfIndex[(int)GameCore.currentStage].Count / 2)
//        {


//            //Debug.Log("current length: " + current.Count + "  List of index length: " + ListOfIndex[(int)GameCore.currentStage].Count / 2);
//            int selectedIndex = Random.Range(0, (ListOfIndex[(int)GameCore.currentStage].Count));

//            // ListOfIndex = List with all the modules ID's
//            // GameCore.currentStage = Representation of the current zone
//            // selectedIndex = Random number to select a modlue inside the corresponding zone
//            // ListOfIndex[(int)GameCore.currentStage][selectedIndex] = ID of a module
//            WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)GameCore.currentStage][selectedIndex]];

//            if (previousModule == null)
//            {
//                previousModule = currentModule;
//                current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                moduleCounter++;

//            }

//            else
//            {
//                if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection && previousModule.type != currentModule.type)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//                else if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//            }

//        }

//        foreach (uint xData in current)
//        {
//            Debug.Log(xData);
//        }
//    }

//    // Use this for initialization
//    void Start () {

//        StartCoroutine(FillQueue());
//    }
	
//	// Update is called once per frame
//	void Update () {

//        //------------------- ↓↓↓↓↓ SPAWN MAGIC GOES HERE ↓↓↓↓↓ -----------------//

//        //Decide when to dequeue
//        if (current.Count == ListOfIndex[(int)GameCore.currentStage].Count / 2)
//        {
//            Debug.Log("Ready to spawn!");
//            current.Dequeue();
//        }

//        //-------------------- MODULE QUEUE ADDITION MANAGEMENT -----------------------//

//        //Automatically adds new modules to the queue if not full
//        while (current.Count < ListOfIndex[(int)GameCore.currentStage].Count / 2)
//        {

//            int selectedIndex = Random.Range(0, (ListOfIndex[(int)GameCore.currentStage].Count));

//            // ListOfIndex = List with all the modules ID's
//            // GameCore.currentStage = Representation of the current zone
//            // selectedIndex = Random number to select a modlue inside the corresponding zone
//            //ListOfIndex[(int)GameCore.currentStage][selectedIndex] = ID of a module
//            WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)GameCore.currentStage][selectedIndex]];

//            //First module added || in case of empty queue
//            if (previousModule == null || current.Count <= 0)
//            {
//                previousModule = currentModule;
//                current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                moduleCounter++;
//            }

//            else
//            {
//                //In case there's the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
//                if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection && previousModule.type != currentModule.type)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//                //In case there isn't the possiblity of spawn a different type of module(EX: prev = VOID, next = SIMPLE_JUMP)
//                else if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//            }
//        }

//    }

//    private IEnumerator FillQueue()
//    {
//        if (current.Count < ListOfIndex[(int)GameCore.currentStage].Count / 2)
//        {


//            //Debug.Log("current length: " + current.Count + "  List of index length: " + ListOfIndex[(int)GameCore.currentStage].Count / 2);
//            int selectedIndex = Random.Range(0, (ListOfIndex[(int)GameCore.currentStage].Count));

//            // ListOfIndex = List with all the modules ID's
//            // GameCore.currentStage = Representation of the current zone
//            // selectedIndex = Random number to select a modlue inside the corresponding zone
//            // ListOfIndex[(int)GameCore.currentStage][selectedIndex] = ID of a module
//            WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)GameCore.currentStage][selectedIndex]];

//            if (previousModule == null)
//            {
//                previousModule = currentModule;
//                current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                moduleCounter++;

//            }

//            else
//            {
//                if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection && previousModule.type != currentModule.type)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//                else if (!current.Contains(ListOfIndex[(int)GameCore.currentStage][selectedIndex])
//                && previousModule.endConnection == currentModule.beginConnection)
//                {
//                    previousModule = currentModule;
//                    current.Enqueue(ListOfIndex[(int)GameCore.currentStage][selectedIndex]);
//                    moduleCounter++;
//                }
//            }

//        }


//        yield return StartCoroutine(FillQueue());
//    }
//}
