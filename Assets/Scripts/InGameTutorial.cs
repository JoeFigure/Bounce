using UnityEngine;
using System.Collections;

public class InGameTutorial : Background {


	new float speed {
		get{ return base.speed * 4f; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate(new Vector3( speed,0,0));

		if (transform.position.x < -20) {
			Destroy (gameObject);
		}

	}
}
