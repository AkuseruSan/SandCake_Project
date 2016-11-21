using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour {

    public static ParallaxController Instance { get; private set; }

    public enum ParallaxLayerOrder { BACK, FRONT }

    public float parallaxLayersWidthScale;
    public float parallaxLayersHeightScale;

    public float parallaxMinSpeed;
    public float parallaxMaxSpeed;

    public List<DualTexture> parallaxLayers;

    private int parallaxLayerMin = 1;
    private int parallaxLayerMax = 2;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

	// Use this for initialization
	void Start ()
    {
        parallaxLayersWidthScale = Mathf.Clamp(parallaxLayersWidthScale, 1, 100);
        parallaxLayersHeightScale = Mathf.Clamp(parallaxLayersHeightScale, 1, 100);

        CreateParallaxLayers();

        //Rescale
        Rescale();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void Rescale()
    {
        float h = (Camera.main.orthographicSize * 2.0f);
        float w = (h * Screen.width / Screen.height) * parallaxLayersWidthScale;

        transform.localScale = new Vector3(w, h * parallaxLayersHeightScale, 1f);
    }

    void CreateParallaxLayers()
    {
        for (int i = 0; i < parallaxLayers.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = this.transform;

            if (parallaxLayers[i].order == ParallaxLayerOrder.BACK)
            {
                go.name = "Parallax_BackLayer " + i;
                go.transform.localPosition = new Vector3(0, 0, AuxLib.Map(i, 0, parallaxLayers.Count, parallaxLayerMin, parallaxLayerMax));
            }
            else
            {
                go.name = "Parallax_FrontLayer " + i;
                go.transform.localPosition = new Vector3(0, 0, AuxLib.Map(i, 0, parallaxLayers.Count, -parallaxLayerMin, -parallaxLayerMax));
            }

            GameObject goDay = GameObject.CreatePrimitive(PrimitiveType.Quad);
            goDay.name ="DayLayer";
            goDay.transform.parent = go.transform;
            goDay.transform.localPosition = Vector3.zero;
            Destroy(goDay.GetComponent<MeshCollider>());
            goDay.GetComponent<MeshRenderer>().material = Resources.Load("Materials/MAT_ParallaxLayer") as Material;
            goDay.GetComponent<MeshRenderer>().material.mainTexture = parallaxLayers[i].day;
            goDay.layer = GameCore.DAY_LAYER;
            goDay.AddComponent<ParallaxLayerBehaviour>().movementSpeed = SetParallaxLayerSpeed(i);

            GameObject goNight = GameObject.CreatePrimitive(PrimitiveType.Quad);
            goNight.name = "NightLayer";
            goNight.transform.parent = go.transform;
            goNight.transform.localPosition = Vector3.zero;
            Destroy(goNight.GetComponent<MeshCollider>());
            goNight.GetComponent<MeshRenderer>().material = Resources.Load("Materials/MAT_ParallaxLayer") as Material;
            goNight.GetComponent<MeshRenderer>().material.mainTexture = parallaxLayers[i].night;
            goNight.layer = GameCore.NIGHT_LAYER;
            goNight.AddComponent<ParallaxLayerBehaviour>().movementSpeed = SetParallaxLayerSpeed(i);
        }
    }

    #region Auxiliar Functions
    private float SetParallaxLayerSpeed(int index)
    {
        if (parallaxLayers.Count > 1)
        {
            float aux = parallaxLayers.Count - index;
            float aux2 = AuxLib.Map(aux, 1, parallaxLayers.Count, parallaxMinSpeed, parallaxMaxSpeed);

            return aux2;
        }
        else
            return parallaxMaxSpeed;
    }
    #endregion
}
