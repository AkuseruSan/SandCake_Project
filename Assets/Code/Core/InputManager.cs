using UnityEngine;
using System.Collections;

public static class InputManager {

    public static bool DrawTouch(ref Vector3 posRef)
    {
        foreach (Touch t in Input.touches)
        {
            posRef = Camera.main.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, 0));
            posRef.z = -5;
            return true;
        }

        //if(Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width/3)
        //{
        //    posRef = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //    posRef.z = -5;

        //    return true;
        //}

        return false;
    }
}
