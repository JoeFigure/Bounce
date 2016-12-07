using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour
{

	float scaleMult = 0;

	float spikeScale = 0.35f;

	int lastSpikeAmount;
	float timer;
	public GameObject spikePrefab;
	Transform[] hillsTransforms;

	float spikeWidth;

	public float inversePercFullSpeed {
		get { 
			float gameSpeed = GameplayController.gameTimePercentOfFullSpeed;
			float inversePerc = 1 - gameSpeed;
			return inversePerc;
		}
	}

	float spikeInitialXPos{
		get{ return GameplayController.screenWidth + spikeWidth;}
	}

	float doubleSpikeInitialXpos {
		get { return spikeInitialXPos + spikeWidth; }
	}

	float tripleSpikeInitialXpos {
		get { return spikeInitialXPos + (spikeWidth * 2); }
	}


	void Start ()
	{
		
		spikeWidth = (spikePrefab.GetComponent<SpriteRenderer> ().sprite.bounds.size.x) * spikeScale;
		GameplayController.spikes = this;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (GameplayController.ball.alive) {
			Timer ();
		}
	}

	void Timer(){
		timer -= Time.deltaTime;
		if (timer <= 0) {
			CreateRandomSpike ();
		}
	}

	void CreateRandomSpike ()
	{

		int spikeType = Random.Range (0, 100);

		if (spikeType < 50 || LevelDesign.FirstSpikes()) {
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

	void CreateSpike (float spikeInitialXpos, bool pointSpike)
	{
		GameObject newSpike = Instantiate (spikePrefab,transform) as GameObject;
		newSpike.SetActive (true);

		newSpike.transform.position = new Vector3 (spikeInitialXpos, GameplayController.hills.HillHeightAtX(spikeInitialXpos)); //LandHeight (spikeInitialXpos));
		newSpike.transform.localScale = Vector3.one * spikeScale;
		newSpike.GetComponent<Spike> ().pointSpike = pointSpike;//Gives the player a point if avoided
		ResetSpikeTimer ();
	}


	void ResetSpikeTimer ()
	{

		const float smallestDist = 0.05f;
		const float largestDist = 0.3f;

		float lowerExtra = 0.7f * inversePercFullSpeed;
		float higherExtra = 1.5f * inversePercFullSpeed;
			
		timer = Random.Range (smallestDist + lowerExtra, largestDist + higherExtra);
	}

	public void Init(){
		DestroyAllSpikes ();
		timer = LevelDesign.firstSpikeTime;
		Spike.spikeNumber = 0;
	}

	public void DestroyAllSpikes(){
		Transform[] spikes = GetComponentsInChildren<Transform> ();
		foreach(Transform i in spikes){
			if (i.gameObject == gameObject)
				continue;

			Destroy (i.gameObject);
		}
	}

}