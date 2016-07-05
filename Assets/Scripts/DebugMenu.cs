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
	}

	public void Slider2(){
		float sliderValue = slider2.value;
		slider2Text.text = slider2Name.ToString() + sliderValue.ToString ();
		spikeGenerator.spikeFreq = sliderValue;
	}

	public void Slider3(){
		float sliderValue = slider3.value;
		slider3Text.text = slider3Name.ToString() + sliderValue.ToString ();
		ball.force = sliderValue;
	}

	public void Slider4(){
		float sliderValue = slider4.value;
		slider4Text.text = slider4Name.ToString() + sliderValue.ToString ();
		ball.bounceHeight = sliderValue;
	}

	public void Slider5(){
		float sliderValue = slider5.value;
		slider5Text.text = slider5Name.ToString() + sliderValue.ToString ();
		spikeGenerator.scaleMult = sliderValue;
		ball.scaleMult = sliderValue;
	}

}
