using UnityEngine;
using System.Collections;

public class SpikeGeneratorScript : MonoBehaviour
{


	public float scaleMult;

	int lastSpikeAmount;
	float timer;

	public GameObject spike;
	public GameObject spikesParent;

	public Camera camera;

	public GameObject hillsParent;

	Transform[] hillsTransforms;

	public float inversePercFullSpeed {
		get { 
			float gameSpeed = GameManager.instance.gameTimePercentOfFullSpeed;
			float inversePerc = 1 - gameSpeed;
			return inversePerc;
		}
	}

	float spikeInitialXPos{
		get{ 
			float spikeWidth = spikeSize.x * 100;
			Vector2 screenSpace = camera.ScreenToWorldPoint (new Vector2( Screen.width + spikeWidth, 0));
			return screenSpace.x; 
		}
	}

	float doubleSpikeInitialXpos {
		get { return spikeInitialXPos + spikeSize.x; }
	}

	float tripleSpikeInitialXpos {
		get { return spikeInitialXPos + (spikeSize.x * 2); }
	}

	Vector3 spikeSize {
		get {
			float size = 0.35f;
			float temp = scaleMult * size;
			return new Vector3 (temp, temp, 0);
		}
	}
		
	// Use this for initialization
	void Start ()
	{
		scaleMult = 1f;
		Random.seed = 1;
		timer = 3;

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (timer <= 0) {
			CreateRandomSpike ();
		}


		if (GameManager.instance.ball.alive) {
			timer -= Time.deltaTime;
		}
	}

	void CreateRandomSpike ()
	{

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

	void CreateSpike (float spikeInitialXpos, bool pointSpike)
	{
		GameObject newSpike = Instantiate (spike) as GameObject;
		newSpike.SetActive (true);

		newSpike.transform.position = new Vector3 (spikeInitialXpos, LandHeight (spikeInitialXpos));
		newSpike.transform.parent = spikesParent.transform;
		newSpike.transform.localScale = spikeSize;
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