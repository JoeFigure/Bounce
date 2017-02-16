using UnityEngine;
using System.Collections;
using System;
using UnityEngine.iOS;
using Prime31;


public class NotificationManager : MonoBehaviour {

	void Start(){
		#if UNITY_IOS
		RegisterForNotif ();

		#endif
	}

	void RegisterForNotif(){
		#if UNITY_IOS
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert|  UnityEngine.iOS.NotificationType.Sound);
		#endif
	}

	void ScheduleNotification(){
		#if UNITY_IOS
		UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();
		notif.fireDate = DateTime.Now.AddSeconds (5500);
		notif.alertBody = "Come back and play!";
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
		#endif
	}

	void OnApplicationPause (bool isPause){

		if( isPause ){ // App going to background
			#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

			ScheduleNotification ();
			EtceteraBinding.setBadgeCount(0);
			#endif
		}

		else {

			#if UNITY_IOS
			/*
			Debug.Log("Local notification count = " + UnityEngine.iOS.NotificationServices.localNotificationCount);
			if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) {
				Debug.Log(UnityEngine.iOS.NotificationServices.localNotifications[0].alertBody);
			}
*/
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
			EtceteraBinding.setBadgeCount(0);
			#endif
		}
	}
		
}



