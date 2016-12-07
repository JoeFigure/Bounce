using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


public class UnityAds : MonoBehaviour
{

	public Image adStatus;

	void Update(){
		if (Advertisement.IsReady("rewardedVideo")) {
			adStatus.color = Color.green;
		}
	}

	public static void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private static void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			// YOUR CODE TO REWARD THE GAMER
			UIManager.instance.ShowTextPopup ("Reward", "You now have +1 Zoins!", true);
			GameSparksManager.AddZoin (20);
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

	public static void ShowAd()
		{
			if (Advertisement.IsReady())
			{
				Advertisement.Show();
			}
		}

}