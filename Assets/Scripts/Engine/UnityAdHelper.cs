/// <summary>
/// Unity ads
/// Currently only for Android
/// </summary>
using UnityEngine;
using System.Collections;
using System;

#if UNITY_ANDROID
using UnityEngine.Advertisements;
#endif


public class UnityAdHelper : MonoBehaviour
{

	public string androidGameID = "48833";
	public bool isTestMode = true;
	public bool isDebugging = true;
	private static Action handleFinished;
	private static Action handleSkipped;
	private static Action handleFailed;
	private static Action onContinue;


#if UNITY_ANDROID

	void Awake()
	{
		if (!Advertisement.isSupported) {
			Debug.Log ("Unity ads are not supported");
		} else if (Advertisement.isInitialized) {
			Debug.Log ("Unity Ads already initialized");
		}else if(string.IsNullOrEmpty(androidGameID)) {
			Debug.Log("Game ID not set");
		}else{
			if(isDebugging)
				Advertisement.debugLevel |= Advertisement.DebugLevel.Debug;

				Advertisement.debugLevel |= Advertisement.DebugLevel.Warning;

		}

		Advertisement.Initialize(androidGameID, isTestMode);
		StartCoroutine(UnityAdsInit());
		}

		private IEnumerator UnityAdsInit()
		{
			do yield return new WaitForSeconds (0.1f); 
			while(!Advertisement.isInitialized);

			Debug.Log ("Unity Ads initialized");
			yield break;
		}

		public bool isShowing {get {return Advertisement.isShowing;}}
		public bool isSupported{get {return Advertisement.isSupported;}}
		public bool isInitialized{get {return Advertisement.isInitialized;}}

		public static bool isReady()
		{
		return isReady (null);
	}

		public static bool isReady(string _zoneID)
		{
			if (string.IsNullOrEmpty (_zoneID))
			_zoneID = null;

			return Advertisement.IsReady(_zoneID);
		}


	public void ShowAd()
	{
		ShowAd (null, null, null, null, null);
	}
	public void ShowAd(string _zoneID)
	{
		ShowAd (_zoneID, null, null, null, null);
	}
	public void ShowAd (string _zoneID, Action _handleFinished)
	{
		ShowAd (_zoneID, _handleFinished, null, null, null);
	}
	public void  ShowAd(string _zoneID, Action _handleFinished, Action _handleSkipped)
	{
		ShowAd (_zoneID, _handleFinished, _handleSkipped, null, null);
	}
	public void ShowAd(string _zoneID, Action _handleFinished, Action _handleSkipped, Action _handleFailed)
	{
		ShowAd (_zoneID, _handleFinished, _handleSkipped, _handleFailed, null);
	}

	public void ShowAd(string _zoneID,Action _handleFinished, Action _handleSkipped, Action _handleFailed, Action _onContinue)
	{
		if(string.IsNullOrEmpty(_zoneID)) _zoneID = null;

		handleFinished = _handleFinished;
		handleSkipped = _handleSkipped;
		handleFailed = _handleFailed;
		onContinue = _onContinue;

		if (Advertisement.IsReady (_zoneID)) {
			ShowOptions options = new ShowOptions();
			options.resultCallback = HandleShowResult;
		} else {
			Debug.Log("Unable to show Ad");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log("Ad Finished Succesfully");
			if(!object.ReferenceEquals(handleFinished,null)) 
				handleFinished();
			break;

		case ShowResult.Skipped:
			Debug.Log("Ad was skipped");
			if(!object.ReferenceEquals(handleSkipped,null)) 
				handleSkipped();
			break;

		case ShowResult.Failed:
			Debug.Log("Ad failed");
			if(!object.ReferenceEquals(handleFailed,null)) 
				handleFailed();
			break;
		}

		if (!object.ReferenceEquals (onContinue, null))
			onContinue ();
	}


#else
	void Awake ()
	{
		Debug.Log ("Unity Ads is not supported");
	}
	
	public bool isShowing { get { return false; } }

	public bool isSupported{ get { return false; } }

	public bool isInitialized{ get { return false; } }
	
	public static bool isReady ()
	{
		return false;
	}
	
	public static bool isReady (string _zoneID)
	{
		return false;
	}
	
	public void ShowAd ()
	{
		return;
	}

	public void ShowAd (string _zoneID)
	{
		ShowAd ();
	}

	public void ShowAd (string _zoneID, Action _handleFinished)
	{
		ShowAd ();
	}

	public void  ShowAd (string _zoneID, Action _handleFinished, Action _handleSkipped)
	{
		ShowAd ();
	}

	public void ShowAd (string _zoneID, Action _handleFinished, Action _handleSkipped, Action _handleFailed)
	{
		ShowAd ();
	}
	
	#endif
}
/*Advertisement.Show(null,new ShowOptions {
resultCallback = result => {
Debug.Log(result.ToString());
	*/
