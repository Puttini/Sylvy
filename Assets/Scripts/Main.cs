using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{
	// Singleton class
	private static Main instance;
	public static Main get() { return Main.instance; }

	public int money;
	public float timeScale;
	public int subvention;
	public GameObject incomes;

	public GameObject speedUpButton;
	public GameObject speedDownButton;
	public GameObject pauseButton;
	public GameObject unpauseButton;

	public GameObject notifMessage;

	public GameObject descriptionPanel;
	public Text descriptionContent;

	float theTime;
	float lastTime;
	float tempTimeScale;

	System.Random rng;

	bool normalSpeed;

	// Pour les messages ingame
	bool firstTimeButtonPlant;
	bool firstTimeButtonCut;
	bool firstTimeButtonUproot;
	bool firstTimeInsertTree;

	void Start()
	{
		instance = this;
		rng = new System.Random();
		normalSpeed = true;

		bool tuto = false;
		firstTimeButtonPlant = tuto;
		firstTimeButtonCut = tuto;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			leaveSelection ();
			descriptionPanel.SetActive( false );
		}

		float t = Time.time;
		theTime += timeScale * ( t - lastTime );
		lastTime = t;
	}

	public static GameObject addIncome( float income, float period, float duration = -1.0f )
	{
		GameObject obj = new GameObject();
		Income inc = obj.AddComponent<Income>();
		inc.duration = duration;
		inc.period = period;
		inc.income = income;
		obj.transform.SetParent( get().incomes.transform );
		return obj;
	}

	public static float time()
	{
		return get().theTime;
	}

	public static void leaveSelection()
	{
		PlantManager.get ().leaveSelection ();
		ButtonCut.get().leaveSelection();
		ButtonUproot.get().leaveSelection();
	}

	public static void init()
	{
		get().speedNormal();
		GridManager.get().init();
		CameraController.get().init();
		addIncome( get().subvention, 30 );

		get().theTime = 0;
		get().lastTime = Time.time;
	}

	public static void reset()
	{
		GridManager.get().reset();
		foreach( Transform inc in get().incomes.transform )
			GameObject.Destroy( inc.gameObject );
	}

	public static void pause()
	{
		get().tempTimeScale = get().timeScale;
		get().timeScale = 0;

		get().pauseButton.SetActive( false );
		get().unpauseButton.SetActive( true );
	}

	public static void unpause()
	{
		get().timeScale = get().tempTimeScale;

		get().unpauseButton.SetActive( false );
		get().pauseButton.SetActive( true );
	}

	public void menu()
	{
		leaveSelection();
		pause();
		Menu.get().gameObject.SetActive( true );
	}

	public void speedUp()
	{
		timeScale *= 3;

		speedUpButton.SetActive( false );
		speedDownButton.SetActive( true );

		normalSpeed = false;
	}

	public void speedDown()
	{
		timeScale /= 3.0f;

		speedDownButton.SetActive( false );
		speedUpButton.SetActive( true );

		normalSpeed = true;
	}

	public void speedNormal()
	{
		if( !normalSpeed )
			speedDown();
	}

	public void pauseAction()
	{
		pause();
	}

	public void unpauseAction()
	{
		unpause();
	}

	public static float random()
	{
		return (float)get().rng.NextDouble();
	}

	public static void displayNotif( string title, string subtitle, string content )
	{
		pause();
		get().notifMessage.SetActive( true );
		get().notifMessage.GetComponent< NotifMessage >().set( title, subtitle, content );
	}

	public static void openDescription( Case c )
	{
		get().descriptionPanel.SetActive( true );
		get().descriptionContent.text = c.getDescription();
	}

	public static void openDescription( int x, int y )
	{
		openDescription( GridManager.get().getCase( x, y ) );
	}

	public static void msgButtonPlant()
	{
		if ( get().firstTimeButtonPlant )
		{
			displayNotif(
				"Planter des arbres",
				"Pourquoi planter des arbres ?",
				"Votre but principal est de planter des arbres afin d'entretenir votre forêt.\n" +
				"Chaque arbre a sa particularité, et agît sur les cases les plus proches : sur leur taux d'humidité, de luminosité et de fertilité.\n" +
				"Agir sur l'environnement permet l'apparition de certaines espèces de fleurs et de champignons, et permet de compléter la flaure locale.\n" +
				"Chaque arbre vous coûte un certain montant à planter ; " +
				"mais une fois à maturité il vous rapportera un petit peu, ce qui vous permettra de continuer à faire grandir votre forêt !\n" +
				"Une fois assez grand, vous pouvez décider de couper un arbre pour vendre le son bois, mais cela vous laissera une souche qu'il est coûteux de déraciner." +
				"\nChoisissez donc soigneusement ce que vous souhaitez planter ! ;)" );
			get().firstTimeButtonPlant = false;
		}
	}

	public static void msgInsertTree()
	{

	}
}
