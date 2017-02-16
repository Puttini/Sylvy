using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateIsometricToolkit;
using Assets.UltimateIsometricToolkit.Scripts.Core;

public class GridManager : MonoBehaviour
{
	public GameObject ground;
	public GameObject sapin;
	public int size;

	// Use this for initialization
	void Start ()
	{
		for( int i = 0 ; i < size ; ++i )
		{
			for( int j = 0 ; j < size ; ++j )
				createObject(i, j, ground);
		}

		createObject( 5, 7, sapin );


	}

	public void createObject( float i, float j, GameObject prefab )
	{
		GameObject obj = GameObject.Instantiate( prefab );
		IsoTransform iso = obj.GetComponent<IsoTransform>();
		iso.Position = new Vector3(i+1, iso.Position.y, j+1);
		obj.transform.parent = this.transform;
	}

	public int getSize() { return size; }
}
