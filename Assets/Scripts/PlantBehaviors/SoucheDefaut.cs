using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class SoucheDefaut : MonoBehaviour, Uprootable, CaseActor
{
	float age;
	int uprootCost;
	float size;
	bool isDead;

	public float getAge() { return age; }
	public void setProperties( float age, float scale, bool dead, float cost, float humidite, float luminosite, float fertilite )
	{
		IsoTransform iso = GetComponent<IsoTransform>();
		iso.Size *= scale;
		iso.Position = new Vector3( iso.Position.x, scale * iso.Position.y, iso.Position.z );
		transform.localScale *= scale;
		size = scale;

		this.age = age;
		this.isDead = dead;
		this.uprootCost = (int)(size * uprootCost);
	}

	public bool uproot()
	{
		if (Main.get ().money >= uprootCost)
		{
			Main.get ().money -= uprootCost;
			GetComponent<AssignedCase>().get().addFertilite( -200 );
			return true;
		}
		return false;
	}

	public int getUprootCost()
	{
		return uprootCost;
	}

	public void updateCase( Case c )
	{

	}
}
