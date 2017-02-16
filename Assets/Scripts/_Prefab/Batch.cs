using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using GameSparks.Core;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class Batch : MonoBehaviour {

	public int batchNumber;

	public Text batchTitle;

	List<Data> profileData = new List<Data>();

	public GameObject containerPanel, intantWinnerIcon;

	public Button nextButton, prevButton;

	public int page;
	int perPage = 5;

	//Init calls first round of profile pics - Based on Month + Batch number
	public void Init(){
		page = 0;
		GetWinners ();
		AssignButtons ();
	}

	void AssignButtons(){
		nextButton.onClick.AddListener (NextButton);
		prevButton.onClick.AddListener (PreviousButton);
	}

	//Gamesparks call
	void GetWinners (){

		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GETINSTANTGENERIC").
		SetEventAttribute ("MONTH", DateTime.Now.Month).
		SetEventAttribute ("YEAR", DateTime.Now.Year).
		SetEventAttribute ("BATCH", batchNumber).
		SetDurable(true).
		Send ((response) => {
			if (!response.HasErrors) {

				//Get images
				var dump = response.ScriptData.GetGSDataList("DUMP");
				List<GameSparks.Core.GSData> d = (List<GameSparks.Core.GSData>)dump;

				int x = 0;
				foreach(GSData i in d){
					profileData.Insert(x, new Data( i.GetString("playerID"),i.GetString("image")));
					x++;
				}

				page = 0;
				StartCoroutine (ChangePage());


			}
		});
	}

	IEnumerator ChangePage(){

		GameManager.DestroyChildren (containerPanel);

		int pageNumber = page * perPage ;

		for (int i = pageNumber ; i <= pageNumber + (perPage -1); i++) {

			if (i < profileData.Count) {
				GameObject newButton = Instantiate (intantWinnerIcon);
				newButton.transform.SetParent (containerPanel.GetComponent<Transform> (), false);

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

	void NextButton(){
		page++;
		if( page > (profileData.Count / perPage)){
			page = 0;
		}
		StartCoroutine (ChangePage ());
	}

	void PreviousButton(){
		page--;
		if( page < 0){
			page = (profileData.Count / perPage);
		}
		StartCoroutine (ChangePage ());
	}

}
