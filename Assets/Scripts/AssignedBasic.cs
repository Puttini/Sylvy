using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedBasic : MonoBehaviour, CaseActor
{
	public BasicPlant basic;

	public void Start()
	{
		basic.Start();
	}

	public void updateCase( Case c )
	{
		basic.updateCase( c );
	}
}
