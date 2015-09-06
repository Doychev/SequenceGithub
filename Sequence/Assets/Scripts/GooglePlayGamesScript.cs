using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using GooglePlayGames;
using GoogleMobileAds.Api;

public class GooglePlayGamesScript : MonoBehaviour {

	private BannerView bannerView;
	private string bannerId = "ca-app-pub-2205037698598396/4984392877";

	// Use this for initialization
	void Start () {
		PlayGamesPlatform.Activate ();

		Social.localUser.Authenticate((bool success) => {

		});

		bannerView = new BannerView(bannerId, AdSize.SmartBanner, AdPosition.Top);
		bannerView.LoadAd(new AdRequest.Builder().AddKeyword("unity").Build());
		bannerView.Show();		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
