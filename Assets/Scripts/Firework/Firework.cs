using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour
{

	public Vector3 Direction;
	public float Speed = 10f;
	public int Generation = 0;
	public float Energy = 1.0f;
	public Color Color = Color.white;
	public float Mass = 0.1f;

	private const int MAX_GENERATION = 2;
	private const float SPLIT_NUM = 50f;

	private float VanishThreshold = 0.1f;
	private float VanishingDuration = 1.4f;
	private bool vanished = false;
	private float createTime;
	private Rigidbody rb;
	private Renderer renderer;
	private ParticleSystem ps;

	// Use this for initialization
	void Start ()
	{
		this.createTime = Time.time;
		this.rb = GetComponent<Rigidbody> ();
		this.renderer = GetComponent<Renderer> ();
		this.renderer.material.SetColor ("_EmissionColor", Color);
		this.rb.mass = Mass;
		this.ps = GetComponent<ParticleSystem> ();
		ps.startColor = Color;
	}
	
	// Update is called once per frame
	void Update ()
	{

		float elapsed = Time.time - this.createTime;
		float dampedSpeed = this.Speed * Mathf.Exp (-elapsed * 5f);
		this.rb.velocity = Direction * dampedSpeed;

		if (vanished) {
			return;
		}
		ps.Emit (transform.position, Random.onUnitSphere * ps.startSpeed, ps.startSize, ps.startLifetime, ps.startColor);

		if (dampedSpeed < VanishThreshold) {
			vanished = true;
			StartCoroutine (Vanish ());

			if (this.Generation <= MAX_GENERATION) {
//				Debug.Log (string.Format ("{0}, {1}", Random.value, this.Energy));
				if (Random.value < this.Energy) {
					for (int i = 0; i < (int)(this.Energy * SPLIT_NUM); i++) {
						var theta = Random.Range (0f, 2f * Mathf.PI);
						var copy = Instantiate (gameObject, transform.position, Quaternion.identity) as GameObject;
						var f = copy.GetComponent<Firework> ();
						f.Direction = new Vector3 (Mathf.Cos (theta), Mathf.Sin (theta), 0);
						f.Speed = this.Energy * Random.Range (30f, 40f);
						f.Generation = this.Generation + 1;
						float newEnergy = this.Energy * Random.Range (0.4f, 0.6f);
						f.Energy = newEnergy;
						f.Color = Color.HSVToRGB (Random.value, 1.0f, 1.0f);
						f.transform.localScale = 0.4f * newEnergy * Vector3.one;
						f.Mass = 0.01f * newEnergy;
					}
				}
			}
		}
	}

	IEnumerator Vanish ()
	{
		var initialScale = this.transform.localScale;
		var vanishStartedTime = Time.time;
		while (true) {
			var diff = Time.time - vanishStartedTime;
			if (diff > VanishingDuration) {
				break;
			}
			var coeff = Mathf.Exp (-diff);
			var newColor = Color * coeff;
			this.renderer.material.SetColor ("_EmissionColor", newColor);
			this.transform.localScale = coeff * initialScale;
			yield return new WaitForFixedUpdate ();
		}
		Destroy (gameObject);
	}
}
