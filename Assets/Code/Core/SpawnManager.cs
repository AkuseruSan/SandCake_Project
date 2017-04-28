using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1, Z_2, Z_3, Z_4, Z_BOSS };

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    Queue current;
    public List<List<WorldDictionaryList>> worldModulesList;
    public Stage currentStage;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    //public List<WorldDictionaryList> worldModulesList;

    public Dictionary<WorldModuleType, List<WorldModuleData>> worldModules;

    // Use this for initialization
    void Start () {
		
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
            default:
                break;
        }
    }

   public void InitializeWorldModules(Stage currentStage)
    {

        foreach (WorldDictionaryList data in worldModulesList[(int)currentStage])
        {
            foreach (WorldModuleData mod in data.worldModules)
            {
                if (!worldModules.ContainsKey(data.type))
                    worldModules.Add(data.type, new List<WorldModuleData>());

                worldModules[data.type].Add((new WorldModuleData(mod.beginConnection, mod.endConnection, mod.module)));
            }
        }
    }

    void UpdateWorldManager()
    {
        GameCore.Instance.worldManager.position = new Vector3(GameCore.Instance.cameraSystemTransform.position.x, 0, 0);
    }


}
