using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour{

    public static InputManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public bool DrawTouch(ref Vector3 posRef)
    {
        foreach (Touch t in Input.touches)
        {
            posRef = Camera.main.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, 0));
            posRef.z = -5;
            return true;
        }

        if (Input.GetMouseButton(0))
        {
            posRef = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            posRef.z = -5;

            return true;
        }

        return false;
    }
}
