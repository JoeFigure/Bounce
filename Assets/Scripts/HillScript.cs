using UnityEngine;
using System.Collections;

public class HillScript : MonoBehaviour {

	float levelHeight = 3.5f;

	public Camera camera;

	bool instantiated;

	float speed{
		get{ return GameManager.instance.speed;}
	}

	float sizeX {
		get {
			return coll.bounds.size.x;
		}
	}

	int widthInPixels {
		get{ return (int)sizeX * 100; }
	}

	float halfSizeX {
		get {
			return coll.bounds.extents.x;
		}
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

		transform.Translate (speed * Time.deltaTime, 0, 0);

		if (positionInPixels.x <= Screen.width) {
			if (!instantiated) {

				GameObject newHill = Instantiate (gameObject) as GameObject;
				float posX = transform.position.x;
				float platformHeight = PlatformHeight(Random.Range (0, 4));

				newHill.transform.position = new Vector2 (posX + sizeX, platformHeight);
				newHill.name = "hill";
				newHill.transform.parent = hillParent.transform;
				instantiated = true;
			}
		}
			
		if(positionInPixels.x < 0 - widthInPixels){
			Destroy (gameObject);
		}
	}

	float PlatformHeight(int number){

		float platformHeight = 0;

		switch (number) 
		{
		case 0:
			platformHeight = 0;
			break;
		case 1:
			platformHeight = -0.5f;
			break;
		case 2:
			platformHeight = -1f;
			break;
		default:
			break;
		}

		return platformHeight - levelHeight;
	}
}
