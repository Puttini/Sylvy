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
	public GameObject souche;
	public Sprite dead;
	public float cutIncome;
	public float uprootCost;

	public float luminosite;
	public float humidite;
	public float fertilite;

	public float deadLuminosite;
	public float deadHumidite;
	public float deadFertilite;

	public float lmin;
	public float hmin;
	public float pDie;

	float lastUpdate;
	float finalY;
	Vector3 finalSize;
	Vector3 finalScale;
	float age;
	GameObject inc;
	bool isDead;

	// Use this for initialization
	void Start ()
	{
		lastUpdate = Main.time();
		age = 0;
		inc = null;
		IsoTransform iso = GetComponent<IsoTransform>();
		finalY = iso.Position.y;
		finalSize = new Vector3( iso.Size.x, iso.Size.y, iso.Size.z );
		finalScale = transform.localScale;

		// Changing alpha
		Color c = GetComponent<SpriteRenderer>().color;
		c.a = 0.7f;
		GetComponent<SpriteRenderer>().color = c;

		isDead = false;
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
		IsoTransform iso1 = GetComponent<IsoTransform>();
		IsoTransform iso2 = newTronc.GetComponent<IsoTransform>();
		iso2.Position = new Vector3( iso1.Position.x, iso2.Position.y, iso1.Position.z );
		t.setAge( age );
		float scale = 0.4f + 0.6f*Mathf.Min( 1.0f, age / growingTime );
		t.setScale( scale );
		t.setUprootCost (uprootCost);
		t.setDead( isDead );

		if( inc != null )
			GameObject.Destroy( inc );

		float scale2 = Mathf.Min( 1.0f, age / growingTime );
		Main.get ().money += (int)(scale2 * cutIncome);

		return newTronc;
	}

	public void updateCase( Case c )
	{
		if ( !isDead && ( c.getLuminosite() < lmin || c.getHumidite() < hmin ) )
		{
			// Mort de l'arbre
			if ( pDie > Main.random() )
			{
				isDead = true;
				GetComponent<SpriteRenderer>().sprite = dead;

				if (inc != null)
					GameObject.Destroy( inc );
			}
		}

		float scale = Mathf.Min( 1.0f, age / growingTime );
		if ( !isDead )
		{
			c.addHumidite( (0.7f+0.3f*scale)*humidite );
			c.addLuminosite( (0.2f+0.8f*scale)*luminosite );
			c.addFertilite( (0.5f + 0.5f*scale)*fertilite );
		}
		else
		{
			c.addHumidite( (0.7f+0.3f*scale)*deadHumidite );
			c.addLuminosite( (0.2f+0.8f*scale)*deadLuminosite );
			c.addFertilite( (0.5f + 0.5f*scale)*deadFertilite );
		}
	}
}
