using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateIsometricToolkit;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class GridManager : MonoBehaviour
{
	// Singleton class
	static GridManager instance;
	public static GridManager get() { return instance; }

	public GameObject ground;
	public GameObject sapin;
	public int size;

	// Use this for initialization
	void Start ()
	{
		instance = this;

		for( int i = 0 ; i < size ; ++i )
		{
			for( int j = 0 ; j < size ; ++j )
				createObject(i, j, ground);
		}

		// Tests
		createObject( 5, 7, sapin );
	}

	void createObject( float i, float j, GameObject prefab )
	{
		GameObject obj = GameObject.Instantiate( prefab );
		IsoTransform iso = obj.GetComponent<IsoTransform>();
		iso.Position = new Vector3(i+1, iso.Position.y, j+1);
		obj.transform.parent = this.transform;
	}

	public void placePlant( float i, float j, Plant p )
	{
		//TODO
	}

	public int getSize() { return size; }
}
