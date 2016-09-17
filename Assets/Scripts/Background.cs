using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public GameObject gradient;
	public GameObject planets;
	public GameObject movingSprites;
	public GameObject mountains;

	Sprite planetsSprite;

	float speed{
		get{ return GameplayController.speed / 9 ;}
	}

	float planetSpriteX {
		get{ return planetsSprite.bounds.size.x * planets.transform.localScale.x; }
	}

	float screenHeight {
		get{ return Camera.main.orthographicSize * 2; }
	}

	float screenWidth{
		get{	
			float worldScreenHeight = Camera.main.orthographicSize * 2;
			float screenWidth = worldScreenHeight / Screen.height * Screen.width;
			return screenWidth;
		}
	}

	float zeroScreenX {
		get{ return (screenWidth / 2) * -1; }
	}
		
	// Use this for initialization
	void Start () {
	
		PositionGradient ();

		CreateNewBackground ();

		planets.transform.position = new Vector2(-(screenWidth/2), screenHeight/2);

		mountains.transform.position = planets.transform.position;

		planetsSprite = planets.GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {

		MoveSprites ();

		if (planets.transform.position.x <= (zeroScreenX - planetSpriteX) + screenWidth) {
				CreateNewBackground ();
		}
	}

	void MoveSprites(){
		if (GameplayController.ball.alive) {

			Transform[] transforms = movingSprites.GetComponentsInChildren<Transform> ();
			foreach (Transform t in transforms) {
				t.Translate (speed, 0, 0);
				if (t.position.x < zeroScreenX - planetSpriteX) {
					//Destroy (t.gameObject);
				}
			}
		}
	}

	void CreateNewBackground(){
		planets = Instantiate (planets) as GameObject;
		planets.transform.position = new Vector2(screenWidth, screenHeight/2);
		planets.transform.SetParent(movingSprites.transform);

		mountains = Instantiate (mountains) as GameObject;
		mountains.transform.position = planets.transform.position;
		mountains.transform.SetParent(movingSprites.transform);

	}

	void PositionInitialBG(){
		if (GameplayController.gameTime < 0.5) {
		}
	}

	void PositionGradient(){
		SpriteRenderer sr = gradient.GetComponent<SpriteRenderer>();
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		Transform gradientTrans = gradient.transform;

		gradientTrans.localScale = new Vector2( screenWidth / width, height);
		gradientTrans.position = new Vector2 (zeroScreenX, screenHeight/2);
	}
}
