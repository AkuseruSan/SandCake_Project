using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour {

    public List<DualSprite> parallaxSprites;
	// Use this for initialization
	void Start () {
        CreateParallaxLayers();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateParallaxLayers()
    {
        for(int i = 0; i < parallaxSprites.Count; i++)
        {
            GameObject go = new GameObject("ParallaxLayer " + i);
            go.transform.parent = this.transform;
            go.AddComponent<ParallaxLayerBehaviour>();

            GameObject goDay = new GameObject("DayLayer");
            goDay.AddComponent<SpriteRenderer>().sprite = parallaxSprites[i].day;
            goDay.transform.parent = go.transform;

            GameObject goNight = new GameObject("NightLayer");
            goNight.AddComponent<SpriteRenderer>().sprite = parallaxSprites[i].night;
            goNight.GetComponent<SpriteRenderer>().sortingOrder = parallaxSprites[i].sortOrder;
            goNight.transform.parent = go.transform;
        }
    }
}
