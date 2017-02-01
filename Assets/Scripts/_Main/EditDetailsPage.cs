using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dobro.Text.RegularExpressions;

public class EditDetailsPage : MonoBehaviour {


	public static EditDetailsPage instance = null;

	public GameObject editDetailPage, backButton;

	public Image profileImage, mainProfileImage;

	string newEmail;

	public Text currentEmail;

	void Awake (){
		instance = this;
	}

	public void ShowEditDetailsPage(){
		editDetailPage.SetActive (true);
		profileImage.sprite = mainProfileImage.sprite;
		WinnerPageUI.instance.ShowBurgerButton (false);
		currentEmail.text = GameManager.email;
	}

	public void HideEditDetailsPage(){
		editDetailPage.SetActive (false);
		WinnerPageUI.instance.ShowBurgerButton (true);
	}

	public void CheckNewEmail(string newEmail){
		if (TestEmail.IsEmail (newEmail)) {
			this.newEmail = newEmail;
		} else {
			UIManager.instance.ShowTextPopup ("Warning", "Email not valid", true);
		}
	}

	public void  SubmitNewEmail(){
		if (!string.IsNullOrEmpty (newEmail)) {
			GameSparksManager.instance.SubmitNewEmail (newEmail);
			GameManager.email = newEmail;
			UIManager.instance.ShowPage ("profile");
			EditDetailsPage.instance.HideEditDetailsPage ();
		} else {
			UIManager.instance.ShowTextPopup ("Warning", "Email not valid", true);
		}
	}
}
