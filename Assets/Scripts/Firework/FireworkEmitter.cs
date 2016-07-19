using UnityEngine;
using System.Collections;

public class FireworkEmitter : MonoBehaviour
{

	public GameObject BallPrefab;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (Loop ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	IEnumerator Loop ()
	{
		while (true) {
			float x = Random.Range (-4f, 4f);
			var ball = Instantiate (BallPrefab, new Vector3 (x, -4.5f, 0), Quaternion.identity) as GameObject;
			var f = ball.GetComponent<Firework> ();
			f.Direction = Vector3.up;
			f.Speed = Random.Range (30f, 40f);
			f.Energy = 1f;

			float interval = Random.Range (1f, 6f);
			yield return new WaitForSeconds (interval);
		}
	}
}
