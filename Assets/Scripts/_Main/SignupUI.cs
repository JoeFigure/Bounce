using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;
using Dobro.Text.RegularExpressions;

public class SignupUI : MonoBehaviour
{

	public static SignupUI instance = null;

	public Text facebookEmail, userName;

	public string email;
	bool matchingEmail = false;

	public GameObject fbImage;

	public GameObject tandCPanel, emailPanel, checkEmailPanel, photoPanel;
	public GameObject welcomeUIContainer;

	List<GameObject> panels = new List<GameObject> ();

	void Awake (){
		instance = this;
	}

	void Start (){
		panels.Add (tandCPanel);
		panels.Add (emailPanel);
		panels.Add (checkEmailPanel);
		panels.Add (photoPanel);

	}

	void HidePanels (){
		UIManager.instance.DeactivateAllChildren (welcomeUIContainer);
	}

	public void ShowCheckEmailMenu (){
		HidePanels ();

		if (string.IsNullOrEmpty (facebookEmail.text)) {
			ShowEmailPanel ();
		}

		checkEmailPanel.SetActive (true);
		userName.text = GameManager.userName;
		facebookEmail.text = GameManager.email;
	}

	public void ShowEmailPanel (){
		HidePanels ();
		emailPanel.SetActive (true);
	}

	public void ShowPhotoPanel (){
		HidePanels ();
		photoPanel.SetActive (true);
		DisplayFBPicture ();
	}

	public void ConfirmEmail (){
		if (!string.IsNullOrEmpty(email) && matchingEmail) {
			ShowPhotoPanel ();
			GameManager.email = email;
		} else {
			UIManager.instance.ShowTextPopup ("Warning", "Enter email", true);
		}
	}

	void DisplayFBPicture (){
		Texture2D pic = GameManager.profilePic;
		Rect rec = new Rect (0, 0, pic.width, pic.height);
		Sprite fbSprite = Sprite.Create (pic, rec, new Vector2());

		fbImage.GetComponent<Image> ().sprite = fbSprite;
	}

	public void InitialSubmitToGameSparks(){
		UIManager.instance.ShowLoadingScreen ();
		GameSparksManager.instance.InitSubmit ();
	}

	public void CheckEmail (string input){
		if (TestEmail.IsEmail (input)) {
			email = input;
		} else {
			UIManager.instance.ShowTextPopup ("Warning", "Email not valid", true);
		}
	}

	public void CheckIfEmailMatches (string input){
		if (input == email) {
			matchingEmail = true;
		} else {
			UIManager.instance.ShowTextPopup ("Warning", "Emails do not match", true);
		}

	}

}
