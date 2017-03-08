using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifMessage : MonoBehaviour
{
	public Text theTitle;
	public Text theSubtitle;
	public Text theContent;

	void Start ()
	{
		
	}

	void Update ()
	{
		Main.leaveSelection();
	}

	public void retour()
	{
		Main.unpause();
		gameObject.SetActive( false );
	}

	public void set( string title, string subtitle, string content )
	{
		theTitle.text = title;
		theSubtitle.text = subtitle;
		theContent.text = content;
	}
}
