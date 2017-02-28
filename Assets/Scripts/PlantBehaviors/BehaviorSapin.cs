using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using UnityEngine;

public class BehaviorSapin : MonoBehaviour, Cuttable
{
	public float income;
	public float period;

	public float growingTime;
	public GameObject tronc;

	float startTime;
	float finalY;
	Vector3 finalSize;
	Vector3 finalScale;
	float age;
	GameObject inc;

	// Use this for initialization
	void Start ()
	{
		startTime = Main.time();
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
	}
	
	// Update is called once per frame
	void Update ()
	{
		age = Main.time() - startTime;
		float scale = 0.3f + 0.7f*Mathf.Min( 1.0f, age / growingTime );
		if ( inc == null && scale >= 0.6 )
			inc = Main.addIncome( income, period );

		transform.localScale = scale * finalScale;
		IsoTransform iso = GetComponent<IsoTransform>();
		iso.Size = scale * finalSize;
		iso.Position = new Vector3( iso.Position.x, scale * finalY, iso.Position.z );
	}

	public float getAge()
	{
		return age;
	}

	public GameObject cut()
	{
		GameObject newTronc = GameObject.Instantiate( tronc );
		TroncSapin t = newTronc.AddComponent<TroncSapin>();
		IsoTransform iso1 = GetComponent<IsoTransform>();
		IsoTransform iso2 = newTronc.GetComponent<IsoTransform>();
		iso2.Position = new Vector3( iso1.Position.x, iso2.Position.y, iso1.Position.z );
		t.setAge( age );
		float scale = 0.4f + 0.6f*Mathf.Min( 1.0f, age / growingTime );
		t.setScale( scale );

		if( inc != null )
			GameObject.Destroy( inc );
		return newTronc;
	}
}
