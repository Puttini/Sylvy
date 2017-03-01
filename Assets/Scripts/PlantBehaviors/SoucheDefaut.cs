using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class SoucheDefaut : MonoBehaviour
{
	float age;

	public float getAge() { return age; }
	public void setAge( float a ) { age = a; } 
	public void setScale( float scale ) // Utilisée une seule fois
	{
		IsoTransform iso = GetComponent<IsoTransform>();
		iso.Size *= scale;
		iso.Position = new Vector3( iso.Position.x, scale * iso.Position.y, iso.Position.z );
		transform.localScale *= scale;
	}
}
