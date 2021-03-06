﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using System;

public class PlantManager : MonoBehaviour
{
	// Singleton class
	static PlantManager instance;
	public static PlantManager get() { return instance; }

	public int w, h;
	public float iconW, iconH;
	public float xMargin, yMargin;
	public GameObject buttonPrefab;
	public GameObject uiManager;
	public Text buttonDesc;
	GameObject[,] icons;

	// Plant prefabs
	public Plant sapin;
	public Plant saule;
	public Plant hetre;
	public Plant bouleau;
	public Plant chene;
	public Plant fougere;
	public Plant herbe;
	public Plant marguerite;
	public Plant lin;
	public Plant cepe;
	public Plant tricholome;
	public Plant amanite;
	int nbPlants = 12;

	Plant selectedPlant;
	GameObject cursorPlant;


	// Use this for initialization
	void Start ()
	{
		instance = this;
		cursorPlant = null;
		selectedPlant = null;

		icons = new GameObject[w,h];
		for( int i = 0; i < w; ++i )
		{
			for( int j = 0; j < h; ++j )
				icons[ i, j ] = null;
		}

		// Tests
		addToPanel( sapin );
		addToPanel( saule );
		addToPanel( hetre );
		addToPanel( bouleau );
		addToPanel( chene );
		/*
		addToPanel( fougere );
		addToPanel( herbe );
		addToPanel( marguerite );
		addToPanel( lin );
		addToPanel( cepe );
		addToPanel( tricholome );
		addToPanel( amanite );
		*/
	}

	public void addToPanel( Plant p )
	{
		for( int i = 0; i < w; ++i )
		{
			for( int j = 0; j < h; ++j )
			{
				if( icons[ i, j ] == null )
				{
					addToPanel( i, j, p );
					return;
				}
			}
		}

		Debug.Log( "Plus de place dans l'interface !!" );
	}

	void addToPanel( int i, int j, Plant p )
	{
		icons[i,j] = GameObject.Instantiate( buttonPrefab );
		icons[ i, j ].transform.SetParent( this.transform );
		icons[ i, j ].GetComponent<RectTransform>().anchoredPosition =
			new Vector2( xMargin + i * iconW, -yMargin - j * iconH );
		ButtonPlant b = icons[ i, j ].GetComponent<ButtonPlant>();
		b.setPlant( p );
	}

	void Update()
	{
		if ( Input.GetMouseButtonDown(0) && cursorPlant != null && Input.mousePosition.y >= 145 )
		{
			Vector3 pos = (Vector3)Isometric.CreateXYZfromY (Input.mousePosition, 0);
			if( Main.get().money >= selectedPlant.cost )
			{
				if( GridManager.get().placePlant( (int)( Math.Round( pos.x ) ) - 1, (int)( Math.Round( pos.z ) ) - 1, selectedPlant ) )
				{
					Main.get().money -= selectedPlant.cost;

					if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift))
						leaveSelection ();

					Main.msgInsertArbre();
				}
			}
		}

		if ( Input.GetMouseButtonDown(1) )
		{
			Vector3 pos = Isometric.CreateXYZfromY (Input.mousePosition, 0).Value;
			int x = (int)Math.Round (pos.x) - 1;
			int y = (int)Math.Round (pos.z) - 1;

			if ( x >= 0 && x < GridManager.get().getSize()
				&& y >= 0 && y < GridManager.get().getSize() )
				Main.openDescription( x, y );
		}

		if (selectedPlant != null)
		{
			IsoTransform iso = cursorPlant.GetComponent<IsoTransform> ();
			float xdec = iso.Position.x - (float)Math.Round (iso.Position.x);
			float zdec = iso.Position.z - (float)Math.Round (iso.Position.z);
			try
			{
				Vector3 pos = Isometric.CreateXYZfromY (Input.mousePosition, 0).Value;
				pos.x = (float)Math.Round (pos.x) + xdec;
				pos.z = (float)Math.Round (pos.z) + zdec;
				pos.y = iso.Position.y;

				if (pos.x > 0 && pos.z > 0 && pos.x <= GridManager.get().size && pos.z <= GridManager.get().size )
					iso.Position = pos;
				else
					iso.Position = new Vector3 ( -1.0f + xdec, iso.Position.y, -1.0f + zdec);
			}
			catch (System.InvalidOperationException)
			{
				iso.Position = new Vector3 ( -1.0f + xdec, iso.Position.y, -1.0f + zdec);
			}
		}
	}

	public void setSelectedPlant( Plant p )
	{
		Main.leaveSelection();

		selectedPlant = p;
		cursorPlant = GameObject.Instantiate( p.previewPrefab );

		// Changing alpha
		Color c = cursorPlant.GetComponent<SpriteRenderer>().color;
		c.a = 0.4f;
		cursorPlant.GetComponent<SpriteRenderer>().color = c;
	}

	public void leaveSelection()
	{
		selectedPlant = null;
		if (cursorPlant != null)
		{
			GameObject.Destroy (cursorPlant);
			cursorPlant = null;
		}
	}

	public Plant getPlant( int i )
	{
		switch (i)
		{
		case 0:
			return sapin;
		case 1:
			return saule;
		case 2:
			return hetre;
		case 3:
			return bouleau;
		case 4:
			return chene;
		case 5:
			return fougere;
		case 6:
			return herbe;
		case 7:
			return marguerite;
		case 8:
			return lin;
		case 9:
			return cepe;
		case 10:
			return tricholome;
		case 11:
			return amanite;
		}
		return null;
	}

	public int getNbPlants()
	{
		return nbPlants;
	}

	public void buttonDescription( Plant p )
	{
		if ( p == null )
		{
			buttonDesc.text = "";
		}
		else
		{
			buttonDesc.text = "<b>" + p.theName + "</b>\n"
				+ "Prix : " + p.cost + "\n"
				+ "<i>" + p.description + "</i>";
		}
	}

	public bool isSelected()
	{
		return selectedPlant != null;
	}
}
