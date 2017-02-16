using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main
{
	private static Main instance;

	public int money;

	public Main()
	{
		
	}

	public static Main get()
	{
		if (Main.instance == null)
			Main.instance = new Main ();
		return Main.instance;
	}
}
