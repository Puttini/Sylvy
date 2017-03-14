using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Singleton class
	static CameraController instance;
	public static CameraController get() { return instance; }

	public int border;
	public float speed;
	private float xmin, xmax;
	private float ymin, ymax;
	float lastFrame;

	void Start()
	{
		instance = this;
	}

	public void init()
	{
		float x = transform.position.x;
		float y = transform.position.y;

		Camera camera = GetComponent<Camera>();
		Vector3 p = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
		Vector3 b = camera.ScreenToWorldPoint(new Vector3(0, 150, camera.nearClipPlane));

		Vector3 left = (Vector3)Isometric.IsoToScreen( new Vector3(0,0,GridManager.get().size+1) );
		xmin = left.x + x - p.x;
		Vector3 right = (Vector3)Isometric.IsoToScreen( new Vector3(GridManager.get().size+1,0,0) );
		xmax = right.x - x + p.x;
		Vector3 bottom = (Vector3)Isometric.IsoToScreen( new Vector3(0,0,0) );
		ymin = bottom.y + b.y - p.y;
		Vector3 top = (Vector3)Isometric.IsoToScreen( new Vector3(GridManager.get().size+2,0,GridManager.get().size+2) );
		ymax = top.y - y + p.y;

		lastFrame = (float)Time.time;
	}

	void Update ()
	{
		float x = transform.position.x;
		float y = transform.position.y;

		float frame = (float)Time.time;
		float dt = frame - lastFrame;

		if (Input.mousePosition.x <= border || Input.GetKey(KeyCode.LeftArrow))
			x -= speed * dt;
		if (Input.mousePosition.y <= border || Input.GetKey(KeyCode.DownArrow))
			y -= speed * dt;
		if (Input.mousePosition.x >= Screen.width - border || Input.GetKey(KeyCode.RightArrow))
			x += speed * dt;
		if (Input.mousePosition.y >= Screen.height - border || Input.GetKey(KeyCode.UpArrow))
			y += speed * dt;

		if (x <= xmin)
			x = xmin;
		if (y <= ymin)
			y = ymin;
		if (x >= xmax)
			x = xmax;
		if (y >= ymax)
			y = ymax;

		transform.position = new Vector2 (x, y);
		lastFrame = frame;
	}
}
