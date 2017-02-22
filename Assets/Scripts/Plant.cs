using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
	public GameObject sprite;
	public GameObject previewPrefab;
	public GameObject prefab0;
	public GameObject prefab1;
	public GameObject prefab2;
	public GameObject prefab3;

	public int layer;
	public int cost;


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

	public int points;
}
