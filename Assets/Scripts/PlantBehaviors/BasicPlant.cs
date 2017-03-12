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
	public float fmin;
	public float fmax;
	public float dmin;
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
		bool dead = false;

		float h = c.getHumidite();
		float l = c.getLuminosite();
		float f = c.getFertilite();

		if ( !dead && h < hmin )
		{
			float p = (hmin-h)/hmin * pDie;
			if ( p > Main.random() )
				dead = true;
		}
		else if ( !dead && h > hmax )
		{
			float p = (h-hmax)/(1-hmax) * pDie;
			if ( p > Main.random() )
				dead = true;
		}

		if ( !dead && l < lmin )
		{
			float p = (lmin-l)/lmin * pDie;
			if ( p > Main.random() )
				dead = true;
		}
		else if ( !dead && l > lmax )
		{
			float p = (l-lmax)/(1-lmax) * pDie;
			if ( p > Main.random() )
				dead = true;
		}

		if ( !dead && f < fmin )
		{
			float p = (fmin-h)/fmin * pDie;
			if ( p > Main.random() )
				dead = true;
		}
		else if ( !dead && f > fmax )
		{
			float p = (f-fmax)/(1-fmax) * pDie;
			if ( p > Main.random() )
				dead = true;
		}

		if ( !dead && GridManager.get().getDiversite() < dmin )
		{
			if ( pDie > Main.random() )
				dead = true;
		}

		if ( !dead && Main.time() - firstTime > lifeTime )
		{
			if ( pNaturalDeath > Main.random() )
				dead = true;
		}


		if ( dead )
		{
			if (inc != null)
				GameObject.Destroy( inc );

			c.removeObject(gameObject);
		}
		else
		{
			h = humidite;
			l = luminosite;
			f = fertilite;

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
