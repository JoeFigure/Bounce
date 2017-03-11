using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


public class UnityAds : MonoBehaviour
{

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

			int reward = 3;

			Debug.Log ("The ad was successfully shown.");
			// YOUR CODE TO REWARD THE GAMER
			UIManager.instance.ShowTextPopup ("Reward", "+" + reward.ToString() + " Zoins!", true);
			GameSparksManager.AddZoin (reward);
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
