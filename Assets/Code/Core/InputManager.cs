using UnityEngine;
using System.Collections;

public static class InputManager {

    public static bool JumpTouch()
    {
        foreach (Touch t in Input.touches)
        {
            if(t.position.x < Screen.width / 3)
            {
                Debug.Log("Jumped!");
                return true;
            }
        }

        return false;
    }

    public static bool DrawTouch(ref Vector3 posRef)
    {
        foreach (Touch t in Input.touches)
        {
            if (t.position.x > Screen.width / 3)
            {
                posRef = Camera.main.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, -5));
                Debug.Log("Draw!");
                return true;
            }
        }

        return false;
    }
}
