using UnityEngine;
using System.Collections;

public class HillScript : MonoBehaviour {

	float speed = -5;

	public Camera camera;

	bool instantiated;

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
				float randomHeight = Random.Range (-2, 2);

				newHill.transform.position = new Vector2 (posX + sizeX, randomHeight);
				newHill.name = "hill";
				instantiated = true;
			}
		}
			
		if(positionInPixels.x < 0 - widthInPixels){
			Destroy (gameObject);
		}
	}
}
