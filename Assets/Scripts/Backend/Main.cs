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
	public float acceleration;
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
	public bool enableMsg;
	bool ftButtonPlant;
	bool ftButtonCut;
	bool ftButtonUproot;
	bool ftInsertArbre;
	bool ftArbreMort;
	bool ftDiversite5;
	bool ftDiversite7;
	bool ftDiversite10;
	bool ftDescription;
	bool ftPolypore;

	void Start()
	{
		instance = this;
		rng = new System.Random();
		normalSpeed = true;

		ftButtonPlant = enableMsg;
		ftButtonCut = enableMsg;
		ftButtonUproot = enableMsg;
		ftInsertArbre = enableMsg;
		ftArbreMort = enableMsg;
		ftDiversite5 = enableMsg;
		ftDiversite7 = enableMsg;
		ftDiversite10 = enableMsg;
		ftDescription = enableMsg;
		ftPolypore = enableMsg;
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
		GridManager.get().randomGeneration();
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
		timeScale *= acceleration;

		speedUpButton.SetActive( false );
		speedDownButton.SetActive( true );

		normalSpeed = false;
	}

	public void speedDown()
	{
		timeScale /= acceleration;

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

		Main.msgDescription();
	}

	public static void openDescription( int x, int y )
	{
		openDescription( GridManager.get().getCase( x, y ) );
	}

	public static void msgButtonPlant()
	{
		if ( get().ftButtonPlant )
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
				"\nChoisissez donc soigneusement ce que vous souhaitez planter ! ;)\n" +
				"\n" +
				"Commencez par planter d'autres <b>sapins</b> et <b>bouleaux</b> pour solidifier votre écosystème actuel.\n" );
			get().ftButtonPlant = false;
		}
	}

	public static void msgButtonCut()
	{
		if ( get().ftButtonCut )
		{
			displayNotif(
				"Couper des arbres",
				"Devenez bûcheron !",

				"Une fois l'outil <i>hache</i> actif, vous devez sélectionner la case qui contient l'arbre que vous souhaitez couper, (maintenez <i>Shift</i> pour couper plusieurs arbres).\n" +
				"Couper un arbre vous rapportera de l'argent en vendant son bois.\n" +
				"Le montant reversé dépend de l'espèce de l'arbre, de sa taille, et de sa santé (un arbre mort vous rapportera beaucoup moins !)\n" +
				"\n" +
				"Ceci est probablement votre source principale de revenu, vous devez donc choisir soigneusement les espèces d'arbre que vous plantez pour augmenter votre capital forestier et continuer à améliorer votre écosystème.\n" );
			get().ftButtonCut = false;
		}
	}

	public static void msgButtonUproot()
	{
		if ( get().ftButtonUproot )
		{
			displayNotif(
				"Déraciner une souche",
				"Quelles conséquences",

				"Une fois l'outil <i>pelle</i> actif, vous devez sélectionner la case qui contient la souche que vous souhaitez déraciner, (maintenez <i>Shift</li> pour déraciner plusieurs souches).\n" +
				"Déraciner un arbre vous coûtera de l'argent, en fonction de l'espèce et de la taille de la souche.\n" +
				"En plus de cela, vous arracherez des plantes aux alentours pendant l'opération, et dégradera temporairement la qualité du sol.\n" +
				"\n" +
				"Cela permet cependant de libérer une case, et de supprimer la souche qui pouvait être néfaste pour les autres espèces.\n" );
			get().ftButtonUproot = false;
		}
	}

	public static void msgInsertArbre()
	{
		if ( get().ftInsertArbre )
		{
			displayNotif(
				"Effets un arbre",
				"Conséquences sur l'environnement",

				"A chaque fois que vous plantez un arbre, vous agissez sur l'environnement.\n" +
				"Certains arbres favoriseront/défavoriseront l'<b>humidité</b> des cases proches, la <b>luminosité</b>, et la <b>fertilité</b>.\n" +
				"Ceci permet de faire naturellement pousser des espèces d'herbe, de fleurs et de champignons, qui à leur tour agiront sur l'environnement.\n" +
				"C'est ainsi que vous allez progressivement forger l'écosystème de votre forêt.\n" );
			get().ftInsertArbre = false;
		}
	}

	public static void msgArbreMort()
	{
		if ( get().ftArbreMort )
		{
			displayNotif(
				"Un arbre est mort !",
				"Ne le laissez pas là",

				"Lorsque les conditions d'une case se sont détériorées, ou que l'arbre est arrivé en fin de vie, il arrive que celui-ci meurt.\n" +
				"Les différentes espèces d'arbre ont des durées de vie très variées.\n" +
				"Les espèces mortes vous causeront généralement préjudice, dégradant davantage les conditions des cases proches.\n" +
				"Pour palier à ce problème, il vous faut le couper dans les plus brefs délais. Utilisez l'outil <i>hache</i> pour procéder.\n" );
			get().ftArbreMort = false;
		}
	}

	public static void msgDiversite5()
	{
		if ( get().ftDiversite5 )
		{
			displayNotif(
				"Diversité",
				"Obtenir un écosystème complet",

				"Votre diversité a dépassé 5, bravo !\n" +
				"Chaque espèce de fleur/herbe/champignon/arbre augmentera votre diversité, à condition d'en avoir suffisamment en abondance.\n" +
				"\n" +
				"La diversité est un bon indicateur de la complexité de votre écosystème.\n" +
				"Avoir un bon coefficient de diversité vous permettra de faire apparaître encore plus d'espèces, qui nécessitent un environnement complexe.\n" +
				"A ce stade, vous devriez pouvoir planter des espèces plus intéressantes, comme des <b>hêtres</b>, qui sont très propices à l'apparition de fleurs proches.\n" );
			get().ftDiversite5 = false;
		}
	}

	public static void msgDiversite7()
	{
		if ( get().ftDiversite7 )
		{
			displayNotif(
				"Votre forêt prend vie",
				"Quelle est la prochaine étape",

				"Maintenant que vous avez compris comment établir un écosystème complet, il faut entretenir votre forêt.\n" +
				"Vous pouvez commencer à planter des <b>chênes</b>, qui sont un très bon placement pour votre capital forestier, puisque leur bois peut se vendre très cher. Attention à bien les entretenir, car ceux-ci sont assez fragiles.\n" +
				"Vous pouvez également tenter de planter des <b>saules</b>, qui ne s'entendent en général pas bien avec les autres espèces d'arbre, car ils apportent trop d'humidité. Ils contribuent cependant à améliorer la diversité de votre forêt, et sont des arbres très esthétiques.\n" );
			get().ftDiversite7 = false;
		}
	}

	public static void msgDiversite10()
	{
		if ( get().ftDiversite10 )
		{
			displayNotif(
				"Votre forêt est en pleine forme",
				"Félicitations !",

				"Vous avez atteint un coefficient de diversité de 10, bravo !\n" +
				"Vous êtes digne d'un bon ingénieur forestier.\n" +
				"Mais ne vous arrêtez pas là, une forêt demande beaucoup d'entretien pour survivre.\n" +
				"\n" +
				"Si vous souhaitez plus de challenge, essayez des niveaux de difficulté supérieure.\n" );
			get().ftDiversite10 = false;
		}
	}

	public static void msgDescription()
	{
		if ( get().ftDescription )
		{
			displayNotif(
				"Propriétés du sol",
				"Que représentent ces coefficients ?",

				"3 coefficients sont très importants pour déterminer l'impact sur les espèces de chaque case :\n" +
				" - L'<b>humidité</b> est essentielle pour faire pousser un peu toute sorte de plantes. Certaines espèces sont plus demandantes que d'autres, comme les champignons, d'autres auront besoin que le sol ne soit pas trop inondé.\n" +
				" - La <b>luminosite</b> est très importante pour les fleurs, mais moins pour les champignons.\n" +
				" - La <b>fertilité</b> est très représentative de l'aptitude des fleurs à pousser. Plus ce coefficient est élevé, plus il y aura de plantes, et plus les arbres pousseront vite.\n" +
				"\n" +
				"La principale difficulté réside dans le maniement de ces 3 paramètres à l'aide des différentes espèces d'arbres à votre disposition.\n" );
			get().ftDescription = false;
		}
	}

	public static void msgPolypore()
	{
		if ( get().ftPolypore )
		{
			displayNotif(
				"Des polypores sont apparus",
				"... mais qu'est-ce que c'est !?",

				"Le <i>polypore</i> est une espèce de champignon \"parasite\" qui vont tenter de se répendre et peuvent tuer vos espèces d'arbre.\n" +
				"Leur apparition est en générale dûe à un sol humide et peu fertile, accentuera ce phénomène. Il faut vous débarrasser de cette espèce au plus vite !\n" +
				"Tentez d'augmenter la fertilité des cases proches, et arraches les souches qui baissent la fertilité.\n" );
			get().ftPolypore = false;
		}
	}
}
