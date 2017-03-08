using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class WinnersPage2 : MonoBehaviour {

	public static WinnersPage2 instance = null;

	public GameObject containerPanel;

	public GameObject batchPrefab;

	public Button prevButton, nextButton;

	int perPage = 4;

	int page;

	int batchCount;

	public List<int> batchArray, prizeArray, batch;

	void Awake (){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);  
		}
	}

	public void Init(){

		nextButton.gameObject.SetActive (false);
		prevButton.gameObject.SetActive (false);

		page = 0;
		GameManager.DestroyChildren (containerPanel);
		GameSparksManager.instance.MonthsInstantWins ();

	}

	public void PoolBatches(){
		//Show Next / Previous
		DisplayMainPageButtons ();

		batchCount = batchArray.Count;

		int pageNumber = page * perPage ;

		//Checks whether less than 4 on page
		int a = batchCount > pageNumber + 3 ? pageNumber + perPage  : batchCount&perPage - 1;
		a--; // to account for 0 index

		for (int i = pageNumber ; i <= pageNumber + a; i++) {

			GameObject newBatch = Instantiate (batchPrefab, containerPanel.transform,false) as GameObject;
			newBatch.GetComponent<Batch> ().batchNumber = batch.ElementAt(i);
			//Create Title
			newBatch.GetComponent<Batch> ().batchTitle.text =
				batchArray.ElementAt(i).ToString () +
				" x £" + prizeArray.ElementAt(i).ToString() + " winners";

			//Init batches
			newBatch.GetComponent<Batch>().Init();
		}
	}

	void DisplayMainPageButtons(){
		if (page == (batchCount / perPage)) {
			nextButton.gameObject.SetActive (false);
		} else {
			nextButton.gameObject.SetActive (true);
		}

		if (page == 0) {
			prevButton.gameObject.SetActive (false);
		} else {
			prevButton.gameObject.SetActive (true);
		}
	}

	public void NextButton(){
		page++;

		if( page > (batchCount / perPage)){
			page = 0;
		}

		GameManager.DestroyChildren (containerPanel);
		PoolBatches ();
	}

	public void PreviousButton(){

		if( page > 0){
			page--;
		}

		GameManager.DestroyChildren (containerPanel);
		PoolBatches ();
	}

}
