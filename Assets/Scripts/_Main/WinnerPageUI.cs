using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WinnerPageUI : MonoBehaviour {

	public static WinnerPageUI instance = null;

	public GameObject instantPanel, burgerButton, backButton, winnerProfile;

	public GameObject intantWinnerIcon;

	public Text winnerName, grandWinnerName, instantWinnerLocation, grandWinnerLocation;
	public Image profileImage;

	public List<string> winners;

	float instantIconHeight;

	void Awake (){
		instance = this;
	}

	void Start () {
		instantIconHeight = intantWinnerIcon.GetComponent<LayoutElement> ().preferredHeight;
	}

	public void Activate(){
		GameSparksManager.instance.GetInstantWinners ();
		GameSparksManager.instance.GetGrandWinner ();
	}


	void ResizePanel(){
		int rows = (int)Mathf.Ceil((float)winners.Count/5) + 1;
		instantPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0,instantIconHeight * rows);
	}

	public IEnumerator Icons (){

		foreach (string i in winners) {
			GameObject newButton = Instantiate (intantWinnerIcon);
			newButton.transform.SetParent (instantPanel.GetComponent<Transform> (), false);

			//Add URL image
			if (i != "Empty") {
				WWW www = new WWW (i);
				yield return www;
				Texture2D profilePic = www.texture;
				Rect rec = new Rect (0, 0, profilePic.width, profilePic.height);
				Sprite fbSprite = Sprite.Create (profilePic, rec, new Vector2());
				newButton.GetComponent<Image> ().sprite = fbSprite;

				//Makes Button name url
				newButton.name = www.url;

				//Add button action
				newButton.GetComponent<Button>().onClick.AddListener(()=>{
					GameSparksManager.instance.GetInstantWinnerProfile(newButton.name, newButton.GetComponent<Image>().sprite);
				});
			}

		}
		ResizePanel ();
	}

	public void ShowWinnerProfile(string name, string location , Sprite profileImage){
		ShowBurgerButton (false);
		winnerProfile.SetActive (true);
		winnerName.text = GameManager.GetFirstName(name);
		this.profileImage.sprite = profileImage;
		instantWinnerLocation.text = location;
	}

	public void ShowGrandWinner(string name, string location){
		grandWinnerName.text = GameManager.GetFirstName(name);
		if (!string.IsNullOrEmpty (location)) {
			grandWinnerLocation.text = location;
		} else {
			grandWinnerLocation.text = "Location unavailable";
		}
	}

	public void HideWinnerProfile(){
		winnerProfile.SetActive (false);
		ShowBurgerButton (true);
	}

	public void ShowBurgerButton(bool hide){
		burgerButton.SetActive (hide);
	}

}

//Locations
