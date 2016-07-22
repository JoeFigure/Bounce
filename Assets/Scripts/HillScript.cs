using UnityEngine;
using System.Collections;

public class HillScript : MonoBehaviour {

	float levelHeight = 4.5f;

	float platformHeightMultiplier = 0.4f;

	public int lastHeight;

	int height;

	public int hillNumber;

	public Camera camera;

	bool instantiated;

	float speed{
		get{ return GameManager.instance.speed * Time.deltaTime;}
	}

	float sizeX {
		get {
			return coll.bounds.size.x;
		}
	}

	int widthInPixelsX2 {
		get{ return ((int)sizeX * 100) * 2; }
	}
		

	Vector2 positionInPixels {
		get{ return camera.WorldToScreenPoint (transform.position); }
	}

	Collider2D coll;

	public GameObject hillParent;

	// Use this for initialization
	void Start () {
		instantiated = false;

		coll = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (speed, 0, 0);

		CreateHill ();
					
		if(positionInPixels.x < (0 - widthInPixelsX2)){
			Destroy (gameObject);
		}

	}

	void CreateHill(){
		if (positionInPixels.x <= Screen.width) {
			if (!instantiated) {

				GameObject newHill = Instantiate (gameObject) as GameObject;
				float posX = transform.position.x;

				print (lastHeight);

				height = NewHeight (lastHeight);

				lastHeight = height;

				float finalHeight = (height * platformHeightMultiplier) - levelHeight;

				HillScript newHillScript = newHill.GetComponent<HillScript> ();

				newHillScript.lastHeight = height;
				newHillScript.hillNumber = hillNumber + 1;

				newHill.transform.position = new Vector2 (posX + sizeX, finalHeight);
				newHill.name = "hill";
				newHill.transform.parent = hillParent.transform;
				instantiated = true;
			}
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
