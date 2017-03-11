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
	public GameObject instantLastPanel;
	public GameObject intantWinnerIcon, grandWinnerButton;
	public GameObject lastBatch;

	public GameObject prevWinnersButton;

	public Text winnerName, grandWinnerName, instantWinnerLocation, grandWinnerLocation;
	public Text topScoreProfile, totalGamesProfile, averageScoreProfile;
	public Text grandPrizeText;
	public Text instantPrizeText, instantPrize2Text;

	public Image profileImage;

	public List<Data> profileData = new List<Data> ();
	public List<Data> profileData2 = new List<Data> ();

	public GameObject nextLastButton, prevLastButton;

	public int currBatchTotal, currBatchWinners;

	public Button currentBatchNextButton,currentBatchPrevButton;

	public int page;
	int perPage = 5;

	public int pagePrev;

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

		lastBatch.SetActive (false);
		prevWinnersButton.SetActive (false);
		nextLastButton.SetActive (false);
		prevLastButton.SetActive (false);

		GameSparksManager.instance.GetInstantWinners ();
		GrandWinnerHeader ();
		page = 0;

		//GameSparksManager.instance.GetInstantWinnersLast ();
		pagePrev = 0;
	}

	void GrandWinnerHeader(){
		if (GameManager.daysUntilPrize == 0 && GameManager.hrsUntilPrize == 0
		   && GameManager.minsUntilPrize == 0 && GameManager.minsUntilPrize == 0) {
			grandWinnerLocation.gameObject.SetActive (true);
			GameSparksManager.instance.GetGrandWinner ();
		} else {
			grandWinnerLocation.gameObject.SetActive (false);
			grandWinnerName.text = "TBC " + GameManager.instance.prizeDay.ToLongDateString();
		}
	}

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


	public void NextButton(){
		page++;
		if( page > (profileData.Count / perPage)){
			page = 0;
		}
		StartCoroutine (ChangePage ());
	}

	public void PreviousButton(){
		page--;
		if( page < 0){
			page = (profileData.Count / perPage);
		}
		StartCoroutine (ChangePage ());
	}

	void DisplayMainPageButtons(){
		if (page == (profileData.Count / perPage)) {
			currentBatchNextButton.gameObject.SetActive (false);
		} else {
			currentBatchNextButton.gameObject.SetActive (true);
		}

		if (page == 0) {
			currentBatchPrevButton.gameObject.SetActive (false);
		} else {
			currentBatchPrevButton.gameObject.SetActive (true);
		}
	}

	public IEnumerator ChangePage(){

		foreach(Transform child in instantPanel.transform) {
			Destroy(child.gameObject);
		}

		DisplayMainPageButtons ();

		int pageNumber = page * perPage ;

		for (int i = pageNumber ; i <= pageNumber + (perPage -1); i++) {

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

	public void SetGrandPrizeText(string prize){
		grandPrizeText.text = prize;
	}


	//////////////REPEAT FOR LAST INSTANT WINNERS
	/// 
	/// 
	/// 


	public void NextButton2(){
		pagePrev++;
		if( pagePrev > (profileData2.Count / perPage)){
			pagePrev = 0;
		}
		StartCoroutine (ChangePage2 ());
	}

	public void PreviousButton2(){
		pagePrev--;
		if( pagePrev < 0){
			pagePrev = (profileData2.Count / perPage);
		}
		StartCoroutine (ChangePage2 ());
	}


	public IEnumerator ChangePage2(){

		foreach(Transform child in instantLastPanel.transform) {
			Destroy(child.gameObject);
		}

		int pageNumber = pagePrev * perPage ;

		for (int i = pageNumber ; i <= pageNumber + (perPage -1); i++) {

			if (i < profileData2.Count) {
				GameObject newButton = Instantiate (intantWinnerIcon);
				newButton.transform.SetParent (instantLastPanel.GetComponent<Transform> (), false);

				if (profileData2.ElementAt (i).image != "Empty") {
					WWW www = new WWW (profileData2.ElementAt (i).image);
					yield return www;
					Texture2D profilePic = www.texture;
					Rect rec = new Rect (0, 0, profilePic.width, profilePic.height);
					Sprite fbSprite = Sprite.Create (profilePic, rec, new Vector2 ());
					newButton.GetComponent<Image> ().sprite = fbSprite;

					string id = profileData2.ElementAt (i).id;

					//Add button action
					newButton.GetComponent<Button> ().onClick.AddListener (() => {
						GameSparksManager.instance.GetInstantWinnerProfile (id, fbSprite);
					});
				}
			}
		}
	}

	public void SetInstantPrizeText(int prize){

		int prizesRemaning = currBatchTotal - currBatchWinners;

		instantPrizeText.text = "£" + prize.ToString () + " WINNERS  " +
			prizesRemaning.ToString() + "/" + currBatchTotal.ToString() + " REMAINING";
		

	}

	public void SetInstantPrizeText2(int prize){
		instantPrize2Text.text = "£" + prize.ToString () + " INSTANT WINNERS";
	}

}