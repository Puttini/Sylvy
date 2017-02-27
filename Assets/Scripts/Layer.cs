using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
	int nbObjs;

	GameObject[] objs;

	public Layer()
	{
		nbObjs = 0;
		objs = new GameObject[4];
		for( int i = 0; i < 4; ++i )
			objs[ i ] = null;
	}

	public GameObject insert( Plant p )
	{
		for( int i = 0; i < 4; ++i )
		{
			if( objs[ i ] == null )
			{
				GameObject prefab = p.getPrefab( i );
				if( prefab == null )
					return null;
				GameObject obj = GameObject.Instantiate( prefab );
				AssignedPlant ap = obj.AddComponent<AssignedPlant> ();
				ap.set(p);
				objs[i] = obj;
				nbObjs++;
				return obj;
			}
		}
		return null;
	}

	public GameObject getFirstObject()
	{
		for (int i = 0; i < 4; ++i)
		{
			if (objs [i] != null)
				return objs [i];
		}

		return null;
	}
}
