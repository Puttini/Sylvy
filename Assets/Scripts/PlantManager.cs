using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

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


	GameObject selectedPlant;


	// Use this for initialization
	void Start ()
	{
		instance = this;
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
		if( selectedPlant != null )
			selectedPlant.GetComponent<RectTransform>().anchoredPosition = new Vector3( Input.mousePosition.x, Input.mousePosition.y, 0 );
	}

	public void setSelectedPlant( Plant p )
	{
		if( selectedPlant != null )
			GameObject.Destroy( selectedPlant );
		selectedPlant = GameObject.Instantiate( p.sprite );
		selectedPlant.transform.SetParent( uiManager.transform );
		selectedPlant.GetComponent<RectTransform>().anchorMin = new Vector2( 0, 0 );
		selectedPlant.GetComponent<RectTransform>().anchorMax = new Vector2( 0, 0 );

		// Changing alpha
		Color c = selectedPlant.GetComponent<Image>().color;
		c.a = 0.6f;
		selectedPlant.GetComponent<Image>().color = c;
	}
}
