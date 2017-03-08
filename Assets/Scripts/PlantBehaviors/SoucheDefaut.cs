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

	float h, l, f;

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

		h = humidite;
		l = luminosite;
		f = fertilite;
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
		float h2 = 0.6f * h;
		float l2 = 0.8f * l;
		float f2 = 0.8f * f;


		GridManager gm = GridManager.get();
		int x = c.getX();
		int y = c.getY();

		c.addProperties( h, l, f );
		gm.addProperties( x-1, y-1, h2, l2, f2 );
		gm.addProperties( x  , y-1, h2, l2, f2 );
		gm.addProperties( x+1, y-1, h2, l2, f2 );
		gm.addProperties( x+1, y  , h2, l2, f2 );
		gm.addProperties( x+1, y+1, h2, l2, f2 );
		gm.addProperties( x  , y+1, h2, l2, f2 );
		gm.addProperties( x-1, y+1, h2, l2, f2 );
		gm.addProperties( x-1, y  , h2, l2, f2 );
	}
}
