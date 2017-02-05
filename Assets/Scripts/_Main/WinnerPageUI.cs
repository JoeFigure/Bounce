using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Collections.Specialized;
using System.Linq;

public struct Data
{
	public string id, image;

	public Data (string id, string image)
	{
		this.id = id;
		this.image = image;
	}
}

public class WinnerPageUI : MonoBehaviour
{

	public static WinnerPageUI instance = null;

	public GameObject instantPanel, burgerButton, backButton, winnerProfile;

	public GameObject intantWinnerIcon, grandWinnerButton;

	public Text winnerName, grandWinnerName, instantWinnerLocation, grandWinnerLocation;
	public Text topScoreProfile, totalGamesProfile, averageScoreProfile;

	public Image profileImage;

	//public Dictionary<string,string> wins = new Dictionary<string, string> ();
	//public OrderedDictionary a = new OrderedDictionary ();

	public List<Data> profileData = new List<Data> ();

	//public Button next;

	public int page;
	int perPage = 10;

	float instantIconHeight;

	//GRAND WINNER PROFILE DATA

	public int grandWinnerAverageScore, grandWinnerTotalGames, grandWinnerTopScore;
	public string _grandWinnerName, _grandWinnerLocation;

	void Awake (){
		instance = this;
	}

	void Start (){
		instantIconHeight = intantWinnerIcon.GetComponent<LayoutElement> ().preferredHeight;
	}

	public void Activate (){
		GameSparksManager.instance.GetInstantWinners ();
		GameSparksManager.instance.GetGrandWinner ();
	}

	/*
	void ResizePanel (){
		int rows = (int)Mathf.Ceil ((float)wins.Count / 5) + 1;
		instantPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, instantIconHeight * rows);
	}
	*/

	public void ShowWinnerProfile (string name, string location, Sprite profileImage, string totalGames, string topScore, string averageScore){
		ShowBurgerButton (false);
		winnerProfile.SetActive (true);
		winnerName.text = GameManager.GetFirstName (name);
		this.profileImage.sprite = profileImage;
		instantWinnerLocation.text = location;
		totalGamesProfile.text = totalGames;
		topScoreProfile.text = topScore;
		averageScoreProfile.text = averageScore;
	}

	public void ShowGrandWinner (string name, string location, string fbPic){
		grandWinnerName.text = GameManager.GetFirstName (name);
		if (!string.IsNullOrEmpty (location)) {
			grandWinnerLocation.text = location;
		} else {
			grandWinnerLocation.text = "Location unavailable";
		}
		StartCoroutine (FetchProfilePic (fbPic)); 
	}

	//For Grand Winner
	IEnumerator FetchProfilePic (string url){
		WWW www = new WWW (url);
		yield return www;
		Texture2D profilePic = www.texture;
		Rect rec = new Rect (0, 0, profilePic.width, profilePic.height);
		Sprite fbSprite = Sprite.Create (profilePic, rec, new Vector2 ());

		grandWinnerButton.GetComponent<Image> ().sprite = fbSprite;

		//Action
		grandWinnerButton.GetComponent<Button> ().onClick.AddListener (() => {
			ShowWinnerProfile (_grandWinnerName, _grandWinnerLocation, fbSprite, grandWinnerTotalGames.ToString (),
				grandWinnerTopScore.ToString (), grandWinnerAverageScore.ToString ());
		});
	}

	public void HideWinnerProfile (){
		winnerProfile.SetActive (false);
		ShowBurgerButton (true);
	}

	public void ShowBurgerButton (bool hide){
		burgerButton.SetActive (hide);
	}

	//Instant winners
	//public IEnumerator Icons1 (){
		/*
		foreach (KeyValuePair<string,string> i in wins) {
			GameObject newButton = Instantiate (intantWinnerIcon);
			newButton.transform.SetParent (instantPanel.GetComponent<Transform> (), false);

			//Add URL image
			if (i.Value != "Empty") {
				WWW www = new WWW (i.Value);
				yield return www;
				Texture2D profilePic = www.texture;
				Rect rec = new Rect (0, 0, profilePic.width, profilePic.height);
				Sprite fbSprite = Sprite.Create (profilePic, rec, new Vector2 ());
				newButton.GetComponent<Image> ().sprite = fbSprite;

				//Makes Button name url
				newButton.name = www.url;

				//Add button action
				newButton.GetComponent<Button> ().onClick.AddListener (() => {
					GameSparksManager.instance.GetInstantWinnerProfile (newButton.name, newButton.GetComponent<Image> ().sprite);
				});
			}
			




		}
	*/

	//Data singleData = new Data (i.Key, i.Value);
	//profileData.Add (singleData);
		//ResizePanel ();
		/*
		for (int i = 0; i <= page + 10; i++) {

			Debug.Log (profileData.ElementAt (i).image);
		}
*/
		//yield return null;
	//}

	public void NextButton(){
		page++;
		StartCoroutine (ChangePage ());
	}

	public IEnumerator ChangePage(){

		foreach(Transform child in instantPanel.transform) {
			Destroy(child.gameObject);
		}

		if( page > (profileData.Count / perPage)){
			page = 0;
		}

		int pageNumber = page * perPage ;

		for (int i = pageNumber ; i <= pageNumber + 9; i++) {

			if (i < profileData.Count) {
				GameObject newButton = Instantiate (intantWinnerIcon);
				newButton.transform.SetParent (instantPanel.GetComponent<Transform> (), false);

				if (profileData.ElementAt (i).image != "Empty") {
					WWW www = new WWW (profileData.ElementAt (i).image);
					yield return www;
					Texture2D profilePic = www.texture;
					Rect rec = new Rect (0, 0, profilePic.width, profilePic.height);
					Sprite fbSprite = Sprite.Create (profilePic, rec, new Vector2 ());
					newButton.GetComponent<Image> ().sprite = fbSprite;

					string id = profileData.ElementAt (i).id;

					//Add button action
					newButton.GetComponent<Button> ().onClick.AddListener (() => {
						GameSparksManager.instance.GetInstantWinnerProfile (id, fbSprite);
					});
				}
			}
		}
	}

}