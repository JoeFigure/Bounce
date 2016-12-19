using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HillScript : MonoBehaviour {


	public static int hillNumber;

	public int lastHeight;

	float levelHeight = 1;
	float platformHeightMultiplier = 0.4f;

	Sprite[] hillSprites = new Sprite[4];
	Sprite[] icehillSprites = new Sprite[4];
	Sprite icehillDoubleSprite;

	public GameObject hillPrefab;

	public Transform[] hills1 = new Transform[20];

	float sizeX; 

	float speed{
		get{ return GameplayController.speed;}
	}

	float height{
		get{
			int temp = LevelDesign.HillHeight (hillNumber, NewHeight (lastHeight));
			float finalHeight = (temp * platformHeightMultiplier) - levelHeight;
			lastHeight = temp;
			return finalHeight;
		}
	}

	void LoadSprites(){
		hillSprites[0] = Resources.Load<Sprite>("hills/hill1");
		hillSprites[1] = Resources.Load<Sprite>("hills/hill2");
		hillSprites[2] = Resources.Load<Sprite>("hills/hill3");
		hillSprites[3] = Resources.Load<Sprite>("hills/hill4");

		icehillSprites[0] = Resources.Load<Sprite>("iceHills/icehill1");
		icehillSprites[1] = Resources.Load<Sprite>("iceHills/icehill2");
		icehillSprites[2] = Resources.Load<Sprite>("iceHills/icehill3");
		icehillSprites[3] = Resources.Load<Sprite>("iceHills/icehill4");

		icehillDoubleSprite = Resources.Load<Sprite>("iceHillDouble");
	}


	void Start () {
		GameplayController.hills = this;
		sizeX = hillPrefab.GetComponent<SpriteRenderer>().bounds.size.x ;
		LoadSprites ();
	}
	
	void Update () {
		if (GameplayController.ball.alive) {
			MoveHills ();
		}
	}


	int NewHeight(int lastHeight){

		int output;
		switch(lastHeight){
		case 0:
			output = Random.Range (0, 2);
			break;
		case 1:
			output = Random.Range (0, 3);
			break;
		case 2:
			output = Random.Range (1, 3);
			break;
		default:
			output = 1;
			break;
		}

		return output;
	}

	void SwapSprite(GameObject hill, int height){
		int randHill = Random.Range (0, 3);
		if (height == 0) {
			hill.GetComponent<SpriteRenderer> ().sprite = hillSprites [randHill];
		} else if(height == 1){
			hill.GetComponent<SpriteRenderer> ().sprite = icehillSprites [randHill];
		}else if(height == 2){
			hill.GetComponent<SpriteRenderer> ().sprite = icehillDoubleSprite;
		}
	}

	public void Init(){
		DestroyAllHills ();

		hillNumber = 0;
		lastHeight = 0;

		PoolHills ();
		InitialPositions ();

	}

	void PoolHills(){
		for(int i = 0; i < 20 ; i++){
			GameObject newHill = Instantiate (hillPrefab, transform) as GameObject;
			hills1 [i] = newHill.transform;
			hills1[i].gameObject.GetComponent<Hill> ().hillNumber = hillNumber;
			SwapSprite (hills1 [i].gameObject, lastHeight);
			hillNumber++;
		}
	}

	void InitialPositions(){
		for(int i = 0; i < hills1.Length ; i++){
			hills1 [i].position = new Vector2( (i * sizeX )- 3/*CHANGE THIS*/,-1);
			//SwapSprite (hills1 [i].gameObject, lastHeight);
		}
	}

	void MoveHills(){
		for(int i = 0; i < hills1.Length ; i++){

			//If in killzone (far offscreen) Move to end of Hills
			if (hills1[i].position.x < (GameplayController.zeroScreenX - (20))) {
				hillNumber++;
				hills1 [i].gameObject.GetComponent<Hill> ().hillNumber = hillNumber;
				int lastPos = i > 0 ? i-1 : hills1.Length - 1;
				hills1[i].position = new Vector2( hills1[lastPos].position.x + sizeX,height);
				SwapSprite (hills1 [i].gameObject, lastHeight);
			}

			hills1[i].Translate (GameplayController.speed * Vector3.right);

		}
	}

	//End of new section

	public void DestroyAllHills(){
		Transform[] hills = GetComponentsInChildren<Transform> ();
		foreach(Transform i in hills){
			if (i.gameObject == gameObject)
				continue;
				Destroy (i.gameObject);
		}
	}


	public float HillHeightAtX(float xPos){
		foreach(Transform i in hills1){
			if (i.gameObject == gameObject) {
				continue;
			}
			Bounds hillBounds = i.GetComponent<SpriteRenderer> ().bounds;
			if (xPos < hillBounds.max.x && xPos > hillBounds.min.x) {
				return i.position.y;
			}
		}
		return 0;
	}
}
