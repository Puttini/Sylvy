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

	public float lmin;
	public float lmax;
	public float hmin;
	public float hmax;
	public float pDie;

	GameObject inc;

	public void Start()
	{
		if ( income != 0 && period > 0 )
			inc = Main.addIncome( income, period );
		else
			inc = null;
	}

	public void updateCase( Case c )
	{
		if ( c.getLuminosite() < lmin || c.getLuminosite() > lmax || c.getHumidite() < hmin || c.getHumidite() > hmax )
		{
			// Mort de l'arbre
			if ( pDie > Main.random() )
			{
				if (inc != null)
					GameObject.Destroy( inc );
				
				GetComponent<AssignedCase>().get().removeObject(gameObject);
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
