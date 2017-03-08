using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlant : MonoBehaviour, CaseActor
{
	public float income;
	public float period;

	public float luminosite;
	public float humidite;
	public float fertilite;

	public float hmin;
	public float hmax;
	public float lmin;
	public float lmax;
	public float pDie;

	public float lifeTime;
	public float pNaturalDie;

	float firstTime;

	GameObject inc;
	GameObject instance;
	public void  setInstance( GameObject o ) { instance = o; }

	public void Start()
	{
		if ( income != 0 && period > 0 )
			inc = Main.addIncome( income, period );
		else
			inc = null;

		firstTime = Main.time();
		Debug.Log("start");
	}

	public void updateCase( Case c )
	{
		Debug.Log( Main.time() - firstTime );
		bool again = true;
		if ( Main.time() - firstTime > lifeTime )
		{
			// Mort de la plante
			Debug.Log("DYING");
			if ( pNaturalDie >= Main.random() )
			{
				Debug.Log("Die");
				if (inc != null)
					GameObject.Destroy( inc );

				c.removeObject(instance);
				again = false;
			}
		}

		if ( again && (c.getLuminosite() < lmin || c.getLuminosite() > lmax || c.getHumidite() < hmin || c.getHumidite() > hmax) )
		{
			// Mort de la plante
			if ( pDie >= Main.random() )
			{
				if (inc != null)
					GameObject.Destroy( inc );

				c.removeObject(instance);
			}
		}
		else
		{
			float h = humidite;
			float l = luminosite;
			float f = fertilite;

			float h2 = 0.6f * h;
			float l2 = 0.6f * l;
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
	}
}
