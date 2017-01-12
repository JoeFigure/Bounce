using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;

public class SignupUI : MonoBehaviour {

	public static SignupUI instance = null;

	public Text facebookEmail, userName;

	public GameObject tandCPanel, emailPanel, checkEmailPanel, photoPanel;
	public GameObject welcomeUIContainer;

	List<GameObject> panels = new List<GameObject>();

	void Awake (){
		instance = this;
	}

	void Start(){
		panels.Add (tandCPanel);
		panels.Add (emailPanel);
		panels.Add (checkEmailPanel);
		panels.Add (photoPanel);

	}

	void HidePanels(){
		/*
		foreach (GameObject i in panels) {
			i.SetActive (false);
		}
		*/
		UIManager.instance.DeactivateAllChildren (welcomeUIContainer);
	}

	public void ShowCheckEmailMenu(){
		HidePanels ();
		checkEmailPanel.SetActive (true);
		//string tempUserName = Facebook.MiniJSON.Json.Deserialize
		userName.text = GameManager.userName;
		facebookEmail.text = GameManager.email;
	}
	public void ShowEmailPanel(){
		HidePanels ();
		emailPanel.SetActive (true);
	}
	public void ShowPhotoPanel(){
		HidePanels ();
		photoPanel.SetActive (true);
	}

	public void EnterGame(){
		GameManager.instance.FirstPlay ();
	}

}
