using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinNoiseSphere : MonoBehaviour
{

	public GameObject ballPrefab;
	public GameObject linePrefab;
	private const float R = 5f;
	private const int NUM = 10000;
	private const float THRESHOLD = 0.0f;
	private const float FREQUENCY = 3f;
	private readonly int[] startIndexes = { 0, 1, 2 };
	private readonly int[] endIndexes = { 1, 2, 0 };

	// Use this for initialization
	void Start ()
	{
		Init (true, NUM / 2);
//		Init (false, NUM / 2);
	}

	void Init (bool upside, int num)
	{
		List<Vector2> points2d = new List<Vector2> ();
		List<GameObject> balls = new List<GameObject> ();
		int count = 0;
		while (true) {
			var rand = Random.onUnitSphere;
			rand.y = Mathf.Abs (rand.y);
			if (!upside) {
				rand.y = -rand.y;
			}
			var noise = Mathf.PerlinNoise (FREQUENCY * rand.x, FREQUENCY * rand.z);
			if (noise > THRESHOLD) {
				var pos = R * rand;
				var obj = Instantiate (ballPrefab, pos, Quaternion.identity) as GameObject;
				var cv = (noise - THRESHOLD) / (1f - THRESHOLD);
				obj.GetComponent<NoiseBall> ().Color = Color.HSVToRGB (cv, 1f, cv);
				balls.Add (obj);
				points2d.Add (new Vector2 (rand.x, rand.z));
				if (++count >= num) {
					break;
				}
			}
		}

		Debug.Log (string.Format ("{0} objectes instantiated.", points2d.Count));

		var triangulator = new Triangulator ();
		var triangles = triangulator.TriangulatePolygon (points2d.ToArray ());

		for (var i = 0; i < triangles.Length; i += 3) {
			for (int j = 0; j < 3; j++) {
				var l = triangles [i + startIndexes [j]];
				var r = triangles [i + endIndexes [j]];

				var line = Instantiate (linePrefab, Vector3.zero, Quaternion.identity) as GameObject;
				var lineRenderer = line.GetComponent<LineRenderer> ();

				lineRenderer.SetPosition (0, balls [l].transform.position);
				lineRenderer.SetPosition (1, balls [r].transform.position);
				balls [l].GetComponent<NoiseBall> ().AddLineStarting (lineRenderer);
				balls [r].GetComponent<NoiseBall> ().AddLineEnding (lineRenderer);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		var t = Time.time;
		var x = 10f * Mathf.Cos (t / 10f);
		var z = 10f * Mathf.Sin (t / 10f);
		var campos = Camera.main.transform.position;
		campos.x = x;
		campos.z = z;
		campos.y = 8f;

		Camera.main.transform.position = campos;
		Camera.main.transform.LookAt (Vector3.zero);
	}
}
