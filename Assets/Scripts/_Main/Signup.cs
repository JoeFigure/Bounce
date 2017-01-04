using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;

public class Signup : MonoBehaviour
{

	public static UIData uiData;

	public int birthDay, birthMonth, birthYear;

	public string sex;
	string password;
	public string email = string.Empty;

	void Update(){
	}

	bool PasswordMatches (){
		if (uiData.retypePasswordSignup.text == uiData.passwordSignup.text) {
			return true;
		} else {
			return false;
		}
	}

	public void SetPassword(string input){
		password = input;
	}

	public void SetEmail(string input){
		email = input;
	}

	bool ValidPasswordLength(){
		bool validLength = false;
		if (password.Length >= 3) {
			validLength = true;
		}
		return validLength;
	}

	void CheckLegitimateYear(){
		if (birthYear < 1900 || birthYear > DateTime.Today.Year) {
			UIManager.instance.ShowTextPopup ("Warning", "Incorrect year entered", true);
		}
	}

	bool OverMinAge (){

		DateTime inputDate = new DateTime (birthYear, birthMonth, birthDay);

		int minAge = 16;
		TimeSpan minAgeSpan = new TimeSpan (365 * minAge, 0, 0, 0);
		DateTime minBirthDate = DateTime.Today.Subtract (minAgeSpan);

		return DateTime.Compare (minBirthDate, inputDate) > 0 ? true : false;
	}

	public void ChangeBirthDay (string birthDay){
		this.birthDay = int.Parse (birthDay);
	}

	public void ChangeBirthMonth (string birthMonth){
		this.birthMonth = int.Parse (birthMonth);
	}

	public void ChangeBirthYear (string birthYear){
		this.birthYear = int.Parse (birthYear);
		CheckLegitimateYear ();
	}

	public void SexToggle(bool male){
		if (male) {
			uiData.femaleToggle.isOn = false;
			sex = "MALE";
		} else {
			uiData.maleToggle.isOn = false;
			sex = "FEMALE";
		}
	}

	bool TandCChecked (){
		return uiData.tAndCToggle.isOn;
	}

	public void SubmitRegistration (){

		if (String.IsNullOrEmpty (email)) {
			UIManager.instance.ShowTextPopup ("Warning", "Email not entered", true);
			return;
		}

		if (!ValidPasswordLength()) {
			UIManager.instance.ShowTextPopup ("Warning", "Passwords needs at least 4 characters", true);
			return;
		}

		if (!PasswordMatches ()) {
			UIManager.instance.ShowTextPopup ("Warning", "Passwords dont match", true);
			return;
		}

		if (!TandCChecked ()) {
			UIManager.instance.ShowTextPopup ("Warning", "Agree to Terms & Conditions", true);
			return;
		}

		if (birthDay == 0 || birthMonth == 0 || birthYear == 0) {
			UIManager.instance.ShowTextPopup ("Warning", "Enter BirthDate", true);
			return;
		} else if (!OverMinAge ()) {
				UIManager.instance.ShowTextPopup ("Warning", "Under Minimum Age Restrictions", true);
				return;
			}

		//All checks out!!
		GameSparksManager.instance.RegistrationRequest ();
		GameSparksManager.instance.SignupDetails (sex, email, birthDay, birthMonth, birthYear);
	}
}
