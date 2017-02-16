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
		page = 0;
		GameManager.DestroyChildren (containerPanel);
		GameSparksManager.instance.MonthsInstantWins ();

	}

	public void PoolBatches(){

		batchCount = batchArray.Count;
		Debug.Log (batchCount);
		int pageNumber = page * perPage ;

		//Checks whether less than 4 on page
		int a = batchCount > pageNumber + 3 ? pageNumber + perPage  : batchCount&perPage - 1;
		a--; // to account for 0 index

		for (int i = pageNumber ; i <= pageNumber + a; i++) {

			GameObject newBatch = Instantiate (batchPrefab, containerPanel.transform,false) as GameObject;
			newBatch.GetComponent<Batch> ().batchNumber = batch.ElementAt(i);
			//Create Title
			newBatch.GetComponent<Batch> ().batchTitle.text =
				//"BATCH " +newBatch.GetComponent<Batch> ().batchNumber + "      " +
				batchArray.ElementAt(i).ToString () +
				" £" + prizeArray.ElementAt(i).ToString() + " winners";

			//Init batches
			newBatch.GetComponent<Batch>().Init();
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
