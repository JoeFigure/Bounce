using UnityEngine;
using System.Collections;

public class HillScript : MonoBehaviour {


	public int hillNumber;

	public int lastHeight;
	public GameObject hillParent;
	float levelHeight = 4.5f;
	float platformHeightMultiplier = 0.4f;
	bool instantiated;
	Collider2D coll;

	float speed{
		get{ return GameplayController.speed;}
	}

	float sizeX {
		get {
			return coll.bounds.size.x;
		}
	}

	void Start () {
		instantiated = false;
		coll = GetComponent<Collider2D> ();

	}
	
	void Update () {

		transform.Translate (speed, 0, 0);

		CreateHill ();
		DestroyObject ();
	}

	void CreateHill(){

		if(transform.position.x <= GameplayController.screenWidth){
			if (!instantiated) {

				GameObject newHill = Instantiate (gameObject) as GameObject;
				float posX = transform.position.x;
				int height = LevelDesign.HillHeight (hillNumber, NewHeight (lastHeight));

				float finalHeight = (height * platformHeightMultiplier) - levelHeight;

				newHill.transform.position = new Vector2 (posX + sizeX, finalHeight);

				newHill.GetComponent<HillScript> ().hillNumber =  hillNumber + 1;
				newHill.name = "hill";
				newHill.transform.parent = hillParent.transform;
				instantiated = true;

				lastHeight = height;
			}
		}
	}

	void DestroyObject(){
		if(transform.position.x < (GameplayController.zeroScreenX - (sizeX*2))){
			Destroy (gameObject);
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


}
