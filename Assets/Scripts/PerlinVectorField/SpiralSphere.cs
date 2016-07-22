using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SpiralSphere : MonoBehaviour {

	Rigidbody rb;
	private const float R = 10f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();		
	}
	
	// Update is called once per frame
	void Update () {
		var t = Time.time;
		var theta = t / 1.619f;
		var phi = t / 0.438f;
		float x1 = R * Mathf.Sin (theta) * Mathf.Cos (phi);
		float y1 = R * Mathf.Sin (theta) * Mathf.Sin (phi);
		float z1 = R * Mathf.Cos (theta);
		var pos = transform.position;
		pos.Set (x1, y1, z1);
		transform.position = pos;
	}
}
