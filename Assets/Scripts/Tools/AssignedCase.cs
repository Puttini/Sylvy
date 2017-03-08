using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedCase : MonoBehaviour
{
	Case ca;

	public void set( Case c ) { ca = c; }
	public Case get(){ return ca; }
}
