using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour {



	int spikeInitialXPos = 7;
	int hillCreationXPos = 9;

	int firstHillTime = 5;

	public float scaleMult;

	public float largestSpikeDistance{ get; set; }
	public float midSpikeDistance{ get; set; }
	public float smallestSpikeDistance{ get; set; }

	int lastSpikeAmount;
	float timer;

	float hillTimer;

	public GameObject spike;
	public GameObject spikesParent;

	public GameObject hill;

	public Transform floorTrans;

	public Transform hillTrans;

	float doubleSpikeInitialXpos{
		get { return spikeInitialXPos + spikeSize.x; }
	}

	float tripleSpikeInitialXpos{
		get { return spikeInitialXPos + (spikeSize.x * 2); }
	}

	float floorHeightHalf {
		get{ return  floorTrans.localScale.y / 2;}
	}

	float hillHeightHalf{
		get{ return hillHeight / 2; }
	}

	float hillHeight{
		get{ return  hillTrans.localScale.y;}
	}

	float hillBaseHeight{
		get{ return floorTrans.position.y + floorHeightHalf + hillHeightHalf; }
	}

	Vector3 spikeSize {
		get {
			float size = 0.35f;
			float temp = scaleMult * size;
			return new Vector3 (temp, temp, 0);
		}
	}

	public GameObject hillParent;

	Transform[] hillTransforms;



	// Use this for initialization
	void Start () {

		largestSpikeDistance = 1.5f;
		midSpikeDistance = 0.9f;
		smallestSpikeDistance = 0.4f;
		scaleMult = 1f;
		Random.seed = 2;
		timer = 1;
		hillTimer = firstHillTime;

	}
	
	// Update is called once per frame
	void Update () {

		if (timer <= 0) {
			CreateRandomSpike ();
		}

		if (hillTimer <= 0) {
			CreateHill ();
		}

		if (GameManager.instance.gameStarted) {
			timer -= Time.deltaTime;
			hillTimer -= Time.deltaTime;
		}
	}

	void CreateRandomSpike(){

		int spikeType = Random.Range (0, 100);

		if (spikeType < 50 || GameManager.instance.gameTime < 7) {
			CreateSpike (spikeInitialXPos, true);
			lastSpikeAmount = 1;
			return;
		} 
		if (spikeType < 80) {
			CreateSpike (spikeInitialXPos, false);
			CreateSpike (doubleSpikeInitialXpos, true);
			lastSpikeAmount = 2;
			return;
		} 
		if (spikeType < 100) {
			CreateSpike (spikeInitialXPos, false);
			CreateSpike (doubleSpikeInitialXpos, false);
			CreateSpike (tripleSpikeInitialXpos, true);
			lastSpikeAmount = 3;
		}
	}

	void CreateSpike(float spikeInitialXpos , bool pointSpike){
		GameObject newSpike = Instantiate (spike) as GameObject;
		newSpike.SetActive (true);
		newSpike.transform.position = new Vector3(spikeInitialXpos, LandHeight(spikeInitialXpos));
		newSpike.transform.parent = spikesParent.transform;
		newSpike.transform.localScale = spikeSize;
		newSpike.GetComponent<SpikeScript> ().pointSpike = pointSpike;
		ResetSpikeTimer ();
	}

	void CreateHill(){
		hillTrans.localScale = new Vector3( 3.5f ,scaleMult * 0.7f ,scaleMult);
		GameObject newHill = Instantiate (hill) as GameObject;
		newHill.SetActive (true);
		newHill.transform.position = new Vector3(hillCreationXPos, hillBaseHeight);
		newHill.transform.parent = hillParent.transform;
		ResetHillTimer ();
	}

	void ResetSpikeTimer(){

		int spikeDistanceProbability = Random.Range (0, 100);

		if (spikeDistanceProbability < 70 && lastSpikeAmount < 3) {
			timer = Random.Range (smallestSpikeDistance, midSpikeDistance);
			return;
		}
		if (spikeDistanceProbability < 100) {
			timer = Random.Range (smallestSpikeDistance, largestSpikeDistance);
		}
	}

	void ResetHillTimer(){

		float gameTime = GameManager.instance.gameTime;

		if (gameTime < 11) {
			hillTimer = Random.Range (1.2f, 6);
			print ("easy");
		} else {
			hillTimer = Random.Range (1.2f, 2.5f);
			print ("difficult");
		}
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
