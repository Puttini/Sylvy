﻿using System.Collections;
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
	}

	public void initUpdate()
	{
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
		int n = 4;
		luminosite = ( luminosite*(n-1) + tmpLuminosite ) / n;
		humidite = ( humidite*(n-1) + tmpHumidite) / n;
		fertilite = ( fertilite*(n-1) + tmpFertilite ) / n;

		// Apparition de fleurs, etc...
		int nbPlants = PlantManager.get().getNbPlants();
		float l = getLuminosite();
		float h = getHumidite();
		float f = getFertilite();
		for( int i = 0 ; i < nbPlants ; ++i )
		{
			Plant p = PlantManager.get().getPlant(i);
			if ( p.toss(l, h, f) )
				insert (p);
		}
	}

	public GameObject insert( Plant p )
	{
		GameObject obj = layers[ p.layer ].insert( p, this );
		if ( obj != null )
		{
			GridManager.get().placeObject(obj, coordX, coordY);
			obj.AddComponent<AssignedCase>().set( this );
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

	void spread( int x, int y )
	{
		Case c = GridManager.get ().getCase (x, y);
		if ( c != null )
		{
			c.addFertilite( fertilite*0.06f );
			c.addHumidite( humidite*0.1f );
			c.addLuminosite( luminosite*0.03f );
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

	public float getLuminosite() { return Mathf.Pow( 1.5f, luminosite ); }
	public float getHumidite() { return Mathf.Pow( 1.5f, humidite ); }
	public float getFertilite() { return Mathf.Pow( 1.5f, fertilite ); }

	public string getDescription()
	{
		string txt = "Humidité : " + getHumidite() + "\n";
		txt += "Luminosité : " + getLuminosite() + "\n";
		txt += "Fertilité : " + getFertilite() + "\n\n\n";
		txt += "Contient :\n\n";
		for( int i = 0 ; i < (int)Layers.NbLayers ; ++i )
			txt += layers[i].getTxt() + "\n";
		return txt;
	}
};
