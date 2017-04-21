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

        //PlayerPrefs.DeleteAll();
        StartDataLoader();

    }
    void Start () {
        DontDestroyOnLoad(this);

        //SaveData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadScene()
    {

    }
    void StartDataLoader()
    {
        if (!PlayerPrefs.HasKey("EltaGameData"))
        {
            //INITIALIZE BASIC STRUCT
            playerData.gameComplete = 0;
            playerData.energy = 0;
            playerData.unlockedSpawnPoints = 1;
            playerData.currentSpawnPoint = 0;
            playerData.activeMultiplier = 1f;
            playerData.activePowerUps = new List<bool> { false, false, false, false, false };// 5 Power Ups

            SaveData();
        }
        else LoadData();
    }
    private void LoadData()
    {
        int index = 0;
        string data = PlayerPrefs.GetString("EltaGameData");

        string[] load = data.Split('|');

        Debug.Log("Data String Params: " + load.Length);

        playerData.gameComplete = System.Convert.ToUInt32(load[index++]);
        playerData.energy = System.Convert.ToUInt32(load[index++]);
        playerData.unlockedSpawnPoints = System.Convert.ToUInt32(load[index++]);
        playerData.currentSpawnPoint = System.Convert.ToUInt32(load[index++]);
        playerData.activeMultiplier = (float)System.Convert.ToDouble(load[index++]);

        playerData.activePowerUps = new List<bool> { false, false, false, false, false };

        for (int i = 0; i < playerData.activePowerUps.Count; i++)
        {
            if (load[index+i] == "0")
                playerData.activePowerUps[i] = false;
            else if (load[index+i] == "1")
                playerData.activePowerUps[i] = true;
            else Debug.Log("[Error on Game Data Load!]");
        }

        //Debug
        string str = "Loaded Data:\nGame Complete: " + playerData.gameComplete + "\nEnergy: " + playerData.energy + "\nUnlocked Spawn Points: " + playerData.unlockedSpawnPoints + "\nCurrent Spawn Point: " + playerData.currentSpawnPoint + "\nActive Multiplier: " + playerData.activeMultiplier + "\n";

        for (int i = 0; i < playerData.activePowerUps.Count; i++)
            str += "PowerUp " + i + ": " + playerData.activePowerUps[i] + "\n";

        Debug.Log(str);
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

        Debug.Log(save);

        PlayerPrefs.SetString("EltaGameData", save);

    }

}
