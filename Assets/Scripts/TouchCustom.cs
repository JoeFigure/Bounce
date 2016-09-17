using UnityEngine;
using System.Collections;

public class TouchCustom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HitBall ();
	}

	void HitBall ()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
			case TouchPhase.Began:
				//ball.beenHit = true;
				break;
			}
		}	
	}

}
