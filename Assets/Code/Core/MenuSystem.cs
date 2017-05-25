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
    public Image powerUpImage;
    public Button powerUpBuyButton;

    [Space(20)]

    public RectTransform playPopup;
    public RectTransform noEnergyPopup;
    public GameObject energyText;

    private uint currentPlayCost;

    public GameObject[] startPointButtons = new GameObject[DataManager.SPAWN_POINTS];
    public GameObject bossPointButton;

    public float minOrthoSize, maxOrthoSize;

    private Vector3 originCameraPosition;
	// Use this for initialization
	void Start () {

        currentPlayCost = 0;
        minOrthoSize = Camera.main.orthographicSize;

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
                btn.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                DataManager.Instance.currentSpawnPoint = i;

                //Esto es una marranada, depende de la jerarquia de gameobjects de la UI
                currentPlayCost = System.Convert.ToUInt32(btn.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text);
            }
            else if (btn == bossPointButton)
            {
                btn.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                DataManager.Instance.currentSpawnPoint = (uint)WorldConstructor.Stage.Z_BOSS;

                //Esto es una marranada, depende de la jerarquia de gameobjects de la UI
                currentPlayCost = System.Convert.ToUInt32(btn.transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text);
            }
            else
            {
                startPointButtons[i].transform.localScale = new Vector3(1, 1, 1);
                bossPointButton.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private RaycastHit2D GetHit()
    {
        RaycastHit2D hit;

//#if UNITY_ANDROID
//        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)), Vector3.forward, Mathf.Infinity);
//#endif
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
#if UNITY_EDITOR
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
                                    powerUpInfoText.text = "Barrier PowerUp!";
                                    powerUpNameText.text = "Barrier";
                                    powerUpImage.color = Color.blue;
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.BARRIER);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.BARRIER);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.BARRIER;
                                }
                                break;
                            case DataManager.PowerUpID.DOUBLE_JUMP:
                                {
                                    powerUpInfoText.text = "Double Jump PowerUp!";
                                    powerUpNameText.text = "Double Jump";
                                    powerUpImage.color = Color.yellow;
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.DOUBLE_JUMP);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.DOUBLE_JUMP);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.DOUBLE_JUMP;
                                }
                                break;
                            case DataManager.PowerUpID.REVIVE:
                                {
                                    powerUpInfoText.text = "Revive PowerUp!";
                                    powerUpNameText.text = "Revive";
                                    powerUpImage.color = Color.green;
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.REVIVE);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.REVIVE);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.REVIVE;
                                }
                                break;
                            case DataManager.PowerUpID.PAINT_BOOST:
                                {
                                    powerUpInfoText.text = "Paint Boost PowerUp!";
                                    powerUpNameText.text = "Paint Boost";
                                    powerUpImage.color = Color.magenta;
                                    currentPowerUpCost = System.Convert.ToUInt32((int)DataManager.PowerUpCost.PAINT_BOOST);
                                    powerUpCostText.text = System.Convert.ToString((int)DataManager.PowerUpCost.PAINT_BOOST);

                                    powerUpBuyButton.GetComponent<BuyPowerUpButton>().powerUpID = DataManager.PowerUpID.PAINT_BOOST;
                                }
                                break;
                            case DataManager.PowerUpID.STAMINA_BOOST:
                                {
                                    powerUpInfoText.text = "Stamina Boost PowerUp!";
                                    powerUpNameText.text = "Stamina Boost";
                                    powerUpImage.color = Color.cyan;
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
    }
}
