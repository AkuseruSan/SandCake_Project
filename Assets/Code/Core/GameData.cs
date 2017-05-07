using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerData
{
    public uint unlockedSpawnPoints;
    public uint currentSpawnPoint;
    public uint energy;
    public bool[] activePowerUps;
    public float activeMultiplier;
    public uint gameComplete;
}

public class GameData : MonoBehaviour {

    public static GameData Instance { get; private set; }
    public PlayerData playerData;

    #region Constant Data
    public const uint MAX_ENERGY = 10000;
    public const uint POWERUP_COUNT = 10;
    public const char CONTROL_CHAR = '|';
    public const string DATA_FILE = "EltaGameData";

    #endregion
    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

    }
    void Start () {

        StartDataManager();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadScene()
    {

    }

    void StartDataManager()
    {
        if (!PlayerPrefs.HasKey(DATA_FILE))
        {
            //INITIALIZE BASIC STRUCT
            playerData.gameComplete = 0;
            playerData.energy = 0;
            playerData.unlockedSpawnPoints = 1;
            playerData.currentSpawnPoint = 0;
            playerData.activeMultiplier = 1f;
            playerData.activePowerUps = new bool[POWERUP_COUNT];

            for(int i = 0; i < playerData.activePowerUps.Length; i++)
            {
                playerData.activePowerUps[i] = false;
            }

            SaveData();
        }
        else LoadData();
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey(DATA_FILE);
    }

    private void LoadData()
    {
        int index = 0;
        string data = PlayerPrefs.GetString(DATA_FILE);

        string[] load = data.Split(CONTROL_CHAR);

        Debug.Log("Data String Params: " + load.Length);

        playerData.gameComplete = System.Convert.ToUInt32(load[index++]);
        playerData.energy = System.Convert.ToUInt32(load[index++]);
        playerData.unlockedSpawnPoints = System.Convert.ToUInt32(load[index++]);
        playerData.currentSpawnPoint = System.Convert.ToUInt32(load[index++]);
        playerData.activeMultiplier = (float)System.Convert.ToDouble(load[index++]);

        playerData.activePowerUps = new bool[POWERUP_COUNT];

        for (int i = 0; i < playerData.activePowerUps.Length; i++)
        {
            if (load[index+i] == "0")
                playerData.activePowerUps[i] = false;
            else if (load[index+i] == "1")
                playerData.activePowerUps[i] = true;
            else Debug.Log("[Error on Game Data Load!]");
        }

        //Debug
        string str = "Loaded Data:\nGame Complete: " + playerData.gameComplete + "\nEnergy: " + playerData.energy + "\nUnlocked Spawn Points: " + playerData.unlockedSpawnPoints + "\nCurrent Spawn Point: " + playerData.currentSpawnPoint + "\nActive Multiplier: " + playerData.activeMultiplier + "\n";

        for (int i = 0; i < playerData.activePowerUps.Length; i++)
            str += "PowerUp " + i + ": " + playerData.activePowerUps[i] + "\n";

        Debug.Log(str);
    }

    void SaveData()
    {
        string save = playerData.gameComplete.ToString() + CONTROL_CHAR +
            playerData.energy + CONTROL_CHAR +
            playerData.unlockedSpawnPoints + CONTROL_CHAR +
            playerData.currentSpawnPoint + CONTROL_CHAR +
            playerData.activeMultiplier;

        foreach(bool b in playerData.activePowerUps)
        {
            save += CONTROL_CHAR + (b ? "1" : "0");
        }

        Debug.Log(save);

        PlayerPrefs.SetString(DATA_FILE, save);

    }

}
