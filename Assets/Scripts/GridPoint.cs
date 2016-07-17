using UnityEngine;
using System.Collections;

public class GridPoint : MonoBehaviour
{

	private Rigidbody rb;
	private float xRandom;
	private float yRandom;
	private float zRandom;
	private float offset;
	private float magnitude = 0.1f;
	private Material material;
	private float colorChangeSpeed;
	private float hue;
	private Color emissionColor;
	public GameObject Lightning;
	private GameObject[] nearbyObjects;

	public float Hue {
		get {
			return hue;
		}
	}

	public Color EmissionColor {
		get {
			return emissionColor;
		}
	}

	// Use this for initialization
	void Start ()
	{
		this.rb = GetComponent<Rigidbody> ();
		this.offset = Random.Range (0f, 2 * Mathf.PI);
		var renderer = GetComponent<Renderer> ();
		this.material = renderer.material;
		this.colorChangeSpeed = Random.Range (0.25f, 0.5f);
		this.xRandom = Random.Range (4f, 7f);
		this.yRandom = Random.Range (4f, 7f);
		this.zRandom = Random.Range (4f, 7f);

		UpdateNearbyObjects ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.time;
		var velocity = rb.velocity;
		velocity.Set (magnitude * Mathf.Sin (t / xRandom), magnitude * Mathf.Sin (t / yRandom), magnitude * Mathf.Sin (t / zRandom));
		rb.velocity = velocity;
		float actualOffset = offset * Mathf.Pow (2, -t / 10f) + 0.2f;
		float actualColorChangeSpeed = (colorChangeSpeed * Mathf.Pow (2, -t / 10f) + 0.25f);
		this.hue = Mathf.Sin (actualOffset + t * actualColorChangeSpeed) / 2f + 0.5f;
//		colorChangeSpeed += Time.deltaTime / 20f;
		this.emissionColor = Color.HSVToRGB (this.hue, 1.0f, 1.0f);
		this.material.SetColor ("_EmissionColor", this.emissionColor * 0.2f);
//		this.material.SetColor ("_EmissionColor", Color.white * 0.1f);

		foreach (var other in nearbyObjects) {
			var gridPoint = other.GetComponent<GridPoint> ();
			if (gridPoint == null) {
				continue;
			}
			if (gridPoint == this) {
				continue;
			}
			float otherHue = gridPoint.Hue;
			int colorDistance = Mathf.Abs ((int)(otherHue * 1000f) - (int)(this.hue * 1000f));
			if (colorDistance < 16) {
				this.material.SetColor ("_EmissionColor", this.emissionColor * 2.0f);
				var lightning = Instantiate (Lightning);
				var line = lightning.GetComponent<LineRenderer> ();
				line.SetPosition (0, this.transform.position);
				line.SetPosition (1, other.transform.position);
			}
		}
	}

	void UpdateNearbyObjects ()
	{
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, 2.5f);
		nearbyObjects = new GameObject[hitColliders.Length];
		for (var i = 0; i < hitColliders.Length; i++) {
			nearbyObjects [i] = hitColliders [i].gameObject;
		}
	}
}
