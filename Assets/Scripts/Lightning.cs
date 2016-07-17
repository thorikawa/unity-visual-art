using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
	private float createdTime = 0f;

	// Use this for initialization
	void Start ()
	{
		this.createdTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - createdTime > 0.016f) {
			Destroy (gameObject);
		}
	}
}
