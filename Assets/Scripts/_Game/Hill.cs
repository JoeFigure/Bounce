using UnityEngine;
using System.Collections;

public class Hill : MonoBehaviour {

	public int hillNumber;

	public bool instantiated;
	
	// Update is called once per frame
	void Update () {
		//transform.Translate (GameplayController.speed * Vector3.right);
		DestroyObject ();
	}

	void DestroyObject(){
		if(transform.position.x < (GameplayController.zeroScreenX - (200/*CHANGE*/))){
			Destroy (gameObject);
		}
	}
}
