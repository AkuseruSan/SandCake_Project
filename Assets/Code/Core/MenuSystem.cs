using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuSystem : MonoBehaviour {

    public enum CamTravelMode { IN, OUT }
    private CamTravelMode camTravelMode = CamTravelMode.OUT;

    [Header("Power Up Panel Attributes")]
    public RectTransform powerUpPopup;
    public Text powerUpInfoText;
    public Text powerUpNameText;
    public Text powerUpCostText;
    public Button powerUpBuyButton;

    [Space(20)]

    public RectTransform playPopup;
    public RectTransform noEnergyPopup;
    public GameObject energyText;

    private uint currentPlayCost;

    public GameObject[] startPointButtons = new GameObject[DataManager.SPAWN_POINTS];
    public GameObject bossPointButton;

    public GameObject title;
    private Animator titleAnimator;

    RaycastHit2D hit;

    public float minOrthoSize, maxOrthoSize;

    private Vector3 originCameraPosition;
	// Use this for initialization
	void Start () {

        currentPlayCost = 0;
        minOrthoSize = Camera.main.orthographicSize;

        titleAnimator = title.GetComponent<Animator>();

        powerUpPopup.gameObject.SetActive(false);
        playPopup.gameObject.SetActive(false);
        noEnergyPopup.gameObject.SetActive(false);

        originCameraPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        
        energyText.GetComponent<TextMesh>().text = System.Convert.ToString(DataManager.Instance.playerData.energy);
	}

    //Used on UI Buttons events.
    public void CheckStartPointButtonAvailable(GameObject btn)
    {
        uint unlockedPoints = DataManager.Instance.playerData.unlockedSpawnPoints;

        for (uint i = 0; i < startPointButtons.Length; i++)
        {
            if (i < unlockedPoints)
                startPointButtons[i].GetComponent<Button>().interactable = true;
            else startPointButtons[i].GetComponent<Button>().interactable = false;

            if (unlockedPoints == startPointButtons.Length + 1) bossPointButton.GetComponent<Button>().interactable = true;
            else bossPointButton.GetComponent<Button>().interactable = false;

            if (btn == startPointButtons[i])
            {
                btn.transform.localScale = new Vector3(2.5f, 2.5f, 1);
                DataManager.Instance.currentSpawnPoint = i;

                //Esto es una marranada, depende de la jerarquia de gameobjects de la UI
                currentPlayCost = System.Convert.ToUInt32(btn.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text);
            }
            else if (btn == bossPointButton)
            {
                btn.transform.localScale = new Vector3(2.5f, 2.5f, 1);
                DataManager.Instance.currentSpawnPoint = (uint)WorldConstructor.Stage.Z_BOSS;

                //Esto es una marranada, depende de la jerarquia de gameobjects de la UI
                currentPlayCost = System.Convert.ToUInt32(btn.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text);

                startPointButtons[i].transform.localScale = new Vector3(2, 2, 1);
            }
            else
            {
                startPointButtons[i].transform.localScale = new Vector3(2, 2, 1);
                bossPointButton.transform.localScale = new Vector3(2, 2, 1);
            }
        }
    }

    private RaycastHit2D GetHit()
    {

#if UNITY_ANDROID
        if(Input.touchCount > 0)
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)), Vector3.forward, Mathf.Infinity);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), Vector3.forward, Mathf.Infinity);
#endif
        if (hit)
            return hit;

        return new RaycastHit2D();
    }

    public void SetCameraToOrigin()
    {
        camTravelMode = CamTravelMode.OUT;
        StopCoroutine("MoveAndLookAt");
        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, originCameraPosition, CamTravelMode.OUT, 3, true));
        titleAnimator.SetBool("stay", true);
    }

    private IEnumerator MoveAndLookAt(Vector3 origin, Vector3 target, CamTravelMode mode, float speed, bool scalable)
    {
        float t = 0;

        float camOrthoScale = Camera.main.orthographicSize;
        while (t < 1)
        {
            Debug.Log("Traveling Camera . . ."+t);

            t += Time.deltaTime*speed;
            t = Mathf.Clamp(t, 0, 1);

            Camera.main.transform.position = new Vector3(Mathf.Lerp(origin.x, target.x, t), Mathf.Lerp(origin.y, target.y, t), origin.z);
            if (scalable)
            {
                if (mode == CamTravelMode.IN)
                    Camera.main.orthographicSize = Mathf.Lerp(camOrthoScale, maxOrthoSize, t);
                else if (mode == CamTravelMode.OUT)
                    Camera.main.orthographicSize = Mathf.Lerp(camOrthoScale, minOrthoSize, t);
            }
            yield return Camera.main;
        }

        Debug.Log("Camera Travel Done!");
        yield return 0;
    }

    public void BuyPowerUp(GameObject caller)
    {
        DataManager.Instance.playerData.activePowerUps[(int)caller.GetComponent<BuyPowerUpButton>().powerUpID] = true;
        DataManager.Instance.playerData.energy -= System.Convert.ToUInt32(powerUpCostText.text);
        DataManager.Instance.SaveData();

        SetCameraToOrigin();
        powerUpPopup.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (DataManager.Instance.playerData.energy < currentPlayCost)
        {
            noEnergyPopup.gameObject.SetActive(true);
        }
        else
        {
            DataManager.Instance.playerData.energy -= currentPlayCost;
            CoreSceneManager.Instance.SwitchScene(CoreSceneManager.SceneID.GAME);
        }
    }

    private void Update()
    {
        //Update energy text value
        energyText.GetComponent<TextMesh>().text = System.Convert.ToString(DataManager.Instance.playerData.energy);

        bool input = false;

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            input = true;
        }
        else input = false;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            input = true;
        }
        else input = false;
#endif
        if (input)
        { 

            RaycastHit2D hit = GetHit();

            if (hit)
            {
                titleAnimator.SetTrigger("exit");
                titleAnimator.SetBool("stay", false);

                if (hit.transform.GetComponent<Button2D>() != null)
                {

                    Button2D btn = hit.transform.GetComponent<Button2D>();

                    if (btn.type == Button2D.ButtonType.POWER_UP && camTravelMode == CamTravelMode.OUT)
                    {
                        camTravelMode = CamTravelMode.IN;
                        StopCoroutine("MoveAndLookAt");
                        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, btn.cameraTarget.position, CamTravelMode.IN, 3, true));

                        uint currentPowerUpCost = 0;

                        //Filling Popup depending on every power-up
                        switch (btn.powerUpID)
                        {
                            case DataManager.PowerUpID.BARRIER:
                                {
                                    powerUpInfoText.text = "Protect your wolf with a special barrier! For a game, it will protect you from any projectile or obstacles like rocks and fire. ";
                                    powerUpNameText.text = "Barrier";
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.BARRIER);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.BARRIER);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.BARRIER;
                                }
                                break;
                            case DataManager.PowerUpID.DOUBLE_JUMP:
                                {
                                    powerUpInfoText.text = "Give your wolf the ability to double jump in your next game";
                                    powerUpNameText.text = "Double Jump";
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.DOUBLE_JUMP);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.DOUBLE_JUMP);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.DOUBLE_JUMP;
                                }
                                break;
                            case DataManager.PowerUpID.REVIVE:
                                {
                                    powerUpInfoText.text = "Let's take a second opportunity if the wolf's stamina it's over. Come back to life and continue your game! ";
                                    powerUpNameText.text = "Revive";
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.REVIVE);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.REVIVE);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.REVIVE;
                                }
                                break;
                            case DataManager.PowerUpID.PAINT_BOOST:
                                {
                                    powerUpInfoText.text = "Enlarge your painted area! This powerup will gives you a bigger area when you paint, it lasts for one game";
                                    powerUpNameText.text = "Paint Boost";
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.PAINT_BOOST);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.PAINT_BOOST);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.PAINT_BOOST;
                                }
                                break;
                            case DataManager.PowerUpID.STAMINA_BOOST:
                                {
                                    powerUpInfoText.text = "Get your stamina bar twice as big, enlarge it and get the double stamina for a game";
                                    powerUpNameText.text = "Stamina Boost";
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.STAMINA_BOOST);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.STAMINA_BOOST);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.STAMINA_BOOST;
                                }
                                break;
                            default:
                                break;
                        }

                        if(DataManager.Instance.playerData.energy < currentPowerUpCost || DataManager.Instance.playerData.activePowerUps[(int)powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID] == true)
                        {
                            powerUpBuyButton.interactable = false;
                        }
                        else
                        {
                            powerUpBuyButton.interactable = true;
                        }

                        powerUpPopup.gameObject.SetActive(true);
                    }
                    else if(btn.type == Button2D.ButtonType.PLAY && camTravelMode == CamTravelMode.OUT)
                    {
                        camTravelMode = CamTravelMode.IN;
                        StopCoroutine("MoveAndLookAt");
                        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, btn.cameraTarget.position, CamTravelMode.IN, 3, false));
                        playPopup.gameObject.SetActive(true);
                        CheckStartPointButtonAvailable(startPointButtons[0]);
                    }

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && CoreSceneManager.Instance.currentScene.buildIndex == (int)CoreSceneManager.SceneID.MENU)
        {
            Debug.Log("Button Works");
            Application.Quit();
        }
    }
}
