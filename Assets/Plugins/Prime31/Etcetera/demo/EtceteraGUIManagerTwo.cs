using UnityEngine;
using System.Collections;
using Prime31;



namespace Prime31
{
	public class EtceteraGUIManagerTwo : MonoBehaviourGUI
	{
#if UNITY_IOS
		public GameObject testPlane;
		private string imagePath;


		void Start()
		{
			// Listen to image picker event so we can load the image into a texture later
			EtceteraManager.imagePickerChoseImageEvent += imagePickerChoseImage;
		}


		void OnDisable()
		{
			// Stop listening to the image picker event
			EtceteraManager.imagePickerChoseImageEvent -= imagePickerChoseImage;
		}

		/*
		void OnGUI()
		{
			beginColumn();


			if( GUILayout.Button( "Show Activity View" ) )
			{
				EtceteraBinding.showActivityView();

				// hide the activity view after a short delay
				StartCoroutine( hideActivityView() );
			}


			if( GUILayout.Button( "Show Bezel Activity View" ) )
			{
				EtceteraBinding.showBezelActivityViewWithLabel( "Loading Stuff..." );

				// hide the activity view after a short delay
				StartCoroutine( hideActivityView() );
			}


			if( GUILayout.Button( "Prompt to Review This App" ) )
			{
				EtceteraBinding.askForReview( "Do you like this game?", "Please review the game if you do!", "572395608" );
			}


			if( GUILayout.Button( "Open App Store to Review Page" ) )
			{
				EtceteraBinding.openAppStoreReviewPage( "572395608" );
			}


			if( GUILayout.Button( "Register for Push" ) )
			{
				EtceteraBinding.registerForRemoteNotifications( P31RemoteNotificationType.Alert | P31RemoteNotificationType.Badge | P31RemoteNotificationType.Sound );
			}


			if( GUILayout.Button( "Get Registered Push Types" ) )
			{
				P31RemoteNotificationType types = EtceteraBinding.getEnabledRemoteNotificationTypes();

				if( ( types & P31RemoteNotificationType.Alert ) != P31RemoteNotificationType.None )
					Debug.Log( "registered for alerts" );

				if( ( types & P31RemoteNotificationType.Sound ) != P31RemoteNotificationType.None )
					Debug.Log( "registered for sounds" );

				if( ( types & P31RemoteNotificationType.Badge ) != P31RemoteNotificationType.None )
					Debug.Log( "registered for badges" );
			}


			if( GUILayout.Button( "Register with GameThrive" ) )
			{
				// registerDeviceWithGameThrive must be called in a couroutine since it makes a WWW request
				// Make sure you use your own appId and that you pass the actual deviceToken that the remoteRegistrationSucceededEvent
				// provides!
				StartCoroutine( EtceteraBinding.registerDeviceWithGameThrive( "13aa4c98-1931-11e4-aefc-5bf05f62f703", "token", null, www =>
				{
					// check for an error or success. If www.error is not null then an error occured
					if( www.error == null )
					{
						// success! Lets dump the data to the console so we can see what was returned
						Debug.Log( "GameThrive registration success: " + www.text );
					}
					else
					{
						Debug.Log( "Error registering with GameThrive: " + www.error );
					}
				}) );
			}


			// Second row
			endColumn( true );


			if( GUILayout.Button( "Set Urban Airship Credentials" ) )
			{
				// enter your own Urban Airship credentials here!
				EtceteraBinding.setUrbanAirshipCredentials( "S8Tf2CiUQSuh2A4NVdD2CA", "J6O97Dm2QK2-GGXZsPMlEA", "optional alias" );
			}


			if( GUILayout.Button( "Set Push.IO Credentials" ) )
			{
				// enter your own Push.IO credentials here!
				EtceteraBinding.setPushIOCredentials( "5VRVDMujew_a9UQ" );

				// optinally, pass in categories
				//EtceteraBinding.setPushIOCredentials( "5VRVDMujew_a9UQ", new string[] { "BaseballPlayers", "Gamers" } );
			}


			if( GUILayout.Button( "Prompt for Photo" ) )
			{
				EtceteraBinding.promptForPhoto( 0.25f, PhotoPromptType.CameraAndAlbum );
			}


			if( GUILayout.Button( "Prompt for Multiple Photos" ) )
			{
				EtceteraBinding.promptForMultiplePhotos( 3, 0.25f );
			}


			if( GUILayout.Button( "Load Photo Texture" ) )
			{
				if( imagePath == null )
				{
					var buttons = new string[] { "OK" };
					EtceteraBinding.showAlertWithTitleMessageAndButtons( "Load Photo Texture Error", "You have to choose a photo before loading", buttons );
					return;
				}

				// No need to resize because we asked for an image scaled from the picker but this is how we sould do it if we wanted to
				// Resize the image so that we dont end up trying to load a gigantic image
				//EtceteraBinding.resizeImageAtPath( imagePath, 256, 256 );

				// Add 'file://' to the imagePath so that it is accessible via the WWW class
				StartCoroutine( EtceteraManager.textureFromFileAtPath( "file://" + imagePath, textureLoaded, textureLoadFailed ) );
			}


			if( GUILayout.Button( "Save Photo to Album" ) )
			{
				if( imagePath == null )
				{
					var buttons = new string[] { "OK" };
					EtceteraBinding.showAlertWithTitleMessageAndButtons( "Load Photo Texture Error", "You have to choose a photo before loading", buttons );
					return;
				}

				EtceteraBinding.saveImageToPhotoAlbum( imagePath );
			}


			if( GUILayout.Button( "Get Image Size" ) )
			{
				if( imagePath == null )
				{
					var buttons = new string[] { "OK" };
					EtceteraBinding.showAlertWithTitleMessageAndButtons( "Error Getting Image Size", "You have to choose a photo before checking it's size", buttons );
					return;
				}

				var size = EtceteraBinding.getImageSize( imagePath );
				Debug.Log( "image size: " + size );
			}


			endColumn();


			// Next scene button
			if( bottomRightButton( "Next" ) )
			{
				loadLevel( "EtceteraTestSceneThree" );
			}
		}
		*/

		void imagePickerChoseImage( string imagePath )
		{
			this.imagePath = imagePath;
		}


		public IEnumerator hideActivityView()
		{
			yield return new WaitForSeconds( 2.0f );
			EtceteraBinding.hideActivityView();
		}


		// Texture loading delegates
		public void textureLoaded( Texture2D texture )
		{
			testPlane.GetComponent<Renderer>().material.mainTexture = texture;
		}


		public void textureLoadFailed( string error )
		{
			var buttons = new string[] { "OK" };
			EtceteraBinding.showAlertWithTitleMessageAndButtons( "Error Loading Texture.  Did you choose a photo first?", error, buttons );
			Debug.Log( "textureLoadFailed: " + error );
		}

#endif
	}

}
