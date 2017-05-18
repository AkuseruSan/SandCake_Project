using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour {

    public enum CamTravelMode { IN, OUT }
    private CamTravelMode camTravelMode = CamTravelMode.OUT;

    public RectTransform powerUpPopup;
    public RectTransform playPopup;
    public GameObject energyText;

    public float lerpSpeed;

    public float minOrthoSize, maxOrthoSize;

    private Vector3 originCameraPosition;
	// Use this for initialization
	void Start () {
        minOrthoSize = Camera.main.orthographicSize;

        powerUpPopup.gameObject.SetActive(false);
        playPopup.gameObject.SetActive(false);

        originCameraPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        
        energyText.GetComponent<TextMesh>().text = System.Convert.ToString(DataManager.Instance.playerData.energy);
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
        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, originCameraPosition, CamTravelMode.OUT));
    }

    private IEnumerator MoveAndLookAt(Vector3 origin, Vector3 target, CamTravelMode mode)
    {
        float t = 0;

        while (t < 1)
        {
            Debug.Log("Traveling Camera . . ."+t);

            t += Time.deltaTime*lerpSpeed;
            t = Mathf.Clamp(t, 0, 1);

            Camera.main.transform.position = new Vector3(Mathf.Lerp(origin.x, target.x, t), Mathf.Lerp(origin.y, target.y, t), origin.z);

            if (mode == CamTravelMode.IN)
                Camera.main.orthographicSize = Mathf.Lerp(minOrthoSize, maxOrthoSize, t);
            else if (mode == CamTravelMode.OUT)
                Camera.main.orthographicSize = Mathf.Lerp(maxOrthoSize, minOrthoSize, t);

            yield return Camera.main;
        }

        Debug.Log("Camera Travel Done!");
        yield return 0;
    }

    public void StartGame()
    {
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
                        StartCoroutine(MoveAndLookAt(Camera.main.transform.position, btn.cameraTarget.position, CamTravelMode.IN));
                        powerUpPopup.gameObject.SetActive(true);
                    }
                    else if(btn.type == Button2D.ButtonType.PLAY)
                    {
                        playPopup.gameObject.SetActive(true);
                    }

                }
            }
        }
    }
}
