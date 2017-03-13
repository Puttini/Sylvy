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
				obj.AddComponent<AssignedPlant>().set(p);
				obj.transform.SetParent (GridManager.get ().transform);
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

	public bool uproot()
	{
		for( int i = 0; i < 4; ++i )
		{
			if( objs[ i ] != null )
			{
				Uprootable u = objs[ i ].GetComponent<Uprootable>();
				if( u != null )
				{
					if (u.uproot ())
					{
						GameObject.Destroy (objs [i]);
						return true;
					}
					else
						return false;
				}
			}
		}
		return false;
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

	public void updateCase( Case c )
	{
		for( int i = 0 ; i < 4 ; ++i )
		{
			if (objs [i] != null)
			{
				CaseActor ca = objs [i].GetComponent< CaseActor > ();
				if (ca != null)
					ca.updateCase( c );
			}
		}
	}

	public void destroyPlants()
	{
		for (int i = 0; i < 4; ++i)
		{
			if (objs [i] != null)
			{
				GameObject.Destroy( objs[i] );
				objs[i] = null;
			}
		}
	}

	public void destroyPlants( float p )
	{
		for (int i = 0; i < 4; ++i)
		{
			if (objs [i] != null)
			{
				if ( Main.random() >= p )
				{
					GameObject.Destroy( objs[i] );
					objs[i] = null;
				}
			}
		}
	}

	public string getTxt()
	{
		string txt = "";
		for( int i = 0 ; i < 4 ; ++i )
		{
			if ( objs[i] != null )
				txt += " - " + objs[i].name + "\n";
		}
		return txt;
	}

	public bool removeObject( GameObject o )
	{
		for( int i = 0 ; i < 4 ; ++i )
		{
			if (objs[i] == o )
			{
				objs[i] = null;
				GameObject.Destroy( o );
				return true;
			}
		}
		return false;
	}

	public void addToDico( Dictionary<string,float> dico )
	{
		for( int i = 0 ; i < 4 ; ++i )
		{
			if ( objs[i] != null )
			{
				AssignedPlant p = objs[i].GetComponent<AssignedPlant>();
				Mort m = objs[i].GetComponent<Mort>();
				if ( p != null && m == null )
				{
					string n = p.get().theName;
					if ( dico.ContainsKey(n) )
						dico[n] += p.get().diversImpact;
					else
						dico[n] = 1;
				}
			}
		}
	}
}
