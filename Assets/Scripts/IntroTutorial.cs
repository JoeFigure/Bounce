using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;


public class IntroTutorial : MonoBehaviour {

	public List<GameObject>slides = new List<GameObject>();

	List<Image>pageIndicators = new List<Image>();

	int currentSlide;

	public GameObject pageIndicatorParent;
	public GameObject pageIndicator;

	public Color nonCurrentColor;
	public Color currentColor;

	void Start(){
		GeneratePageIndicators ();
	}

	void OnEnable(){

		currentSlide = 0;
		ShowSlide (currentSlide);

	}

	void Update(){
		
		TouchCustom.Swipe ();

		if (TouchCustom.swiped == TouchCustom.Swiped.Right && currentSlide > 0) {//&& currentSlide < (slides.Count - 1)) {
			currentSlide--;
			ShowSlide (currentSlide);
		} else if (TouchCustom.swiped == TouchCustom.Swiped.Left && currentSlide < (slides.Count - 1)) {
			currentSlide++;
			ShowSlide (currentSlide);

		}

		PageIndicatorColor ();
	}

	void ShowSlide(int currentSlide){
		ClearSlides ();
		slides [currentSlide].SetActive (true);
	}

	void ClearSlides(){
		foreach (GameObject slide in slides) {
			slide.SetActive (false);
		}
	}

	void GeneratePageIndicators(){
		foreach (GameObject slide in slides) {
			GameObject pImage = Instantiate (pageIndicator) as GameObject;
			pImage.transform.SetParent(pageIndicatorParent.transform,false);
			pageIndicators.Add(pImage.GetComponent<Image>());
		}
	}

	void PageIndicatorColor(){

		foreach (Image i in pageIndicators) {
			i.color = nonCurrentColor;
		}
		pageIndicators [currentSlide].color = currentColor;
	}
}
