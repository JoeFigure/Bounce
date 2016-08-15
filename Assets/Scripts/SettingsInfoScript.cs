using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SettingsInfoScript : MonoBehaviour {

	public Text versionText;
	public Text updatesText;

	string versionNumber {
		get{ return Application.version; }
	}

	// Use this for initialization
	void Start () {

		versionText.text = "Version: " + versionNumber;
		updatesText.text = Updates ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string Updates ()
	{
		string updates =
			"Updates: " ;
			//"\n- Settings menu added" +
			//"\n- App name change";

		return updates;
	}
}
