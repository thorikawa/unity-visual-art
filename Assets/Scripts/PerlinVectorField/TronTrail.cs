using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class TronTrail : MonoBehaviour
{

	public Vector3 InitialDirection = new Vector3 (1f, 0f, 0f);
	public float INITIAL_SPEED = 3.0f;
	private Rigidbody rb;
	public float NOISE_SCALE = 0.1f;
	public float EFFECT_SCALE = 20f;
	public float GRAVITY_FACTOR = 0f;
	public float GRAVITY_DISTANCE_SCALE = 0.2f;
	public GameObject engineObject;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = InitialDirection * INITIAL_SPEED;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var t = Time.time;
		float dist = transform.position.magnitude;
		var gravity = GRAVITY_FACTOR *  Mathf.Pow(dist * GRAVITY_DISTANCE_SCALE, 2f) * (-rb.position.normalized);

		Vector3 noise = Vector3.zero;
		if (engineObject != null) {
			noise = Perlin.NoiseVec (engineObject.transform.position * NOISE_SCALE);
			transform.position = noise * 10f;
		} else {
//			noise = Perlin.NoiseVec (transform.position * NOISE_SCALE);
//			rb.AddForce (noise * EFFECT_SCALE + gravity);
//			rb.velocity = rb.velocity.normalized * 10f;
//			rb.velocity = Random.onUnitSphere;
		}
	}
}
