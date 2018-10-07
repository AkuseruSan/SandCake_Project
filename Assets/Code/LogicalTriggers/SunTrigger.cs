using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class SunTrigger : MonoBehaviour {

    private bool isActive;
    IEnumerator Activator()
    {
        isActive = true;
        yield return new WaitForSeconds(1f);
        isActive = false;
    }

	// Use this for initialization
	void Start () {
        isActive = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Sun Trigger State: " + isActive);
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Depth")
        {
            StartCoroutine(Activator());
        }
    }

    public bool IsActive() { return isActive; }
}
