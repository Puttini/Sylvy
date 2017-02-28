using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public int border;
	public float speed;
	private float xmin, xmax;
	private float ymin, ymax;

	void Start ()
	{
		float x = transform.position.x;
		float y = transform.position.y;

		Vector3 left = (Vector3)Isometric.IsoToScreen( new Vector3(1,0,GridManager.get().size) );
		xmin = left.x + x;
		Vector3 right = (Vector3)Isometric.IsoToScreen( new Vector3(GridManager.get().size,0,1) );
		xmax = right.x + x;
		Vector3 bottom = (Vector3)Isometric.IsoToScreen( new Vector3(1,0,1) );
		ymin = bottom.y + y;
		Vector3 top = (Vector3)Isometric.IsoToScreen( new Vector3(GridManager.get().size,0,GridManager.get().size) );
		ymax = top.y + y;
	}

	void Update ()
	{
		float x = transform.position.x;
		float y = transform.position.y;

		if (Input.mousePosition.x <= border || Input.GetKey(KeyCode.LeftArrow))
			x -= speed;
		if (Input.mousePosition.y <= border || Input.GetKey(KeyCode.DownArrow))
			y -= speed;
		if (Input.mousePosition.x >= Screen.width - border || Input.GetKey(KeyCode.RightArrow))
			x += speed;
		if (Input.mousePosition.y >= Screen.height - border || Input.GetKey(KeyCode.UpArrow))
			y += speed;

		if (x <= xmin)
			x = xmin;
		if (y <= ymin)
			y = ymin;
		if (x >= xmax)
			x = xmax;
		if (y >= ymax)
			y = ymax;

		transform.position = new Vector2 (x, y);
	}
}
