using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1, Z_2, Z_3, Z_BOSS };

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    private int[] zonesNumber;

    Queue current;
    public Stage currentStage = Stage.Z_1;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    public List<WorldModuleData> WorldModuleList;
    private List<List<uint>> ListOfIndex;
    private WorldModuleData previousModule;
    
    public Dictionary<uint, WorldModuleData> WorldModuleDictionary;

    void Awake()
    {
        //Gets the total amount of zones in the enum Stage
        int enumLength = System.Enum.GetNames(typeof(Stage)).Length;

        //Stores the total number of zones
        zonesNumber = new int[enumLength];

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
            WorldModuleList[i].SetID(zonesNumber[(int)WorldModuleList[i].stage]);
            ListOfIndex[(int)WorldModuleList[i].stage].Add(WorldModuleList[i].GetID());
            zonesNumber[(int)WorldModuleList[i].stage]++;
            
            //Puts the ID of the module as key and the module as value
            WorldModuleDictionary.Add(WorldModuleList[i].GetID(), WorldModuleList[i]);
        }

        //Queue start
        current = new Queue();

        while (current.Count < ListOfIndex[(int)currentStage].Count / 2)
        {
            //Debug.Log("current length: " + current.Count + "  List of index length: " + ListOfIndex[(int)currentStage].Count / 2);
            int selectedIndex = Random.Range(0, (ListOfIndex[(int)currentStage].Count));

            WorldModuleData currentModule = WorldModuleDictionary[ListOfIndex[(int)currentStage][selectedIndex]];

            // ListOfIndex = List with all the modules ID's
            // currentStage = Representation of the current zone
            // selectedIndex = Random number to select a modlue inside the corresponding zone

            if(previousModule == null)
            {
                if (!current.Contains(ListOfIndex[(int)currentStage][selectedIndex]))
                {
                    previousModule = currentModule;
                    current.Enqueue(ListOfIndex[(int)currentStage][selectedIndex]);
                }
            }

            else
            {
                if (!current.Contains(ListOfIndex[(int)currentStage][selectedIndex])
                && previousModule.endConnection == currentModule.beginConnection)
                {
                    previousModule = currentModule;
                    current.Enqueue(ListOfIndex[(int)currentStage][selectedIndex]);
                }
            }

        }

        foreach (uint xData in current)
        {
            Debug.Log(xData);
        }
    }

    // Use this for initialization
    void Start () {
        //WorldModuleList.Clear();
    }
	
	// Update is called once per frame
	void Update () {

        if(current.Count == ListOfIndex[(int)currentStage].Count / 2)
        {
            //Debug.Log("Ready to spawn!");
            current.Dequeue();
        }

        while (current.Count < ListOfIndex[(int)currentStage].Count / 2)
        {
            //Debug.Log("Not ready to spawn!");
            int selectedIndex = Random.Range(0, (ListOfIndex[(int)currentStage].Count));

            if (!current.Contains(selectedIndex)) current.Enqueue(selectedIndex);
        }
        
    }

}
