using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;


public class IntroTutorial : MonoBehaviour {

	int currentSlide;
	int amountOfSlides = 4;

	GameObject[] pageIndicators;

	public GameObject pageIndicatorParent, pageIndicator;
	public Color nonCurrentColor, currentColor;
	public Text sectionText, headingText, subheadingText, subText;
	public Image iconImage;

	Sprite mazoinIcon, prizeIcon, smallPrizeIcon, zoinIcon;

	void Awake(){
		pageIndicators = new GameObject[amountOfSlides];
		GeneratePageIndicators ();

		mazoinIcon = Resources.Load<Sprite>("mazoinSimpleIcon");
		prizeIcon = Resources.Load<Sprite>("starIcon");
		smallPrizeIcon = Resources.Load<Sprite>("smallPrizeIcon");
		zoinIcon = Resources.Load<Sprite>("zoinIcon");
	}

	void OnEnable(){
		currentSlide = 0;
		ShowSlide ();
	}

	void Update(){
		TouchCustom.Swipe ();
		if (TouchCustom.swiped == TouchCustom.Swiped.Right && currentSlide > 0) {//&& currentSlide < (slides.Count - 1)) {
			currentSlide--;
			ShowSlide ();
		} else if (TouchCustom.swiped == TouchCustom.Swiped.Left && currentSlide < (amountOfSlides - 1)) {
			currentSlide++;
			ShowSlide ();
		}
	}

	void GeneratePageIndicators(){
		for(int i = 0;i < amountOfSlides;i++) {
			GameObject pImage = Instantiate (pageIndicator) as GameObject;
			pImage.transform.SetParent(pageIndicatorParent.transform,false);
			pageIndicators [i] = pImage;
		}
	}

	void PageIndicatorColor(){
		for(int i = 0;i < amountOfSlides;i++) {
			pageIndicators[i].GetComponent<Image>().color = nonCurrentColor;
		}
		pageIndicators [currentSlide].GetComponent<Image>().color = currentColor;
	}

	void ShowSlide(){

		PageIndicatorColor ();

		switch (currentSlide) {
		case 0:
			sectionText.text = "MAZOIN";
			headingText.text = "NEW";
			iconImage.sprite = mazoinIcon;
			subheadingText.text = "GAME AND PRIZES";
			subText.text = "ARE INTRODUCED\r AT THE START OF\r EVERY MONTH\r TO PLAY & WIN";
			break;

		case 1:
			sectionText.text = "TOP PRIZE";
			headingText.text = "£10,000";
			iconImage.sprite = prizeIcon;
			subheadingText.text = "CASH PRIZE";
			subText.text = "IS GIVEN TO THE\r PLAYER WITH THE\r HIGHEST SCORE AT THE\r END OF THE MONTH";
			break;

		case 2:
			sectionText.text = "EXTRA PRIZES";
			headingText.text = "£50";
			iconImage.sprite = smallPrizeIcon;
			subheadingText.text = "CASH PRIZE";
			subText.text = "IS INSTANTLY GIVEN\r TO THE FIRST\r 100 PLAYERS THAT\r SCORE 80 OR ABOVE";
			break;

		case 3:
			sectionText.text = "ZOINS";
			headingText.text = "PLAY";
			iconImage.sprite = zoinIcon;
			subheadingText.text = "WITH ZOINS";
			subText.text = "EACH TIME YOU\r PLAY IT COSTS 1 ZOIN\r GET MORE ZOINS IN THE\r BANK OF ZOINS";
			break;
		}
	}
}
