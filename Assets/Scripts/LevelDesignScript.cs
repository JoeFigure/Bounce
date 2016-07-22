using UnityEngine;
using System.Collections;

public class LevelDesignScript : MonoBehaviour {

	GameObject hillsParent;

	Transform[] hills;

	// Use this for initialization
	void Start () {
	
		hillsParent = GameObject.Find("GamePlay/Hills");

	}
	
	// Update is called once per frame
	void Update ()
	{

		hills = hillsParent.GetComponentsInChildren<Transform> ();
		foreach (Transform tran in hills) {
			GameObject hill = tran.gameObject;

			HillScript hillScript = hill.GetComponent<HillScript> ();

			if (hillScript != null) {

				int hillNumber = hillScript.hillNumber;
				ModifyHills (hillNumber , hill);
			}
		}
	}

	void ModifyHills (int hillNumber , GameObject hill){

		Transform hillTransform = hill.transform;

		if (hillNumber < 17) {

			hillTransform.position = new Vector2( hillTransform.position.x,  -4.5f);

		}

		if (hillNumber < 53) {
			HillScript hillScript = hill.GetComponent<HillScript> ();
			if (hillScript != null) {
				hillScript.lastHeight = 0;
				//ModifyHills (hillNumber , hill);
			}
		}
	}
}
