using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour
{

	public float scaleMult;

	[SerializeField]
	public float spikeScale;

	int lastSpikeAmount;
	float timer;
	public GameObject spike;
	public GameObject spikesParent;
	public GameObject hillsParent;
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
		timer = 6;
		spikeWidth = (spike.GetComponent<SpriteRenderer> ().sprite.bounds.size.x) * spikeScale;
		GameplayController.spikes = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timer <= 0) {
			CreateRandomSpike ();
		}

		if (GameplayController.ball.alive) {
			timer -= Time.deltaTime;
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
		GameObject newSpike = Instantiate (spike) as GameObject;
		newSpike.SetActive (true);

		newSpike.transform.position = new Vector3 (spikeInitialXpos, LandHeight (spikeInitialXpos));
		newSpike.transform.parent = spikesParent.transform;
		newSpike.transform.localScale = Vector3.one * spikeScale;
		newSpike.GetComponent<SpikeScript> ().pointSpike = pointSpike;
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

	public float LandHeight (float xPos)
	{
		GameObject currentHill = HillUnderneath (xPos);

		float hillHeight = 0;
		if (currentHill != null) {
			hillHeight = currentHill.GetComponent<BoxCollider2D> ().bounds.max.y;
		}
		return hillHeight;
	}

	public GameObject HillUnderneath (float xPos)
	{
		hillsTransforms = hillsParent.GetComponentsInChildren<Transform> ();
		foreach (Transform trans in hillsTransforms) {

			float hillCenter = trans.position.x;
			float hillRight = hillCenter + (trans.localScale.x / 2);
			float hillLeft = hillCenter - (trans.localScale.x / 2);

			if (xPos < hillRight && xPos > hillLeft) {
				return trans.gameObject;
			}
		}
		return null;
	}

}