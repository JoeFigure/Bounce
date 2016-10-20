using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DebugModifier : MonoBehaviour {

	public ModifierTypes type;

	public Text title;

	public float modifiedValue;

	string _modifierName;

	public string modifierName {
		set {
			_modifierName = value;
			title.text = _modifierName; 
		}
		get{ return _modifierName; }
	}


	public void sliderValueChange(float input){
		modifiedValue = input;
		title.text = modifierName + "  " + input.ToString ("n2");

		DebugMenu.ModifyValue (type, input);
	}
}

