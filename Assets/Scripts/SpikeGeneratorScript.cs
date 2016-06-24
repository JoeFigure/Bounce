using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour {



	public GameObject spike;
	public GameObject spikesParent;

	public Transform floorTrans;

	float spikeSizeHalf;
	float floorHeightHalf;

	public bool gameStarted;

	float timer;

	// Use this for initialization
	void Start () {

		gameStarted = false;
		timer = 1;

		spikeSizeHalf =spike.transform.localScale.y / 2;
		floorHeightHalf = floorTrans.localScale.y / 2;
	}
	
	// Update is called once per frame
	void Update () {


		if (timer <= 0) {
			CreateSpike ();
		}

		if (gameStarted) {

			timer -= Time.deltaTime;
		}
	}

	void CreateSpike(){

		float spikeBaseHeight = floorTrans.position.y + floorHeightHalf + spikeSizeHalf;

		GameObject newSpike = Instantiate (spike) as GameObject;
		newSpike.SetActive (true);
		newSpike.transform.position = new Vector3(7, spikeBaseHeight);
		newSpike.transform.parent = spikesParent.transform;

		ResetTimer ();
	}

	void ResetTimer(){

		timer = 3;
	}


}
