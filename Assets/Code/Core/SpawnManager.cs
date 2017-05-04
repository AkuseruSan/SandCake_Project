using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1, Z_2, Z_3, Z_BOSS };

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    private int[] zonesNumber;

    Queue current;
    public Stage currentStage;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    public List<WorldModuleData> WorldModuleList;
    private List<List<uint>> ListOfIndex;

    
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

        for (int i = 0; i < ListOfIndex.Count; ++i)
        {
            for (int j = 0; j < ListOfIndex[i].Count; ++j)
            {
                //Debug.Log("List content in " + i + " " + j + " " + ListOfIndex[i][j]);
            }
        }
    }

    // Use this for initialization
    void Start () {
        WorldModuleList.Clear();        
    }
	
	// Update is called once per frame
	void Update () {

        switch (currentStage)
        {
            case Stage.Z_1:
                break;
            case Stage.Z_2:
                break;
            case Stage.Z_3:
                break;
            case Stage.Z_BOSS:
                break;
        }
    }

   

}
