using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class SoucheDefaut : MonoBehaviour, Uprootable, CaseActor
{
	float age;
	float uprootCost;
	float size;
	bool isDead;

	public float getAge() { return age; }
	public void setAge( float a ) { age = a; } 
	public void setScale( float scale ) // Utilisée une seule fois
	{
		IsoTransform iso = GetComponent<IsoTransform>();
		iso.Size *= scale;
		iso.Position = new Vector3( iso.Position.x, scale * iso.Position.y, iso.Position.z );
		transform.localScale *= scale;
		size = scale;
	}

	public void setDead( bool dead ) { isDead = dead; }

	public bool uproot()
	{
		int cost = (int)(size * uprootCost);
		if (Main.get ().money >= cost)
		{
			Main.get ().money -= cost;
			return true;
		}
		return false;
	}

	public void setUprootCost( float cost )
	{
		uprootCost = cost;
	}

	public void updateCase( Case c )
	{

	}
}
