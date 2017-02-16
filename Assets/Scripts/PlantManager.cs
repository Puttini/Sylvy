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
		for( int i = 0 ; i < 15 ; i++ )
			addToPanel( sapin );
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
		icons[ i, j ].transform.parent = this.transform;
		icons[ i, j ].GetComponent<RectTransform>().anchoredPosition =
			new Vector2( xMargin + i * iconW, -yMargin - j * iconH );
		ButtonPlant b = icons[ i, j ].GetComponent<ButtonPlant>();
		b.setPlant( p );
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && selectedPlant != null)
		{
			selectedPlant = null;
			GameObject.Destroy( cursorPlant );
			cursorPlant = null;
		}

		if (Input.GetMouseButton(0) && cursorPlant != null)
		{

		}

		if (selectedPlant != null)
		{
			IsoTransform iso = cursorPlant.GetComponent<IsoTransform> ();
			Vector3 pos = (Vector3)Isometric.CreateXYZfromY (Input.mousePosition, 0.3f);
			pos.x = (float)Math.Round (pos.x);
			pos.z = (float)Math.Round (pos.z);
			pos.y = iso.Position.y;

			if (pos.x > 0 && pos.z > 0 && pos.x <= GridManager.get().size && pos.z <= GridManager.get().size )
				iso.Position = pos;
			else
				iso.Position = new Vector3 (-1.0f, iso.Position.y, -1.0f);
		}
	}

	public void setSelectedPlant( Plant p )
	{
		selectedPlant = p;

		if( cursorPlant != null )
			GameObject.Destroy( cursorPlant );
		cursorPlant = GameObject.Instantiate( p.prefab );

		// Changing alpha
		Color c = cursorPlant.GetComponent<SpriteRenderer>().color;
		c.a = 0.5f;
		cursorPlant.GetComponent<SpriteRenderer>().color = c;
	}
}
