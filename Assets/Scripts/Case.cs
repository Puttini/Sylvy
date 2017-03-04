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

	public Case()
	{
		layers = new Layer[(int)Layers.NbLayers];
		for( int i = 0 ; i < (int)Layers.NbLayers; ++i )
			layers[ i ] = new Layer();
	}

	public GameObject insert( Plant p )
	{
		return layers[ p.layer ].insert( p );
	}

	public GameObject getFirstObject( int layer )
	{
		return layers [layer].getFirstObject();
	}

	public GameObject cut()
	{
		return layers[ (int)Layers.Arbres ].cut();
	}
};
