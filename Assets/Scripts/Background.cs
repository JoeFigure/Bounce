using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public GameObject gradient;
	public GameObject planets;
	public GameObject movingSprites;
	public GameObject mountains;

	Sprite planetsSprite;

	protected float speed{
		get{ return GameplayController.speed / 9 ;}
	}

	float planetSpriteX {
		get{ return planetsSprite.bounds.size.x * planets.transform.localScale.x; }
	}

	// Use this for initialization
	void Start () {
	
		PositionGradient ();
		CreateNewBackground ();
		planets.transform.position = new Vector2(-(GameplayController.screenWidth/2), GameplayController.screenHeight/2);
		mountains.transform.position = planets.transform.position;
		planetsSprite = planets.GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {

		if (GameplayController.ball.alive) {
			MoveSprites ();
		}

		if (planets.transform.position.x <= (GameplayController.zeroScreenX - planetSpriteX) + GameplayController.screenWidth) {
				CreateNewBackground ();
		}
	}

	void MoveSprites(){
		
			Transform[] transforms = movingSprites.GetComponentsInChildren<Transform> ();
			foreach (Transform t in transforms) {
				t.Translate (speed, 0, 0);
			}
	}

	void CreateNewBackground(){
		planets = Instantiate (planets) as GameObject;
		planets.transform.position = new Vector2(GameplayController.screenWidth, GameplayController.screenHeight/2);
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

		gradientTrans.localScale = new Vector2( GameplayController.screenWidth / width, height);
		gradientTrans.position = new Vector2 (GameplayController.zeroScreenX, GameplayController.screenHeight/2);
	}

	void DestroyObject(){
		Transform[] transforms = movingSprites.GetComponentsInChildren<Transform> ();
		foreach (Transform t in transforms) {
			if (t.position.x < GameplayController.zeroScreenX - planetSpriteX) {
				Destroy (t.gameObject);
			}
		}
	}
}
