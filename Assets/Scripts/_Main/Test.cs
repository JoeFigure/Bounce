using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void DoSummit();

public class Test : MonoBehaviour {

	public DoSummit go;

	public static event DoSummit whu;

	// Use this for initialization
	void Start () {
		go += PrintA;
		go += PrintB;

		whu += PrintA;
	}
	
	// Update is called once per frame
	void Update () {
		//go.Invoke ();
		//whu.Invoke ();

	}

	void Unused(){
		string q = P ("aString").Replace('a','b');
	}

	string P(string input){
		return input;
	}

	void PrintA(){
		print ("YO MAMA");
	}
	void PrintB(){
		print ("WHOA");
	}
}
