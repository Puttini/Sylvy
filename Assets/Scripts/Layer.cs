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
				obj.AddComponent<AssignedPlant> ().set(p);
				objs[i] = obj;
				nbObjs++;
				return obj;
			}
		}
		return null;
	}

	public GameObject cut()
	{
		for( int i = 0; i < 4; ++i )
		{
			if( objs[ i ] != null )
			{
				Cuttable c = objs[ i ].GetComponent<Cuttable>();
				if( c != null )
				{
					GameObject o = c.cut();
					GameObject.Destroy( objs[ i ] );
					objs[ i ] = o;
					return o;
				}
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

	public GameObject popFirstObject()
	{
		for (int i = 0; i < 4; ++i)
		{
			if( objs[ i ] != null )
			{
				GameObject o = objs[ i ];
				objs[ i ] = null;
				return o;
			}
		}

		return null;
	}
}
