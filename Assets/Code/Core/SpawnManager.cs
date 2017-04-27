using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { Z_1, Z_2, Z_3, Z_4, Z_BOSS };

public class SpawnManager : MonoBehaviour {

    Queue current;
    public List<List<Transform>> worldZones;
    public Stage currentStage;

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

    void FillList() {
            
    }
}
