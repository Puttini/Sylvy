﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using System;

public class ButtonCut : MonoBehaviour
{
	// Singleton class
	static ButtonCut instance;
	public static ButtonCut get() { return instance; }

	bool selected;
	public Texture2D cursor;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		selected = false;
	}

	void Update()
	{
		if( !selected && Input.GetKeyDown( KeyCode.Alpha1 ) )
			select();

		if( selected && Input.GetMouseButtonDown( 0 ) && Input.mousePosition.y >= 150 )
		{
			Vector3 pos = (Vector3)Isometric.CreateXYZfromY (Input.mousePosition, 0);
			if( GridManager.get().cut( (int)( Math.Round( pos.x ) ) - 1, (int)( Math.Round( pos.z ) ) - 1 ) )
			{
				if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift))
					leaveSelection ();
			}
		}
	}
	
	public void select()
	{
		Main.leaveSelection ();

		Cursor.SetCursor( cursor, new Vector2( 40, 1 ), CursorMode.Auto );

		selected = true;

		Main.msgButtonCut();
	}

	public void leaveSelection()
	{
		selected = false;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
