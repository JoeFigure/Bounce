using UnityEngine;
using System.Collections;

public class LevelDesign : MonoBehaviour {

	void Awake(){
		Random.seed = 2;
	}

	//Set first hills to height of 0
	public static int HillHeight (int hillNumber, int originalHeight){
		if (hillNumber < 22) {
			return 0;
		} else {
			return originalHeight;
		}
	}

	//Makes first spikes singles
	public static bool FirstSpikes(){
		if (GameplayController.gameTime < 15) {
			return true;
		} else {
			return false;
		}
	}
}



