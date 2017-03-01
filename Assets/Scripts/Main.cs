using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	float theTime;
	float lastTime;
	float tempTimeScale;

	bool normalSpeed;

	void Start()
	{
		instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			leaveSelection ();

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
}
