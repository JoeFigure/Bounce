using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {


	float speed = -1;

	int enterFrame = 18;

	int offFrame = 0;

	bool addedNew;

	// Use this for initialization
	void Start () {

		addedNew = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.instance.gameStarted) {
			transform.Translate (speed * Time.deltaTime, 0, 0);
		}

		if (transform.position.x < offFrame && addedNew == false) {
			CreateNewBackground ();
			addedNew = true;
		}
	}

	void CreateNewBackground(){

		//float spikeBaseHeight = floorTrans.position.y + floorHeightHalf + spikeSizeHalf;

		GameObject newBG = Instantiate (gameObject) as GameObject;
		//newSpike.SetActive (true);
		newBG.transform.position = new Vector3(enterFrame, transform.position.y);
		//newSpike.transform.parent = spikesParent.transform;

		//ResetTimer ();
	}
}
