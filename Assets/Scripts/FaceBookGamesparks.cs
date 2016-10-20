using UnityEngine;
using System.Collections;
using Facebook;
using GameSparks.Api.Requests;

public class FaceBookGamesparks : MonoBehaviour 
{
	public static FaceBookGamesparks instance = null;

	void Awake (){

		instance = this;
	}

	void Start(){
		FB.Init(null, OnHideUnity);
	}

	/*
	public void CallFBInit()
	{
		//When done initialising, call OnInitComplete
		FB.Init(OnInitComplete, OnHideUnity);
	}
*/
	/*
	void OnInitComplete()
	{
		//Print to the debug log that we are initialised and check if we are logged in
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		//Call FB.Login
		CallFBLogin();
	}
	*/
	void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}

	public void CallFBLogin()
	{
		//We first tell Facebook what permissions we want and then tell it todo GameSparksLogin when done
		FB.Login("email,user_friends", GameSparksLogin);
	}

	//FB.Login returns an FBResult, which is just information about our session
	void GameSparksLogin(FBResult fbResult)
	{
		//Check if we are logged in to Facebook
		if (FB.IsLoggedIn)
		{
			//If so, we can use that acces token to log in to Facebook
			new FacebookConnectRequest().SetAccessToken(FB.AccessToken).Send((response) =>
				{
					//If our response has errors we can check what went wrong
					if (response.HasErrors)
					{
						Debug.Log("Something failed when connecting with Facebook " + response.Errors);
					}
					else
					{
						//Otherwise we are successfully logged in!
						Debug.Log("Gamesparks Facebook Login Successful");

						GameSparksManager.instance.Login();
						//Updates user info in Gampeplay Manager

						//FB.
					}
				});
		}
	}

	public void Share(){

		FB.Feed (linkCaption: "I'm playing Mazoin!");
	}

}

