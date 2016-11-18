using UnityEngine;
using System.Collections;

public class AuxLib {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
public struct DualSprite
{
    public Sprite day;
    public Sprite night;
    public int sortOrder;
}