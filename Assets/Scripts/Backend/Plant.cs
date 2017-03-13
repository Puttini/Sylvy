using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
	public string theName;
	public string description;

	public GameObject sprite;
	public GameObject previewPrefab;
	public GameObject prefab0;
	public GameObject prefab1;
	public GameObject prefab2;
	public GameObject prefab3;

	public float hmin, hmax;
	public float lmin, lmax;
	public float fmin, fmax;
	public float dmin;
	public float p;

	public int layer;
	public int cost;

	public float diversImpact;

	int nbPrefabs;
	public int getNbPrefabs() { return nbPrefabs; }

	void Start()
	{
		nbPrefabs = 0;
		if( prefab0 != null )
			nbPrefabs++;
		if( prefab1 != null )
			nbPrefabs++;
		if( prefab2 != null )
			nbPrefabs++;
		if( prefab3 != null )
			nbPrefabs++;
	}

	public GameObject getPrefab( int i )
	{
		switch( i )
		{
		case 0:
			return prefab0;
		case 1:
			return prefab1;
		case 2:
			return prefab2;
		case 3:
			return prefab3;
		}
		return null;
	}

	public bool toss( float l, float h, float f )
	{
		if (l < lmin || l > lmax)
			return false;
		if (h < hmin || h > hmax)
			return false;
		if (f < fmin || f > fmax)
			return false;
		if ( GridManager.get().getDiversite() < dmin )
			return false;

		float c = Mathf.Max(f, 0.1f);
		return (p * c > Main.random ());
	}
}
