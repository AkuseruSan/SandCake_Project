using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public struct PlayerData
{
    public uint unlockedSpawnPoints;
    //public uint currentSpawnPoint;
    public uint energy;
    public bool[] activePowerUps;
    public float activeMultiplier;
    public uint gameComplete;// 0 - FIRST TIME PLAY / 1 - PLAYED BEFORE / 2 - GAME COMPLETE
}

public class DataManager : MonoBehaviour {

    public enum PowerUpID { BARRIER = 0, DOUBLE_JUMP = 1, REVIVE = 2, PAINT_BOOST = 3, STAMINA_BOOST = 4 }
    public enum PowerUpCost { BARRIER = 100, DOUBLE_JUMP = 5, REVIVE = 2500, PAINT_BOOST = 1000, STAMINA_BOOST = 4500 }// Precio de los powerups

    public static DataManager Instance { get; private set; }
    public PlayerData playerData;

    public bool godMode;
    [HideInInspector] public uint currentSpawnPoint;

    #region Constant Data
    public const uint MAX_ENERGY = 10000;
    public const uint SPAWN_POINTS = 6;
    public const uint POWERUP_COUNT = 5;
    public const char CONTROL_CHAR = '|';
    public const string DATA_KEY = "EltaGameData";

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

        currentSpawnPoint = 0;
        StartDataManager();

        if(godMode)
        {
            
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (godMode)
        {
            //DEBUGGER
            if (Input.GetKeyDown(KeyCode.X))
            {
                DeleteData();
                Debug.Log("[ALL GAME DATA HAS BEEN DELETED]");
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                playerData.energy += 100;
                Debug.Log("ENERGY: " + playerData.energy);
                SaveData();
            }

            //GODMODE FILE
            else if (Input.GetKeyDown(KeyCode.G))
            {
                playerData.gameComplete = 2;
                playerData.energy = MAX_ENERGY;
                playerData.unlockedSpawnPoints = SPAWN_POINTS;
                //playerData.currentSpawnPoint = 0;
                playerData.activeMultiplier = 1f;
                playerData.activePowerUps = new bool[POWERUP_COUNT];

                for (int i = 0; i < playerData.activePowerUps.Length; i++)
                {
                    playerData.activePowerUps[i] = true;
                }

                SaveData();
            }
        }
	}

    //EVENTS
    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void StartDataManager()
    {
        if (!PlayerPrefs.HasKey(DATA_KEY))
        {
            //INITIALIZE BASIC STRUCT
            playerData.gameComplete = 0;
            playerData.energy = 0;
            playerData.unlockedSpawnPoints = 1;
            //playerData.currentSpawnPoint = 0;
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

    private void LoadData()
    {
        int index = 0;
        string data = PlayerPrefs.GetString(DATA_KEY);

        string[] load = data.Split(CONTROL_CHAR);

        Debug.Log("Data String Params: " + load.Length);

        playerData.gameComplete = System.Convert.ToUInt32(load[index++]);
        playerData.energy = System.Convert.ToUInt32(load[index++]);
        playerData.unlockedSpawnPoints = System.Convert.ToUInt32(load[index++]);
        //playerData.currentSpawnPoint = System.Convert.ToUInt32(load[index++]);
        playerData.activeMultiplier = (float)System.Convert.ToDouble(load[index++]);

        playerData.activePowerUps = new bool[POWERUP_COUNT];

        for (int i = 0; i < playerData.activePowerUps.Length; i++)
        {
            if (load[index + i] == "0")
                playerData.activePowerUps[i] = false;
            else if (load[index + i] == "1")
                playerData.activePowerUps[i] = true;
            else Debug.Log("[Error on Game Data Load!]");
        }

        //Debug
        string str = "Loaded Data:\nGame Complete: " + playerData.gameComplete + "\nEnergy: " + playerData.energy + "\nUnlocked Spawn Points: " + playerData.unlockedSpawnPoints + "\nActive Multiplier: " + playerData.activeMultiplier + "\n";

        for (int i = 0; i < playerData.activePowerUps.Length; i++)
            str += "PowerUp " + i + ": " + playerData.activePowerUps[i] + "\n";

        Debug.Log(str);
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey(DATA_KEY);
        SceneManager.LoadScene(0);
        StartDataManager();
    }

    public void SaveData()
    {
        //playerData.currentSpawnPoint = System.Convert.ToUInt16(Mathf.Clamp(playerData.currentSpawnPoint, 0, SPAWN_POINTS - 1));
        playerData.unlockedSpawnPoints = System.Convert.ToUInt16(Mathf.Clamp(playerData.unlockedSpawnPoints, 1, SPAWN_POINTS));
        playerData.energy = System.Convert.ToUInt32(Mathf.Clamp(playerData.energy, 0, MAX_ENERGY));

        string save = playerData.gameComplete.ToString() + CONTROL_CHAR +
            playerData.energy + CONTROL_CHAR +
            playerData.unlockedSpawnPoints + CONTROL_CHAR +
            //playerData.currentSpawnPoint + CONTROL_CHAR +
            playerData.activeMultiplier;

        foreach(bool b in playerData.activePowerUps)
        {
            save += CONTROL_CHAR + (b ? "1" : "0");
        }

        Debug.Log("[SAVED GAME DATA] Encoded: [ "+save+" ]");

        PlayerPrefs.SetString(DATA_KEY, save);

    }
}
