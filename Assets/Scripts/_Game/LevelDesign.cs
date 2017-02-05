using UnityEngine;
using System.Collections;

public class LevelDesign : MonoBehaviour {


	public static int randomSeed = 2;

	public static int HillHeight (int hillNumber, int originalHeight){
		if (hillNumber < 22) {
			return 0;
		} else {
			return originalHeight;
		}
	}

}



