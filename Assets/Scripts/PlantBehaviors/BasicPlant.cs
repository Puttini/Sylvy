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
	public float pNaturalDeath;

	float firstTime;

	GameObject inc;

	public void init()
	{
		if ( income != 0 && period > 0 )
			inc = Main.addIncome( income, period );
		else
			inc = null;

		firstTime = Main.time();
	}

	public void updateCase( Case c )
	{
		bool again = true;
		if ( Main.time() - firstTime > lifeTime )
		{
			// Mort naturelle de la plante
			if ( pNaturalDeath >= Main.random() )
			{
				if (inc != null)
					GameObject.Destroy( inc );

				c.removeObject(gameObject);
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

				c.removeObject(gameObject);
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

	public void fromPrefab( BasicPlant b )
	{
		income = b.income;
		period = b.period;

		luminosite = b.luminosite;
		humidite = b.humidite;
		fertilite = b.fertilite;

		hmin = b.hmin;
		hmax = b.hmax;
		lmin = b.lmin;
		lmax = b.lmax;

		pDie = b.pDie;

		lifeTime = b.lifeTime;
		pNaturalDeath = b.pNaturalDeath;
	}
}
