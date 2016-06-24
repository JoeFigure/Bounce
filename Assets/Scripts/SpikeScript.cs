using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {


	float speed = -5;

	public Camera camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (speed * Time.deltaTime, 0, 0);

		Vector3 place = camera.WorldToScreenPoint (transform.position);

		if (place.x < 0) {
			GameManager.instance.currentPoints++;
			Destroy (gameObject);
		}
	}
}
