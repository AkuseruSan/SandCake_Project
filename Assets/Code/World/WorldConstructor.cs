using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldConstructor : MonoBehaviour {

    private float lastX;

    private WorldModuleData lastModuleQueued;
    private Queue<GameObject> worldModulesQueue = new Queue<GameObject>();

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
        WorldModuleType nextType = NextElementTypeToEnqueue();

        WorldModuleData tempMod = GameCore.Instance.worldModules[nextType][Random.Range(0, GameCore.Instance.worldModules[nextType].Count)];
        lastModuleQueued = tempMod;

        return tempMod;
    }

    WorldModuleType NextElementTypeToEnqueue()
    {
        float rand = Random.Range(0, 5);
        
        WorldModuleType randomType = (WorldModuleType)rand;

        return randomType;
        
    }

    bool CompatibleModules(WorldModuleData modA, WorldModuleData modB)
    {
        if(modA.endConnection == modB.beginConnection) return true; return false;
    }
}
