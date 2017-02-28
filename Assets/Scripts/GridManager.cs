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

	Case[,] grid;


	// Use this for initialization
	void Start ()
	{
		instance = this;

		for( int i = 0 ; i < size ; ++i )
		{
			for( int j = 0 ; j < size ; ++j )
				createObject(i, j, ground);
		}

		grid = new Case[size, size];
		for( int i = 0; i < size; ++i )
		{
			for( int j = 0; j < size; ++j )
				grid[ i, j ] = new Case();
		}
	}

	void createObject( int i, int j, GameObject prefab )
	{
		GameObject obj = GameObject.Instantiate( prefab );
		placeObject( obj, i, j );
	}

	public bool placePlant( int i, int j, Plant p )
	{
		if( i < 0 || i >= size || j < 0 || j >= size )
			return false;

		Case c = grid[i, j];
		GameObject obj = c.insert( p );
		if( obj != null )
		{
			placeObject( obj, i, j );
			return true;
		}
		else
			return false;
	}

	void placeObject( GameObject obj, int i, int j )
	{
		IsoTransform iso = obj.GetComponent<IsoTransform>();
		iso.Position = new Vector3(i+1 + iso.Position.x, iso.Position.y, j+1 + iso.Position.z);
		obj.transform.SetParent( this.transform );
	}

	public bool cut( int i, int j )
	{
		GameObject tronc = grid[ i, j ].cut();
		if( tronc != null )
		{
			tronc.transform.SetParent( this.transform );
			return true;
		}

		return false;
	}

	public int getSize() { return size; }
}
