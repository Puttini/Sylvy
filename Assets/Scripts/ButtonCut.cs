using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCut : MonoBehaviour
{
	// Singleton class
	static ButtonCut instance;
	public static ButtonCut get() { return instance; }

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	public void selectCut()
	{
		Main.leaveSelection ();
		//TODO
	}
}
