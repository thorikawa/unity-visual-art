using UnityEngine;
using System.Collections;

public class RotatingCamera : MonoBehaviour
{

	private const float R = 10.0f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		var t = Time.time;
		t = t / 10.0f;
		var x = R * Mathf.Cos (t);
		var z = R * Mathf.Sin (t);
		var pos = transform.position;
		pos.Set (x, pos.y, z);
		transform.position = pos;
		transform.LookAt (Vector3.zero);
	}
}
