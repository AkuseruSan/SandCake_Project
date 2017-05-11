using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private RaycastHit2D GetHit()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)), Vector3.forward, Mathf.Infinity);

        if (hit)
        {
            return hit;
        }


        return new RaycastHit2D();
    }

    private void FixedUpdate()
    {
        if(Input.touchCount > 0)
        {
            //if(GetHit().transform.GetComponent<Button>() != null)
            //{

            //}
        }
    }
}
