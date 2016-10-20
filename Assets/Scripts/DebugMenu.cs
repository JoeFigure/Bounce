using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;

//Add a value for every modifier you want to create.
public enum ModifierTypes{
	Ball_Scale,
	Example_B,
	C,
	Modifier_Wordszzz
}

public class DebugMenu : MonoBehaviour {

	public SpikeScript spikes;
	public SpikeScript hills;
	public SpikeGeneratorScript spikeGenerator;
	public BallScript ball;

	public GameObject modifiersPanel;
	public GameObject modifierPrefab;

	static List<DebugModifier> modifiers = new List<DebugModifier>();

	//Generates modifier controls based on amount of ModifierTypes enum values
	void Start() {

		var valuesAsArray = ModifierTypes.GetNames(typeof(ModifierTypes));

		for(int i = 0; i<= valuesAsArray.Length - 1; i++) {
			
			GameObject modifier =  Instantiate (modifierPrefab) as GameObject;
			modifier.transform.SetParent (modifiersPanel.transform,false);
			modifiers.Add (modifier.GetComponent<DebugModifier>());
			ModifierConfigure (modifiers [i], (ModifierTypes)i);
		}
	}

	void ModifierConfigure(DebugModifier modifier, ModifierTypes m){
		modifier.modifierName = m.ToString();
		modifier.type = m; 
	}

	//Function of each modifier
	public static void ModifyValue(ModifierTypes n, float value){
		switch(n){
		case ModifierTypes.Ball_Scale:
			GameplayController.ball.gameObject.transform.localScale = new Vector2 (value, value);
			break;

		default:
			break;
		}
	}

}
