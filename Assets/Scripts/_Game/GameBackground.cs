using UnityEngine;
using System.Collections;

public class GameBackground : MonoBehaviour {

	void Start () {
		PositionGradient ();
	}

	void PositionGradient(){
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		//Transform gradientTrans = gradient.transform;

		transform.localScale = new Vector2( GameplayController.screenWidth / width, height);
		transform.position = new Vector2 (GameplayController.zeroScreenX, GameplayController.screenHeight/2);
	}
}
