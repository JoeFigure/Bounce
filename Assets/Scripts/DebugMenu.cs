using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DebugMenu : MonoBehaviour {
	/*
	struct SliderPanel{
		public Slider slider;
		public Text sliderText;
		string sliderName;
	}

	SliderPanel[] sPanel;
	//Fill with every slider panel on game object
*/

	public SpikeScript spikes;
	public SpikeScript hills;
	public SpikeGeneratorScript spikeGenerator;
	public BallScript ball;

	public GameObject mainMenuUI;
	public GameObject debugMenuUI;

	public Text debugHud;


	string temp1;
	string temp2;
	string temp3;
	string temp4;
	string temp5;

	string hudText;

	public Slider slider1;
	public Text slider1Text;
	string slider1Name = "Obstacle Start Speed : ";

	public Slider slider2;
	public Text slider2Text;
	string slider2Name = "Spike Amount (smaller = more frequent) : ";

	public Slider slider3;
	public Text slider3Text;
	string slider3Name = "Bounce Force : ";

	public Slider slider4;
	public Text slider4Text;
	string slider4Name = "Bounce Energy : ";

	public Slider slider5;
	public Text slider5Text;
	string slider5Name = "Level Scale : ";

	// Use this for initialization
	void Start () {
		debugMenuUI.SetActive (false);
		slider1Text.text = slider1Name.ToString ();
		slider2Text.text = slider2Name.ToString ();
		slider3Text.text = slider3Name.ToString ();
		slider4Text.text = slider4Name.ToString ();
		slider5Text.text = slider5Name.ToString ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DebugModeBttn(){
		mainMenuUI.SetActive (false);
		debugMenuUI.SetActive (true);
	}

	public void HideDebugBttn(){
		debugMenuUI.SetActive (false);
	}

	public void Slider1(){
		float minusSliderValue = -slider1.value;
		slider1Text.text = slider1Name.ToString() + minusSliderValue.ToString ();
		spikes.speed = minusSliderValue;
		hills.speed = minusSliderValue;

		temp1 = slider1Text.text + "\n";

		SetHud ();
	}

	public void Slider2(){
		float sliderValue = slider2.value;
		slider2Text.text = slider2Name.ToString() + sliderValue.ToString ();
		//spikeGenerator.largestSpikeDistance = sliderValue;

		temp2 = slider2Text.text + "\n";
		SetHud ();
	}

	public void Slider3(){
		float sliderValue = slider3.value;
		slider3Text.text = slider3Name.ToString() + sliderValue.ToString ();
		ball.force = sliderValue;

		temp3 = slider3Text.text + "\n";
		SetHud ();
	}

	public void Slider4(){
		float sliderValue = slider4.value;
		slider4Text.text = slider4Name.ToString() + sliderValue.ToString ();
		ball.energyLoss = sliderValue;

		temp4 = slider4Text.text + "\n";
		SetHud ();
	}

	public void Slider5(){
		float sliderValue = slider5.value;
		slider5Text.text = slider5Name.ToString() + sliderValue.ToString ();
		spikeGenerator.scaleMult = sliderValue;
		ball.ScaleMult = sliderValue;

		temp5 = slider5Text.text + "\n";
		SetHud ();
	}

	void SetHud(){
		hudText = temp1  + temp2  + temp3  + temp4  + temp5;
		debugHud.text = hudText;
	}

}
