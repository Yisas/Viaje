﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorResetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = true;		
	}

    void Update()
    {
        Debug.Log(Cursor.visible);
    }
}