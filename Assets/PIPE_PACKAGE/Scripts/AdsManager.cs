using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if  GOOGLE_MOBILE_ADS
using GoogleMobileAds.Api;
#endif

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

using PIPE_PACKAGE;

namespace PIPE_PACKAGE {

	public class AdsManager : MonoBehaviour {

		public static AdsManager instance;
		[HideInInspector] public string admobUIDAndroid;
		[HideInInspector] public string admobUIDIOS;
#if  GOOGLE_MOBILE_ADS
		private InterstitialAd interstitial;
		private AdRequest requestAdmob;
#endif

		void Awake(){
			instance = this;
			AdsDB adsDB = AdsDB.LoadDB ();
			admobUIDAndroid = adsDB.admobUIDAndroid;
			admobUIDIOS = adsDB.admobUIDIOS;
#if GOOGLE_MOBILE_ADS
#if UNITY_IOS
			interstitial = new InterstitialAd(admobUIDIOS);
#else
			interstitial = new InterstitialAd (admobUIDAndroid);
#endif

			requestAdmob = new AdRequest.Builder().Build();
			interstitial.LoadAd(requestAdmob);
			interstitial.OnAdLoaded += HandleInterstitialLoaded;
			interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;

#endif
		}

#if GOOGLE_MOBILE_ADS
		public void HandleInterstitialLoaded(object sender, EventArgs args){
			print("HandleInterstitialLoaded.");
		}

		public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args){
			print("HandleInterstitialFailedToLoad, message: " + args.Message);
		}
#endif

		public void ShowAds (){
#if GOOGLE_MOBILE_ADS
			if (interstitial.IsLoaded())
			{
				interstitial.Show();
#if UNITY_IOS
				interstitial = new InterstitialAd(admobUIDIOS);
#else
				interstitial = new InterstitialAd (admobUIDAndroid);
#endif
				requestAdmob = new AdRequest.Builder().Build();
				interstitial.LoadAd(requestAdmob);
			}
#endif
		}
		

		public void ShowUnityAds ()  {
			#if UNITY_ADS

			if (Advertisement.IsReady ("video")) {
				Advertisement.Show ("video");
			} else {
				if (Advertisement.IsReady ("defaultZone")) {
					Advertisement.Show ("defaultZone");
				}
			}
			#endif
		}
	}

}