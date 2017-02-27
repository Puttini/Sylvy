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

	void Start()
	{
		instance = this;

		// Mode facile...
		addIncome( subvention, 30 );
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			leaveSelection ();
	}

	public static GameObject addIncome( float income, float period, float duration = -1.0f )
	{
		GameObject obj = new GameObject();
		Income inc = obj.AddComponent<Income>();
		inc.duration = duration;
		inc.period = period;
		inc.income = income;
		return obj;
	}

	public static float time()
	{
		return get().timeScale * (float)Time.time;
	}

	public static void leaveSelection()
	{
		PlantManager.get ().leaveSelection ();
	}
}
