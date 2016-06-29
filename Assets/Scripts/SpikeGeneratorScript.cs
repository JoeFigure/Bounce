using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour {



	int spikeCreationXPos = 7;
	int hillCreationXPos = 9;

	public GameObject spike;
	public GameObject spikesParent;

	public GameObject hill;

	public Transform floorTrans;

	public Transform hillTrans;

	float floorHeightHalf;
	float hillHeightHalf;

	float hillHeight;

	public GameObject hillParent;
	Transform[] hillTransforms;

	float timer;

	float hillTimer;

	// Use this for initialization
	void Start () {

		Random.seed = 2;

		timer = 1;

		hillTimer = 2;

		//float heightOfSpike = 2;

		floorHeightHalf = floorTrans.localScale.y / 2;

		hillHeight = hillTrans.localScale.y;
		hillHeightHalf = hillHeight / 2;
	}
	
	// Update is called once per frame
	void Update () {

		if (timer <= 0) {
			CreateSpike ();
		}

		if (hillTimer <= 0) {
			CreateHill ();
		}

		if (GameManager.instance.gameStarted) {
			timer -= Time.deltaTime;
			hillTimer -= Time.deltaTime;
		}
	}

	void CreateSpike(){

		GameObject newSpike = Instantiate (spike) as GameObject;
		newSpike.SetActive (true);
		newSpike.transform.position = new Vector3(spikeCreationXPos, LandHeight(spikeCreationXPos));
		newSpike.transform.parent = spikesParent.transform;

		ResetTimer ();
	}

	void CreateHill(){

		float hillBaseHeight = floorTrans.position.y + floorHeightHalf + hillHeightHalf;

		GameObject newHill = Instantiate (hill) as GameObject;
		newHill.SetActive (true);
		newHill.transform.position = new Vector3(hillCreationXPos, hillBaseHeight);
		newHill.transform.parent = hillParent.transform;

		ResetHillTimer ();
	}

	void ResetTimer(){

		timer = Random.Range(0.5f,3f);
	}

	void ResetHillTimer(){

		hillTimer = Random.Range(2,6);
	}


	public float LandHeight(float inputObject){

		float floorHeight = floorTrans.position.y + floorHeightHalf;

		hillTransforms = hillParent.GetComponentsInChildren<Transform>();
		foreach (Transform trans in hillTransforms) {

			float hillCenter = trans.position.x;
			float hillRight = hillCenter + (trans.localScale.x / 2);
			float hillLeft = hillCenter - (trans.localScale.x / 2);

			if (inputObject < hillRight && inputObject > hillLeft) {

				return floorHeight + hillHeight;
			} 
		}
		return floorHeight;
	}
}
