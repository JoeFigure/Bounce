using UnityEngine;
using System.Collections;

public class TouchCustom : MonoBehaviour {

	public enum Swiped
	{
		Left,
		Right,
		None
	}

	public static bool tapped;

	static int swipeDistance = 17;
	static Vector2 startSwipePos;
	static Vector2 direction;
	static bool directionChosen;
	public static Swiped swiped;

	// Use this for initialization
	void Start () {
	
		swiped = Swiped.None;
	}
	
	// Update is called once per frame
	void Update () {
		Tap ();

	}

	public static void Tap ()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
			case TouchPhase.Began:
				tapped = true;
				break;
			default:
				tapped = false;
				break;
			} 
		} 
	}

	public static void Swipe ()
	{

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			switch (touch.phase) {
			case TouchPhase.Began:
				direction = Vector2.zero;
				startSwipePos = touch.position;
				break;


			case TouchPhase.Ended:

				direction = touch.position - startSwipePos;

				if (direction.x < swipeDistance && direction.x > -swipeDistance && direction.y < swipeDistance && direction.y > -swipeDistance) {
					swiped = Swiped.None;
				} else {
					//swipe
					if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
						int temp = Mathf.Clamp ((int)direction.x, -1, 1);
						swiped = temp < 0 ? Swiped.Left : Swiped.Right;
					} else {
						//Vertical swipe
					}
				}

				break;
			}
		} else {
		//No fingers touching
			swiped = Swiped.None;
		}
	}


}
