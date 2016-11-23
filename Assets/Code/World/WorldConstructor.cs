using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldConstructor : MonoBehaviour {

    private float lastX;

    private WorldModuleData lastModuleQueued;
    private Queue<GameObject> worldModulesQueue = new Queue<GameObject>();

    private Dictionary<WorldModuleType, float> spawnPriority;

	// Use this for initialization
	void Start () {
        lastX = transform.position.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Queue System
        if (worldModulesQueue.Count < 20)
        {
            WorldModuleData nextModule = NextElementToEnqueue();
            if (CompatibleModules(lastModuleQueued, nextModule))
            {
                worldModulesQueue.Enqueue(nextModule.module);
            }
        }

        if (transform.position.x - lastX >= GameCore.Instance.worldConstructorOffsetX)
        {
            lastX = transform.position.x;
            SpawnWorldModule();
        }
    }

    void SpawnWorldModule()
    {
        GameObject go = Instantiate(worldModulesQueue.Dequeue() as GameObject);
        go.transform.position = transform.position;
    }

    WorldModuleData NextElementToEnqueue()
    {
        WorldModuleData tempMod = GameCore.Instance.worldModules[NextListInDictionary()][Random.Range(0, GameCore.Instance.worldModules.Count - 1)];
        lastModuleQueued = tempMod;

        return tempMod;
    }

    WorldModuleType NextListInDictionary()
    {
        return WorldModuleType.VOID;
    }

    bool CompatibleModules(WorldModuleData modA, WorldModuleData modB)
    {
        if(modA.endConnection == modB.beginConnection) return true; return false;
    }
}
