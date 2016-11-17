using UnityEngine;
using System.Collections;

public class InputManager {

    private InputManager()
    {

    }

    public static InputManager Instance()
    {
        InputManager im = new InputManager();
        return im;
    }
}
