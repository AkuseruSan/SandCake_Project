using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuSystem : MonoBehaviour {

    public enum CamTravelMode { IN, OUT }
    private CamTravelMode camTravelMode = CamTravelMode.OUT;

    public RectTransform powerUpPopup;
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

    public void CheckStartPointButtonAvailable(GameObject btn)
    {
        for (uint i = 0; i < startPointButtons.Length; i++)
        {
            if (btn == startPointButtons[i])
            {
                startPointButtons[i].transform.localScale = new Vector3(1.2f, 1.2f, 1);
                DataManager.Instance.currentSpawnPoint = i;
                currentPlayCost = System.Convert.ToUInt32(startPointButtons[i].transform.parent.GetChild(1).GetChild(0).GetComponent<Text>().text);
            }
            else
            {
                startPointButtons[i].transform.localScale = new Vector3(1, 1, 1);
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

    public void StartGame()
    {
        DataManager.Instance.playerData.energy -= currentPlayCost;
        CoreSceneManager.Instance.SwitchScene(CoreSceneManager.SceneID.GAME);
    }

    private void Update()
    {
        bool input = false;
//#if UNITY_ANDROID
//        if (Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//            input = true;
//        }
//        else input = false;
//#endif
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            input = true;
        }
        else input = false;
#endif
        if(input)
        { 
            Debug.Log("YAY!");
            RaycastHit2D hit = GetHit();

            if (hit)
            {
                if (hit.transform.GetComponent<Button2D>() != null)
                {
                    Debug.Log("PENE!");
                    Button2D btn = hit.transform.GetComponent<Button2D>();

                    if (btn.type == Button2D.ButtonType.POWER_UP && camTravelMode == CamTravelMode.OUT)
                    {
                        camTravelMode = CamTravelMode.IN;
                        StopCoroutine("MoveAndLookAt");
                        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, btn.cameraTarget.position, CamTravelMode.IN, 3, true));
                        powerUpPopup.gameObject.SetActive(true);
                    }
                    else if(btn.type == Button2D.ButtonType.PLAY && camTravelMode == CamTravelMode.OUT)
                    {
                        camTravelMode = CamTravelMode.IN;
                        StopCoroutine("MoveAndLookAt");
                        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, btn.cameraTarget.position, CamTravelMode.IN, 3, false));
                        playPopup.gameObject.SetActive(true);
                    }

                }
            }
        }
    }
}
