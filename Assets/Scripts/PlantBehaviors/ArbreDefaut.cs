using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using UnityEngine;

public class ArbreDefaut : MonoBehaviour, Cuttable, CaseActor
{
	public float income;
	public float period;

	public float growingTime;
	public float lifeTime;

	public GameObject souche;
	public Sprite dead;
	public float cutIncome;
	public float cutDeadIncome;
	public float baseUprootCost;

	public float humidite;
	public float luminosite;
	public float fertilite;

	public float deadHumidite;
	public float deadLuminosite;
	public float deadFertilite;

	public float sHumidite;
	public float sLuminosite;
	public float sFertilite;

	public float sDeadHumidite;
	public float sDeadLuminosite;
	public float sDeadFertilite;

	public float hmin;
	public float hmax;
	public float lmin;
	public float lmax;
	public float fmin;
	public float fmax;
	public float dmin;
	public float pDie;
	public float pNaturalDeath;

	float lastUpdate;
	float finalY;
	Vector3 finalSize;
	Vector3 finalScale;
	float age;
	GameObject inc;
	bool isDead;

	ArbreDefaut()
	{
		age = 0;
		isDead = false;
	}

	// Use this for initialization
	void Start ()
	{
		lastUpdate = Main.time();
		inc = null;

		IsoTransform iso = GetComponent<IsoTransform>();
		finalY = iso.Position.y;
		finalSize = new Vector3( iso.Size.x, iso.Size.y, iso.Size.z );
		finalScale = transform.localScale;

		// Randomizing the final size of the tree
		float adultSize = 0.7f + 0.3f * Main.random();
		finalY *= adultSize;
		finalSize *= adultSize;
		finalScale *= adultSize;

		// Changing alpha
		Color c = GetComponent<SpriteRenderer>().color;
		c.a = 0.7f;
		GetComponent<SpriteRenderer>().color = c;
	}
	
	// Update is called once per frame
	void Update()
	{
		if ( !isDead )
		{
			float t = Main.time();
			float dt = t - lastUpdate;
			age += dt * GetComponent< AssignedCase >().get().getFertilite();
			lastUpdate = t;

			float scale = 0.3f + 0.7f*Mathf.Min( 1.0f, age / growingTime );

			if ( inc == null && scale >= 0.6 )
				inc = Main.addIncome( income, period );

			transform.localScale = scale * finalScale;
			IsoTransform iso = GetComponent<IsoTransform>();
			iso.Size = scale * finalSize;
			iso.Position = new Vector3( iso.Position.x, scale * finalY, iso.Position.z );
		}
		else
			lastUpdate = Main.time();
	}

	public float getAge()
	{
		return age;
	}

	public GameObject cut()
	{
		GameObject newTronc = GameObject.Instantiate( souche );
		SoucheDefaut t = newTronc.AddComponent<SoucheDefaut>();
		newTronc.AddComponent<AssignedCase>().set( GetComponent<AssignedCase>().get() );
		IsoTransform iso1 = GetComponent<IsoTransform>();
		IsoTransform iso2 = newTronc.GetComponent<IsoTransform>();
		iso2.Position = new Vector3( iso1.Position.x, iso2.Position.y, iso1.Position.z );
		float scale = 0.4f + 0.6f*Mathf.Min( 1.0f, age / growingTime );

		float h = isDead ? sDeadHumidite : sHumidite;
		float l = isDead ? sDeadLuminosite : sLuminosite;
		float f = isDead ? sDeadFertilite : sFertilite;
		t.setProperties( age, scale, dead, baseUprootCost, h, l, f );

		if( inc != null )
			GameObject.Destroy( inc );

		Main.get ().money += getCutPrice();

		newTronc.name = "Souche de " + name;

		return newTronc;
	}

	public int getCutPrice()
	{
		float scale2 = Mathf.Min( 1.0f, age / growingTime );
		float c;
		if ( isDead )
			c = (scale2 * cutDeadIncome);
		else
			c = (scale2 * cutIncome );
		return (int)c;
	}

	public void setAge( float a )
	{
		age = a;
	}

	public void updateCase( Case c )
	{
		float h = c.getHumidite();
		float l = c.getLuminosite();
		float f = c.getFertilite();
		if ( !isDead )
		{
			if ( h < hmin )
			{
				float p = (hmin-h)/hmin * pDie;
				if ( p > Main.random() )
					die();
			}
			else if ( h > hmax )
			{
				float p = (h-hmax)/(1-hmax) * pDie;
				if ( p > Main.random() )
					die();
			}

			if ( l < lmin )
			{
				float p = (lmin-l)/lmin * pDie;
				if ( p > Main.random() )
					die();
			}
			else if ( l > lmax )
			{
				float p = (l-lmax)/(1-lmax) * pDie;
				if ( p > Main.random() )
					die();
			}

			if ( f < fmin )
			{
				float p = (fmin-h)/fmin * pDie;
				if ( p > Main.random() )
					die();
			}
			else if ( f > fmax )
			{
				float p = (f-fmax)/(1-fmax) * pDie;
				if ( p > Main.random() )
					die();
			}

			if ( GridManager.get().getDiversite() < dmin )
			{
				if ( pDie > Main.random() )
					die();
			}

			if ( age > lifeTime )
			{
				if ( pNaturalDeath > Main.random() )
					die("Mort de vieillesse");
			}
		}


		float scale = Mathf.Min( 1.0f, age / growingTime );
		h = 0.7f + 0.3f*scale;
		l = 0.2f + 0.8f*scale;
		f = 0.5f + 0.5f*scale;
		if ( !isDead )
		{
			h *= humidite;
			l *= luminosite;
			f *= fertilite;
		}
		else
		{
			h *= deadHumidite;
			l *= deadLuminosite;
			f *= deadFertilite;
		}

		float h2 = 0.6f * h;
		float l2 = 0.65f * l;
		float f2 = 0.6f * f;


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

	public void die( string reason = "Mort")
	{
		isDead = true;
		GetComponent<SpriteRenderer>().sprite = dead;
		name += " (" + reason + ")";

		if (inc != null)
			GameObject.Destroy( inc );

		Main.msgArbreMort();
	}
}