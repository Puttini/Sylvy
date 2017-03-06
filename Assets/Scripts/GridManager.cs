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

	float lastUpdate;

	void Start()
	{
		instance = this;
		lastUpdate = Main.time ();
		size = 0;
		grid = new Case[0,0];
	}

	void Update()
	{
		float dt = 2; // Temps entre chaque calcul (en s, sans prendre en compte le timeScale)
		float t = Main.time ();
		if (t - lastUpdate >= dt)
		{
			updateCases ();
			lastUpdate += dt;
		}
	}

	// Use this for initialization
	public void init()
	{
		for( int i = 0 ; i < size ; ++i )
		{
			for( int j = 0 ; j < size ; ++j )
				createObject(i, j, ground);
		}

		grid = new Case[size, size];
		for( int i = 0; i < size; ++i )
		{
			for( int j = 0; j < size; ++j )
				grid[ i, j ] = new Case( i, j );
		}

		lastUpdate = Main.time ();
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
		return (obj != null);
	}

	public void placeObject( GameObject obj, int i, int j )
	{
		IsoTransform iso = obj.GetComponent<IsoTransform>();
		iso.Position = new Vector3(i+1 + iso.Position.x, iso.Position.y, j+1 + iso.Position.z);
		obj.transform.SetParent( this.transform );
	}

	public bool cut( int i, int j )
	{
		GameObject souche = grid[ i, j ].cut();
		if( souche != null )
		{
			souche.transform.SetParent( this.transform );
			return true;
		}

		return false;
	}

	public bool uproot( int i, int j )
	{
		return grid [i, j].uproot();
	}

	public int getSize() { return size; }

	public void reset()
	{
		foreach( Transform obj in transform )
			GameObject.Destroy( obj.gameObject );
		grid = null;
	}

	void updateCases()
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
				grid [i, j].update();
		}

		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
				grid [i, j].finishUpdate();
		}
	}

	public Case getCase( int i, int j )
	{
		if (i < 0 || i >= size || j < 0 || j >= size)
			return null;
		else
			return grid[i, j];
	}

	public bool addProperties( int x, int y, float h, float l, float f )
	{
		Case c = getCase( x, y );
		if ( c != null )
		{
			c.addProperties( h, l, f );
			return true;
		}
		else
			return false;
	}
}
