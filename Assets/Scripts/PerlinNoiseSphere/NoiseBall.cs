using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoiseBall : MonoBehaviour
{

	private Vector3 initialPosition;
	private List<LineRenderer> lineStarting = new List<LineRenderer>();
	private List<LineRenderer> lineEnding = new List<LineRenderer>();
	private Renderer rend;
	public Color Color { get; set; }

	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		this.rend = GetComponent<Renderer> ();
		this.rend.material.SetColor("_EmissionColor", this.Color);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.time;
		float magnitude = t / 60f;
//		float magnitude = 0.2f;
		var s = initialPosition;
		var noise = (Mathf.PerlinNoise (s.x + t, s.z + t) - 0.5f) * magnitude + 1f;
		var newPos = s * noise;
		transform.position = newPos;

		foreach (var lr in lineStarting) {
			lr.SetPosition (0, newPos);
		}
		foreach (var lr in lineEnding) {
			lr.SetPosition (1, newPos);
		}
	}

	public void AddLineStarting(LineRenderer lr) {
		lineStarting.Add (lr);
	}

	public void AddLineEnding(LineRenderer lr) {
		lineEnding.Add (lr);
	}
}
