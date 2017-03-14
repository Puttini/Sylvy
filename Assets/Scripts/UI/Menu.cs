using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	// Singleton class
	private static Menu instance;
	public static Menu get() { return instance; }

	public GameObject mainPanel;
	public GameObject newGamePanel;
	public GameObject instructionsPanel;
	public GameObject resumeButton;
	bool started;

	void Start ()
	{
		instance = this;

		started = false;
		resumeButton.SetActive( false );
		newGamePanel.SetActive( false );
		instructionsPanel.SetActive( false );
		mainPanel.SetActive( true );

		Main.get().timeScale = 0;
	}

	public void nouvellePartie()
	{
		mainPanel.SetActive( false );
		newGamePanel.SetActive( true );
	}

	public void instructions()
	{
		mainPanel.SetActive( false );
		instructionsPanel.SetActive( true );
	}

	public void quitter()
	{
		Application.Quit();
	}

	public void facile()
	{
		Main.get().money = 3000;
		Main.get().subvention = 50;
		Main.get().timeScale = 1.5f;
		GridManager.get().size = 25;
		Main.get().pauseButton.SetActive( true );
		startNewGame();
	}

	public void normal()
	{
		Main.get().money = 1500;
		Main.get().subvention = 0;
		Main.get().timeScale = 1.5f;
		GridManager.get().size = 35;
		Main.get().pauseButton.SetActive( true );
		startNewGame();
	}

	public void difficile()
	{
		Main.get().money = 1000;
		Main.get().subvention = -50;
		Main.get().timeScale = 1.5f;
		GridManager.get().size = 50;
		Main.get().pauseButton.SetActive( false );
		startNewGame();
	}

	public void newGameRetour()
	{
		newGamePanel.SetActive( false );
		mainPanel.SetActive( true );
	}

	public void instructionsRetour()
	{
		instructionsPanel.SetActive( false );
		mainPanel.SetActive( true );
	}

	public void startNewGame()
	{
		if( started )
			Main.reset();
		Main.init();

		started = true;
		newGamePanel.SetActive( false );
		mainPanel.SetActive( true );
		resumeButton.SetActive( true );

		gameObject.SetActive( false );
	}

	public void reprendre()
	{
		gameObject.SetActive( false );
		//Main.unpause();
	}
}
