using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour
{

	float spikeScale = 0.35f;

	float timer;
	public GameObject spikePrefab;

	public Transform[] spikes = new Transform[10];

	int currentSpike;

	float spikeWidth;


	public float inversePercFullSpeed {
		get { 
			float gameSpeed = GameplayController.gameTimePercentOfFullSpeed;
			float inversePerc = 1 - gameSpeed;
			return inversePerc;
		}
	}

	void Start (){
		spikeWidth = (spikePrefab.GetComponent<SpriteRenderer> ().sprite.bounds.size.x) * spikeScale;
		GameplayController.spikes = this;
	}
	
	void Update (){
		if (GameplayController.ball.alive) {
			Timer ();
			MoveSpikes ();
		}
	}

	void Timer(){
		timer -= Time.smoothDeltaTime;
		if (timer <= 0) {
			LaunchSpike(true,false);
		}
	}

	void ResetSpikeTimer (){
		const float smallestDist = 0.05f;
		const float largestDist = 0.3f;

		float lowerExtra = 0.7f * inversePercFullSpeed;
		float higherExtra = 1.5f * inversePercFullSpeed;
			
		timer = Random.Range (smallestDist + lowerExtra, largestDist + higherExtra);
	}

	public void Init(){
		DestroySpikes ();
		PoolSpikes ();
		timer = 3;
		currentSpike = 0;
	}

	public void DestroySpikes(){
		Transform[] spikes = GetComponentsInChildren<Transform> ();
		foreach(Transform i in transform){
			if (i.gameObject == gameObject)
				continue;
			Destroy (i.gameObject);
		}
	}

	void PoolSpikes(){
		for(int i = 0; i < spikes.Length ; i++){
			GameObject newSpike = Instantiate (spikePrefab, transform) as GameObject;
			spikes [i] = newSpike.transform;
			spikes [i].localScale = Vector3.one * spikeScale;
			spikes [i].position = new Vector2 (Screen.width + 100, 0);
		}
	}

	float initialPosX =  3;
	int spikeMultiple = 0;

	void LaunchSpike (bool pointSpike, bool multiple){

		if (multiple) {
			initialPosX += spikeWidth;
			spikes [currentSpike].GetComponent<Spike> ().pointSpike = false;
		} else {
			spikes [currentSpike].GetComponent<Spike> ().pointSpike = true;
		}

		spikes [currentSpike].transform.position = new Vector2 (initialPosX, GameplayController.hills.HillHeightAtX(initialPosX));
		currentSpike++;
		spikeMultiple++;

		if (currentSpike >= (spikes.Length - 1))
			currentSpike = 0;

		int r = Random.Range (0, 100);
		if (r > 60 && spikeMultiple < 4 && GameplayController.gameTime > 15) {
			LaunchSpike (false, true);
		}

		ResetSpikeTimer ();
		spikeMultiple = 0;
		initialPosX = 3;

	}
		
	void MoveSpikes(){
		foreach (Transform spike in spikes) {
			spike.Translate (GameplayController.speed * Vector3.right);
		}
	}

}