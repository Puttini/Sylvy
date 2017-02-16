using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
	public Text text;
	public int initialAccount;

	// Use this for initialization
	void Start ()
	{
		Main.get ().money = initialAccount;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Main.get().money++;
		text.text = Main.get ().money.ToString();
	}
}
