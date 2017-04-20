using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public struct PlayerData
{
    public uint unlockedSpawnPoints;
    public uint currentSpawnPoint;
    public uint energy;
    public List<bool> activePowerUps;
    public float activeMultiplier;
    public uint gameComplete;
}

public class GameData : MonoBehaviour {

    public static GameData Instance { get; private set; }
    public PlayerData playerData;


    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        if (!PlayerPrefs.HasKey("EltaGameData"))
        {
            //INITIALIZE BASIC STRUCT
            playerData.gameComplete = 0;
            playerData.energy = 0;
            playerData.unlockedSpawnPoints = 1;
            playerData.currentSpawnPoint = 0;
            playerData.activeMultiplier = 1f;
            playerData.activePowerUps = new List<bool> { false, false, false, false, false };
        }
        else LoadData();
    }
    void Start () {
        DontDestroyOnLoad(this);

        SaveData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadScene()
    {

    }

    void LoadData()
    {

    }

    void SaveData()
    {
        string save = playerData.gameComplete.ToString() + "|" +
            playerData.energy + "|" +
            playerData.unlockedSpawnPoints + "|" +
            playerData.currentSpawnPoint + "|" +
            playerData.activeMultiplier;

        foreach(bool b in playerData.activePowerUps)
        {
            save += "|" + (b ? 1 : 0);
        }

        save += "!";

        Debug.Log(save);
    }

}
