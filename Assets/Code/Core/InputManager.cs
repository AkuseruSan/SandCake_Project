using UnityEngine;
using System.Collections;

//Singleton Controller
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
#if UNITY_ANDROID
        foreach (Touch t in Input.touches)
        {
            posRef = Camera.main.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, 0));
            posRef.z = -5;
            return true;
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            posRef = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            posRef.z = -5;

            return true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameCore.Instance.playerController.Jump();

            return true;
        }
#endif
        return false;
    }
}
