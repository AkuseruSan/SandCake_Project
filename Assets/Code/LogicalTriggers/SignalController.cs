using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalController : MonoBehaviour {

    public List<SunTrigger> triggers;
	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Signal Controller State: " + IsActive());
	}

    bool IsActive()
    {
        foreach (SunTrigger t in triggers)
        {
            if (!t.IsActive()) return false;
        }

        return true;
    }
}
