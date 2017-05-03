using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1 = 20, Z_2 = 30, Z_3 = 40, Z_4 = 50, Z_BOSS = 3 };

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    Queue current;
    public Stage currentStage;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    public List<ListOfWorldDictionaryList> listWorldModuleList;

    public Dictionary<WorldModuleType, List<WorldModuleData>> worldModules;

    // Use this for initialization
    void Start () {

        worldModules = new Dictionary<WorldModuleType, List<WorldModuleData>>();
        InitializeWorldModules();
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
            case Stage.Z_4:
                break;
            case Stage.Z_BOSS:
                break;
        }
    }

   public void InitializeWorldModules()
    {
        foreach(ListOfWorldDictionaryList index in listWorldModuleList)
        {
            foreach (WorldDictionaryList data in index.worldDictionaryList)
            {
                foreach (WorldModuleData mod in data.worldModules)
                {
                    if (!worldModules.ContainsKey(data.type))
                        worldModules.Add(data.type, new List<WorldModuleData>());

                    worldModules[data.type].Add((new WorldModuleData(mod.beginConnection, mod.endConnection, mod.module)));
                }
            }
        }
        
    }

}
