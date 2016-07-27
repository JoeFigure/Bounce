using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {


	public SpikeScript spikes;

	public float speed{
		get{ return GameManager.instance.speed / 4 ;}
	}

	int enterFrame = 16;

	int offFrame = - 3;

	bool addedNew;

	// Use this for initialization
	void Start () {

		addedNew = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.instance.ball.alive) {
				
			transform.Translate (speed, 0, 0);
		}

		if (transform.position.x < offFrame && addedNew == false) {
			CreateNewBackground ();
			addedNew = true;
		}
	}

	void CreateNewBackground(){

		GameObject newBG = Instantiate (gameObject) as GameObject;
		newBG.transform.position = new Vector3(enterFrame, transform.position.y);

	}
}
