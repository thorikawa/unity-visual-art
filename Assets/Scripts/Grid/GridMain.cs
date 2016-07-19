using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

	public GameObject GridPointPrefab;
	private int numOfGridPoint = 8;
	private float a = 0.71f;
	private float b = 0.1f;
	private float c = 0.3333f;
	private float d = 1.333f;
	private float r = 3.0f;
	private float interval = 1.6f;

	// Use this for initialization
	void Start ()
	{
		int min = -numOfGridPoint / 2;
		int max = numOfGridPoint / 2;
		for (var x = min; x < max; x++) {
			for (var y = min; y < max; y++) {
				for (var z = min; z < max; z++) {
					var position = new Vector3 (x, y, z) * interval;
					Instantiate (GridPointPrefab, position, Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.time / 10.0f;
		float theta = a * t + b;
		float phi = c * t + d;
		float x1 = r * Mathf.Sin (theta) * Mathf.Cos (phi);
		float y1 = r * Mathf.Sin (theta) * Mathf.Sin (phi);
		float z1 = r * Mathf.Cos (theta) - 3f;
		var campos = Camera.main.transform.position;
		campos.Set (x1, y1, z1);
		Camera.main.transform.position = campos;
	}
}
