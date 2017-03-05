using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlant : MonoBehaviour
{
	Plant plant;

	public void setPlant (Plant p)
	{
		plant = p;
		GameObject img = GameObject.Instantiate( p.sprite );
		img.transform.SetParent(transform);
		img.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0, 0 );
	}

	public void onClick()
	{
		Main.msgButtonPlant();

		PlantManager.get().setSelectedPlant( plant );
	}
}
