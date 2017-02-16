using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	/* 20 : xmin = -5, xmax = 5
	 *      ymin = 4 , ymax = 12
	 * 40 : xmin =-18, xmax = 18
	 *      ymin = 4 , ymax = 27
	 *
	 * xmax = 5 + (18-5)*(s-20)/20 = 5 + 13*s - 13 = 13s - 8
	 * ymax = 12 + (27-12)*(s-20)/20 = 12 + 15*s - 15 = 15s - 3 */

	public int border;
	public float speed;
	private float xmin, xmax;
	private float ymin, ymax;

	public GridManager gridManager;

	void Start ()
	{
		float xmax20 = 4;
		float ymax20 = 12;

		float xmax200 = 131;
		float ymax200 = 144;

		xmax = Mathf.Max( xmax200 + (xmax20 - xmax200) * (200 - gridManager.getSize()) / 180, 0 );
		xmin = -xmax;
		ymin = 4;
		ymax = Mathf.Max( ymax200 + (ymax20 - ymax200) * (200 - gridManager.getSize()) / 180, 4 );
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
