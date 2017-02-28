using System.Collections;
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
	GameObject[,] icons;

	// Plant prefabs
	public Plant sapin;
	public Plant fougere;
	public Plant herbe;
	public Plant marguerite;
	public Plant cepe;

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
		addToPanel( fougere );
		addToPanel( marguerite );
		addToPanel( cepe );
		addToPanel( herbe );
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
		if ( Input.GetMouseButtonDown(0) && cursorPlant != null && Input.mousePosition.y >= 150 )
		{
			Vector3 pos = (Vector3)Isometric.CreateXYZfromY (Input.mousePosition, 0);
			if( Main.get().money >= selectedPlant.cost )
			{
				if( GridManager.get().placePlant( (int)( Math.Round( pos.x ) ) - 1, (int)( Math.Round( pos.z ) ) - 1, selectedPlant ) )
				{
					Main.get().money -= selectedPlant.cost;

					if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift))
						leaveSelection ();
				}
			}
		}

		if (selectedPlant != null)
		{
			IsoTransform iso = cursorPlant.GetComponent<IsoTransform> ();
			Vector3 pos = (Vector3)Isometric.CreateXYZfromY (Input.mousePosition, 0);
			float xdec = iso.Position.x - (float)Math.Round (iso.Position.x);
			float zdec = iso.Position.z - (float)Math.Round (iso.Position.z);
			pos.x = (float)Math.Round (pos.x) + xdec;
			pos.z = (float)Math.Round (pos.z) + zdec;
			pos.y = iso.Position.y;

			if (pos.x > 0 && pos.z > 0 && pos.x <= GridManager.get().size && pos.z <= GridManager.get().size )
				iso.Position = pos;
			else
				iso.Position = new Vector3 ( -1.0f + xdec, iso.Position.y, -1.0f + zdec);
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
}
