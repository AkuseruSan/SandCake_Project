﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button2D : MonoBehaviour {

    public enum ButtonType { POWER_UP, PLAY, SETTINGS }

    public DataManager.PowerUpID powerUpID;

    public ButtonType type;

    public Transform cameraTarget;
	// Use this for initialization
	void Start () {
        if (cameraTarget == null) cameraTarget = this.transform;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
