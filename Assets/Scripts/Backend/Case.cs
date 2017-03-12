using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case
{
	/*
	 * Remarques générales :
	 *  - Déraciner les souches
	 * (détruit l'arbre, temps de rétablissement du sol ?, détruit des trucs autour, mais pas les arbres)
	 *  - Entretenir (fleurs) ?
	 *  - Indicateurs de diversité, et de quantité => qualité, apport naturel ?
	 *  - Ajout d'un gros chêne (optionnel)
	 *  - (Important) Les arbres peuvent mourir/tomber malade, et affecter la fertilité des cases aux alentours
	 * 		Il faudra donc les couper, etc...
	 * 
	 * Propriétés d'une case :
	 *  - luminosité
	 *  - humidité
	 *  - fertilité (coefficient multiplicateur de proba d'apparition de plante ?)
	 * 
	 * Ajout sur la vitesse de poussée ?
	 * 
	 * Une espèce d'arbre (but ultime du jeu) qu'on ne peut pas planter.
	 * 
	 * */

	public enum Layers : int { Arbres, Fleurs, Fougeres, NbLayers };

	Layer[] layers;

	// Propriétés
	float tmpLuminosite;
	float tmpHumidite;
	float tmpFertilite;

	float luminosite;
	float humidite;
	float fertilite;

	int coordX, coordY;
	public int getX() { return coordX; }
	public int getY() { return coordY; }

	public Case( int x, int y )
	{
		coordX = x;
		coordY = y;

		layers = new Layer[(int)Layers.NbLayers];
		for( int i = 0 ; i < (int)Layers.NbLayers; ++i )
			layers[ i ] = new Layer();

		luminosite = 0;
		humidite = 0;
		fertilite = 0;

		tmpLuminosite = 0;
		tmpHumidite = 0;
		tmpFertilite = 0;
	}

	public void update()
	{
		for (int i = 0; i < (int)Layers.NbLayers; ++i)
			layers [i].updateCase(this);

		spread(coordX - 1, coordY - 1);
		spread(coordX, coordY - 1);
		spread(coordX + 1, coordY - 1);
		spread(coordX + 1, coordY);
		spread(coordX + 1, coordY + 1);
		spread(coordX, coordY + 1);
		spread(coordX - 1, coordY + 1);
		spread(coordX - 1, coordY);
	}

	public void finishUpdate()
	{
		luminosite = ( luminosite*2 + tmpLuminosite ) / 3;
		humidite = ( humidite*5 + tmpHumidite) / 6;
		fertilite = ( fertilite*9 + tmpFertilite ) / 10;

		// Apparition de fleurs, etc...
		int nbPlants = PlantManager.get().getNbPlants();
		float l = getLuminosite();
		float h = getHumidite();
		float f = getFertilite();
		for( int i = 0 ; i < nbPlants ; ++i )
		{
			Plant p = PlantManager.get().getPlant(i);
			if( p.toss( l, h, f ) )
			{
				insert( p );
				//Main.msgApparitionFleur();

				if ( i == 9 )
					Main.msgPolypore();
			}
		}

		tmpLuminosite = 0;
		tmpHumidite = 0;
		tmpFertilite = 0;
	}

	public GameObject insert( Plant p )
	{
		GameObject obj = layers[ p.layer ].insert( p );
		if ( obj != null )
		{
			GridManager.get().placeObject(obj, coordX, coordY);
			obj.AddComponent<AssignedCase>().set( this );
			obj.name = p.theName;
		}
		return obj;
	}

	public GameObject getFirstObject( int layer )
	{
		return layers [layer].getFirstObject();
	}

	public GameObject cut()
	{
		return layers[ (int)Layers.Arbres ].cut();
	}

	public bool uproot()
	{
		if (layers [(int)Layers.Arbres].uproot ())
		{
			destroyLocalPlants();
			destroyNearlyPlants();
			return true;
		}
		else
			return false;
	}

	public void addHumidite( float h )
	{
		tmpHumidite += h;
	}

	public void addLuminosite( float l )
	{
		tmpLuminosite += l;
	}

	public void addFertilite( float f )
	{
		tmpFertilite += f;
	}

	public void addProperties( float h, float l, float f )
	{
		addHumidite(h);
		addLuminosite(l);
		addFertilite(f);
	}

	void spread( int x, int y )
	{
		Case c = GridManager.get ().getCase (x, y);
		if ( c != null )
		{
			c.addProperties(
				0.08f  * humidite,
				0.01f * luminosite,
				0.07f * fertilite );
		}
	}

	public void destroyLocalPlants()
	{
		for (int i = 0; i < (int)Layers.NbLayers; ++i)
			layers [i].destroyPlants ();
	}

	public void destroySomeLocalPlants( float p )
	{
		for( int i = 1 ; i < (int)Layers.NbLayers; ++i ) // Pas les arbres !
			layers[i].destroyPlants( p );
	}

	public void destroyNearlyPlants()
	{
		destroyNearlyPlants(coordX - 1, coordY - 1);
		destroyNearlyPlants(coordX, coordY - 1);
		destroyNearlyPlants(coordX + 1, coordY - 1);
		destroyNearlyPlants(coordX + 1, coordY);
		destroyNearlyPlants(coordX + 1, coordY + 1);
		destroyNearlyPlants(coordX, coordY + 1);
		destroyNearlyPlants(coordX - 1, coordY + 1);
		destroyNearlyPlants(coordX - 1, coordY);
	}

	public static void destroyNearlyPlants( int x, int y, float p = 0.5f )
	{
		Case c = GridManager.get ().getCase (x, y);
		if ( c != null )
			c.destroySomeLocalPlants( p );
	}

	public float getLuminosite() { return Mathf.Atan( luminosite )/Mathf.PI*2 + 1; }
	public float getHumidite() { return Mathf.Atan( humidite )/Mathf.PI*2 + 1; }
	public float getFertilite() { return Mathf.Atan( fertilite )/Mathf.PI*2 + 1; }

	public string getDescription()
	{
		string txt = "";

		GameObject tree = layers[0].getFirstObject();
		if ( tree != null )
		{
			txt += "<b>" + tree.name + "</b>\n";
			AssignedPlant ap = tree.GetComponent<AssignedPlant>();
			if ( ap != null )
				txt += "<i>" + ap.get().description + "</i>\n";

			Cuttable cut = tree.GetComponent<Cuttable>();
			if ( cut != null )
				txt += "Coupe : " + cut.getCutPrice() + "$\n";

			Uprootable up = tree.GetComponent<Uprootable>();
			if ( up != null )
				txt += "Déracinement : " + up.getUprootCost() + "$\n";

			txt += "\n";
		}

		txt += "Humidité : " + getHumidite() + "\n";
		txt += "Luminosité : " + getLuminosite() + "\n";
		txt += "Fertilité : " + getFertilite() + "\n\n";

		txt += "Contient :\n";
		for( int i = 1 ; i < (int)Layers.NbLayers ; ++i )
			txt += layers[i].getTxt();
		return txt;
	}

	public bool removeObject( GameObject o )
	{
		for( int i = 0 ; i < (int)Layers.NbLayers ; ++i )
		{
			if ( layers[i].removeObject(o) )
				return true;
		}
		return false;
	}

	public void addToDico( Dictionary<string,int> dico )
	{
		for( int i = 0 ; i < (int)Layers.NbLayers ; ++i )
			layers[i].addToDico(dico);
	}
};
