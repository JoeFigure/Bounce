using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

	float freq = 5;
	float height = 3;
	float hitPower = -0.5f;

	float posCos;
	float bouncingTime;
	float startPosY;

	bool beenHit;

	//public Transform floorTransform;


	void Start ()
	{

		beenHit = false;

		startPosY = transform.position.y;
	
	}

	void Update ()
	{

		if (!beenHit) {
			Bounce ();
		} else {
			HitBall ();
		}

		if (Input.GetButtonDown ("Jump")) {
			beenHit = true;
		}
	}

	void Bounce ()
	{

		bouncingTime += Time.deltaTime;

		posCos = Mathf.Abs (Mathf.Sin (freq * (bouncingTime)) * height);
		transform.position = new Vector2 (0, posCos);

	}

	void HitBall ()
	{

		transform.Translate (0, hitPower, 0);

		float yPos = transform.position.y;

		if (yPos < startPosY) {
			beenHit = false;
			bouncingTime = 0;
		}
	}
}

// ALT BOUNCE - transform.localPosition = new Vector3(0, Mathf.PingPong( Mathf.Sin(Time.time*4),4),0);