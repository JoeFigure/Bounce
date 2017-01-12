using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;

public class TandC : MonoBehaviour {

	public Text tAndCs;
	XmlDocument tCDoc = new XmlDocument();
	string[] tcPages = new string[5];
	int currentPage;

	void Start () {
		LoadXML ();
		FillPagesArray();
		//tAndCs.text = tcPages [0];
	}

	void OnEnable(){
		tAndCs.text = tcPages [0];
	}

	void LoadXML(){
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			tCDoc.Load (Application.dataPath + "/Raw/TermsAndConditions.xml");
		}else if (Application.platform == RuntimePlatform.Android) {
			//tCDoc.Load (Application.dataPath + "jar:file://" + Application.dataPath + "!/assets//TermsAndConditions.xml");
		} else {
			tCDoc.Load (Application.dataPath + "/StreamingAssets/TermsAndConditions.xml");
		}
	}

	void FillPagesArray(){
		tcPages[0] = tCDoc.SelectSingleNode("text/body/page1").FirstChild.InnerText;
		tcPages[1] = tCDoc.SelectSingleNode("text/body/page2").FirstChild.InnerText;
		tcPages[2] = tCDoc.SelectSingleNode("text/body/page3").FirstChild.InnerText;
		tcPages[3] = tCDoc.SelectSingleNode("text/body/page4").FirstChild.InnerText;
		tcPages[4] = tCDoc.SelectSingleNode("text/body/page5").FirstChild.InnerText;
	}

	public void NextPage(){
		currentPage++;

		if (currentPage == tcPages.Length)
			currentPage = 0;

		tAndCs.text = tcPages [currentPage];
	}

}
