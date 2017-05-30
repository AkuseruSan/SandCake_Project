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

public struct ConfigData
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}

public class DataManager : MonoBehaviour {

    public enum PowerUpID { BARRIER = 0, DOUBLE_JUMP = 1, REVIVE = 2, PAINT_BOOST = 3, STAMINA_BOOST = 4 }
    public enum PowerUpCost { BARRIER = 1500, DOUBLE_JUMP = 1200, REVIVE = 3500, PAINT_BOOST = 1100, STAMINA_BOOST = 2500 }// Precio de los powerups

    public static DataManager Instance { get; private set; }
    public PlayerData playerData;
    public ConfigData configData;

    public bool doingTutorial;

    public bool godMode;
    [HideInInspector] public uint currentSpawnPoint;

    #region Constant Data
    public const uint MAX_ENERGY = 10000;
    public const uint SPAWN_POINTS = 6;
    public const uint POWERUP_COUNT = 5;
    public const char CONTROL_CHAR = '|';
    public const string DATA_KEY = "EltaGameData";
    public const string CONFIG_KEY = "EltaGameConfig";

    #endregion
    // Use this for initialization
    void Awake()
    {
        //Visual settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

    }
    void Start () {
        currentSpawnPoint = 0;
        StartDataManager();
        doingTutorial = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;

    }
	
	// Update is called once per frame
	void Update () {

        if((1.0f / Time.deltaTime) < 25f)
        {
            Debug.Log("Frame Drop");
        }

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
        SaveConfig();
    }

    private void StartDataManager()
    {
        if (!PlayerPrefs.HasKey(DATA_KEY))
        {
            //INITIALIZE BASIC STRUCT
            playerData.gameComplete = 0;
            playerData.energy = 0;
            playerData.unlockedSpawnPoints = 1;
            playerData.activeMultiplier = 1f;
            playerData.activePowerUps = new bool[POWERUP_COUNT];

            for(int i = 0; i < playerData.activePowerUps.Length; i++)
            {
                playerData.activePowerUps[i] = false;
            }

            SaveData();
        }
        else LoadData();

        if (!PlayerPrefs.HasKey(CONFIG_KEY))
        {
            SaveConfig();
        }
        else LoadConfig();
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

    public void LoadConfig()
    {
        string data = PlayerPrefs.GetString(CONFIG_KEY);

        string[] load = data.Split(CONTROL_CHAR);

        configData.masterVolume = (float)System.Convert.ToDecimal(load[0]);
        configData.musicVolume = (float)System.Convert.ToDecimal(load[1]);
        configData.sfxVolume = (float)System.Convert.ToDecimal(load[2]);
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey(DATA_KEY);
        SceneManager.LoadScene(0);
        StartDataManager();
    }

    public void SaveData()
    {
        playerData.unlockedSpawnPoints = System.Convert.ToUInt16(Mathf.Clamp(playerData.unlockedSpawnPoints, 1, SPAWN_POINTS));
        playerData.energy = System.Convert.ToUInt32(Mathf.Clamp(playerData.energy, 0, MAX_ENERGY));

        string save = playerData.gameComplete.ToString() + CONTROL_CHAR +
            playerData.energy + CONTROL_CHAR +
            playerData.unlockedSpawnPoints + CONTROL_CHAR +
            playerData.activeMultiplier;

        foreach(bool b in playerData.activePowerUps)
        {
            save += CONTROL_CHAR + (b ? "1" : "0");
        }

        Debug.Log("[SAVED GAME DATA] Encoded: [ "+save+" ]");

        PlayerPrefs.SetString(DATA_KEY, save);

    }

    public void SaveConfig()
    {
        string save = configData.masterVolume.ToString() + CONTROL_CHAR +
            configData.musicVolume + CONTROL_CHAR +
            configData.sfxVolume;

        Debug.Log("[SAVED CONFIG DATA] Encoded: [ " + save + " ]");

        PlayerPrefs.SetString(CONFIG_KEY, save);
    }
}
