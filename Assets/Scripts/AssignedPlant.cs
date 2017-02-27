using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedPlant : MonoBehaviour
{
	Plant plant;

	public void set( Plant p ) { plant = p; }
	public Plant get(){ return plant; }
}
