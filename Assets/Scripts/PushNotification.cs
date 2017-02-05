using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using GameSparks.Core;

using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;


//Listen to Etcetera

public class PushNotification : MonoBehaviour {
	#if UNITY_IOS
	// Use this for initialization

	//string deviceToken;

	void Start () {
		ScriptMessage.Listener += GetMessages;
	}
	
	// Update is called once per frame
	void Update () {
		//ManagePushNotifications ();
	}

	void OnEnable(){
		EtceteraManager.remoteRegistrationSucceededEvent += remoteRegistrationSucceeded;
		EtceteraManager.remoteRegistrationFailedEvent += remoteRegistrationFailed;
		//ManagePushNotifications ();
	}

		void remoteRegistrationSucceeded( string deviceToken )
		{
			Debug.Log( "remoteRegistrationSucceeded with deviceToken: " + deviceToken );
		GameManager.deviceToken = deviceToken;
		}


		void remoteRegistrationFailed( string error )
		{
			Debug.Log( "remoteRegistrationFailed : " + error );
		}

//	private void ManagePushNotifications ()
//	{           
//		//if (!registrationErrorDone && NotificationServices.registrationError != null) {
//		//	registrationErrorDone = true;
//		//	Debug.Log ("NotificationServices.registrationError: " + NotificationServices.registrationError);
//		//}
//		if (!pushRegistrationRequestDone && NotificationServices.deviceToken != null) {
//			pushRegistrationRequestDone = true;
//		string deviceToken = System.BitConverter.ToString (deviceToken).Replace ("-", "").ToLower ();
//		GameSparksSender sender = GameSparks.Api.Requests.PushRegistrationRequest (deviceToken);
//            
//	}





	#endif

	public void GetMessages(ScriptMessage message)
	{
		if (message.ExtCode == "T")
		{
			Debug.Log ("Received");
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications ();
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
			//UnityEngine.iOS.NotificationServices.CancelLocalNotification ();

		}

		if (message.ExtCode == "SecondScriptMessage")
		{
			//Do some other stuff
		}
	}

}
