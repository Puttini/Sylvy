using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case
{
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
