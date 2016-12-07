using UnityEngine;
using System.Collections;

public class HillScript : MonoBehaviour {


	public static int hillNumber;

	public int lastHeight;

	float levelHeight = 1;
	float platformHeightMultiplier = 0.4f;

	Sprite[] hillSprites = new Sprite[4];
	Sprite[] icehillSprites = new Sprite[4];

	public GameObject hillPrefab;
	public Transform[] hills = new Transform[0];

	float sizeX; 

	float speed{
		get{ return GameplayController.speed;}
	}


	void Start () {
		GameplayController.hills = this;

		sizeX = hillPrefab.GetComponent<SpriteRenderer>().bounds.size.x ;

		hillSprites[0] = Resources.Load<Sprite>("hills/hill1");
		hillSprites[1] = Resources.Load<Sprite>("hills/hill2");
		hillSprites[2] = Resources.Load<Sprite>("hills/hill3");
		hillSprites[3] = Resources.Load<Sprite>("hills/hill4");

		icehillSprites[0] = Resources.Load<Sprite>("iceHills/icehill1");
		icehillSprites[1] = Resources.Load<Sprite>("iceHills/icehill2");
		icehillSprites[2] = Resources.Load<Sprite>("iceHills/icehill3");
		icehillSprites[3] = Resources.Load<Sprite>("iceHills/icehill4");

	}
	
	void Update () {
		CreateNewHill ();
		MoveHills ();
	}

	void CreateNewHill(){

		foreach (Transform hill in hills) {

			if (hill == transform) {
				continue;
			}

			Hill originalHill = hill.gameObject.GetComponent<Hill> ();

			if (hill.position.x <= GameplayController.screenWidth) {

				if (!originalHill.instantiated) {

					hillNumber++;

					int height = LevelDesign.HillHeight (hillNumber, NewHeight (lastHeight));
					float finalHeight = (height * platformHeightMultiplier) - levelHeight;

					GameObject newHill = Instantiate (hillPrefab, transform) as GameObject;
					newHill.transform.position = new Vector2 (hill.position.x + sizeX, finalHeight);
					newHill.GetComponent<Hill> ().hillNumber = hillNumber;

					SwapSprite (newHill, height);

					lastHeight = height;

					originalHill.instantiated = true;

				}
			}
		}

		FillHillsArrayWithChildren ();
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
		} else {
			hill.GetComponent<SpriteRenderer> ().sprite = icehillSprites [randHill];
		}
	}

	public void DestroyAllHills(){
		Transform[] hills = GetComponentsInChildren<Transform> ();
		foreach(Transform i in hills){
			if (i.gameObject == gameObject)
				continue;

			Destroy (i.gameObject);
		}
		InitialSetup ();
	}

	void InitialSetup(){
		lastHeight = 0;
		GameObject newHill = Instantiate (hillPrefab,transform) as GameObject;
		FillHillsArrayWithChildren ();
		newHill.transform.position = new Vector2 (newHill.transform.position.x, -1);
	}

	void FillHillsArrayWithChildren(){
			hills = GetComponentsInChildren<Transform> ();
	}

	void MoveHills(){
		foreach(Transform i in hills){
			if (i.gameObject == gameObject) 
				continue;
			i.Translate (GameplayController.speed * Vector3.right);
			}
		}

	public float HillHeightAtX(float xPos){
		foreach(Transform i in hills){

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
