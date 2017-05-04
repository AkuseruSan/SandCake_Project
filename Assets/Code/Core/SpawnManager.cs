using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1 , Z_2, Z_3, Z_BOSS };

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    Queue current;
    public Stage currentStage;

    [Space(20)]
    [Header("[World Dictionary Lists]")]
    public List<WorldModuleData> WorldModuleList;

    public Dictionary<uint, WorldModuleData> WorldModuleDictionary;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < WorldModuleList.Count - 1; ++i)
        {
           
        }
        
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
