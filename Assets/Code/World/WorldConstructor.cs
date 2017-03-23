using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldConstructor : MonoBehaviour {

    private float lastX;

    private float flowerSpawnCtr;//Counter = ctr

    private WorldModuleData lastModuleQueued;
    private Queue<GameObject> worldModulesQueue = new Queue<GameObject>();


	// Use this for initialization
	void Start () {
        lastX = transform.position.x;


    }

    void Update()
    {
        SpawnFlowers();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
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

        if (transform.position.x - lastX >= GameCore.Instance.worldConstructorSpawnToSpawnDistance)
        {
            lastX = transform.position.x;
            SpawnWorldModule();
        }
    }

    void SpawnFlowers()
    {
        if(flowerSpawnCtr <= 0)
        {
            flowerSpawnCtr = Random.Range(2, 5);
            GameObject go = Instantiate(Resources.Load("Prefabs/Interactable/StaminaFlower_INT"), AuxLib.SetPositionOnRaycastHit2D(new Vector3(transform.position.x -10, 10, 0), "Terrain", Vector2.down, 1), Quaternion.identity) as GameObject;
        }

        flowerSpawnCtr -= Time.deltaTime;
    }

    void SpawnWorldModule()
    {
        GameObject go = Instantiate(worldModulesQueue.Dequeue() as GameObject);
        go.transform.position = transform.position;
        go.transform.localScale = GameCore.Instance.worldModuleScale;
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
