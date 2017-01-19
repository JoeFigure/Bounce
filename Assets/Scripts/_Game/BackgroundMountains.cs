using UnityEngine;
using System.Collections;

public class BackgroundMountains : MonoBehaviour {

	public GameObject mountainsPrefab, planetsPrefab;

	Vector2 planetStartPos, mountainStartPos;

	int backgroundCount = 2;

	float sizeX; 

	float posY = 1.05f;
	float planetsPosY = 5.3f;

	Transform[] mountains, planets;

	protected float speed{
		get{ return GameplayController.speed /3;}
	}

	// Use this for initialization
	void Start () {

		GameplayController.bGMountains = this;

		mountains = new Transform[backgroundCount];
		planets = new Transform[backgroundCount];

		sizeX = mountainsPrefab.GetComponent<SpriteRenderer>().bounds.size.x ;

	}
	
	// Update is called once per frame
	void Update () {
		if (GameplayController.ball.alive) {
			Move();
		}
	}

	public void Init(){
		DestroyAll ();

		Pool ();
		InitialPos ();
	}

	void Move(){
		for(int i = 0; i < backgroundCount ; i++){

			//If in killzone (far offscreen) Move to end of Hills
			if (mountains[i].position.x < (GameplayController.zeroScreenX - (sizeX))) {
				int lastPos = i > 0 ? i-1 : backgroundCount - 1;
				mountains[i].position = new Vector2( mountains[lastPos].position.x + sizeX, posY);
			}
			//Planets
			if (planets[i].position.x < (GameplayController.zeroScreenX - (sizeX))) {
				int lastPos = i > 0 ? i-1 : backgroundCount - 1;
				planets[i].position = new Vector2( planets[lastPos].position.x + sizeX, planetsPosY);
			}

			mountains[i].Translate (speed * Vector3.right);
			planets[i].Translate (speed/2 * Vector3.right);
		}
	}

	public void DestroyAll(){
		Transform[] ii = GetComponentsInChildren<Transform> ();
		foreach(Transform i in ii){
			if (i.gameObject == gameObject)
				continue;
			Destroy (i.gameObject);
		}
	}

	void Pool(){
		for(int i = 0; i < backgroundCount ; i++){
			GameObject newMountain = Instantiate (mountainsPrefab, transform) as GameObject;
			mountains [i] = newMountain.transform;
			//Planet pool
			GameObject newPlanets = Instantiate (planetsPrefab, transform) as GameObject;
			planets [i] = newPlanets.transform;
		}
	}

	void InitialPos(){
		for(int i = 0; i < backgroundCount ; i++){
			mountains [i].position = new Vector2( (i * sizeX )- 3/*CHANGE THIS*/,posY);
			//Planet Pos
			planets [i].position = new Vector2( (i * sizeX )- 3/*CHANGE THIS*/,planetsPosY);

		}
	}
		
}
