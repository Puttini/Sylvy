using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Income : MonoBehaviour
{
	public float income;
	public float period;
	public float duration;
	float currentDuration;
	float lastUpdate;
	float leftOver;

	// Use this for initialization
	void Start ()
	{
		currentDuration = 0;
		lastUpdate = Main.time();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float currentTime = Main.time();
		if( currentTime - lastUpdate >= period )
		{
			currentDuration += period;
			lastUpdate += period;

			leftOver += income - (int)( income );
			int x = (int)leftOver;
			Main.get().money += (int)income + x;
			leftOver -= x;
		}

		if( duration > 0 && currentDuration >= duration )
			GameObject.Destroy( this );
	}
}
