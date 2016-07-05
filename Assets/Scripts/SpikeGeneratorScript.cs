using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour {



	int spikeCreationXPos = 7;
	int hillCreationXPos = 9;

	public float scaleMult;

	public float spikeFreq;

	public GameObject spike;
	public GameObject spikesParent;

	public GameObject hill;

	public Transform floorTrans;

	public Transform hillTrans;

	public float floorHeightHalf {
		get{ return  floorTrans.localScale.y / 2;}
	}

	public float hillHeightHalf{
		get{ return hillHeight / 2; }
	}

	public float hillHeight{
		get{ return  hillTrans.localScale.y;}
	}

	public float hillBaseHeight{
		get{ return floorTrans.position.y + floorHeightHalf + hillHeightHalf; ;}
	}

	public GameObject hillParent;

	Transform[] hillTransforms;

	float timer;

	float hillTimer;

	// Use this for initialization
	void Start () {

		spikeFreq = 2.5f;
		scaleMult = 0.5f;
		Random.seed = 2;
		timer = 1;
		hillTimer = 2;

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
		newSpike.transform.localScale = new Vector3( scaleMult * 0.5f ,scaleMult * 0.5f,scaleMult);

		ResetTimer ();
	}

	void CreateHill(){

		hillTrans.localScale = new Vector3( 3.5f ,scaleMult * 0.7f ,scaleMult);
		GameObject newHill = Instantiate (hill) as GameObject;
		newHill.SetActive (true);
		newHill.transform.position = new Vector3(hillCreationXPos, hillBaseHeight);
		newHill.transform.parent = hillParent.transform;

		ResetHillTimer ();
	}

	void ResetTimer(){

		timer = Random.Range(0.5f,spikeFreq);
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
