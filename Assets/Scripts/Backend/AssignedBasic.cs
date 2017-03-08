using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedBasic : MonoBehaviour
{
	public BasicPlant prop;

	public void Start()
	{
		BasicPlant b = gameObject.AddComponent<BasicPlant>();
		b.fromPrefab(prop);
		b.init();
	}
}
