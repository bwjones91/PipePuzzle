using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{

	public class AdsEditor : EditorWindow{
		private static AdsEditor window;

		protected static AdsDB adsDB;

		public static void Init (){
			adsDB = AdsDB.LoadDB ();

			window = (AdsEditor)EditorWindow.GetWindow(typeof (AdsEditor), false, "Ads Editor");
			window.Show ();

		}

		void OnGUI (){
			if (window == null) {
				Init ();
			}

			bool oldenableAds = adsDB.enableUnityAds;

			GUILayout.Label ("Ads settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width (150) });
			GUILayout.BeginHorizontal ();

			//UNITY ADS

			adsDB.enableUnityAds = EditorGUILayout.Toggle ("Enable Unity ads", adsDB.enableUnityAds, new GUILayoutOption[] {
				GUILayout.Width (200)
			});
			GUILayout.Label ("Install: Windows->\n Services->Ads - ON", new GUILayoutOption[] { GUILayout.Width (130) });

			GUILayout.EndHorizontal ();

			GUILayout.Space (10);

			if (oldenableAds != adsDB.enableUnityAds)
				SetScriptingDefineSymbols ();

			//GOOGLE MOBILE ADS
				
			bool oldenableGoogleMobileAds = adsDB.enableGoogleMobileAds;
			GUILayout.BeginHorizontal ();
			adsDB.enableGoogleMobileAds = EditorGUILayout.Toggle ("Enable Google Mobile Ads", adsDB.enableGoogleMobileAds, new GUILayoutOption[] {
				GUILayout.Width (50),
				GUILayout.MaxWidth (200)
			});
			if (GUILayout.Button ("Install", new GUILayoutOption[] { GUILayout.Width (100) })) {
				Application.OpenURL ("https://github.com/googleads/googleads-mobile-unity/releases/download/v3.3.0/GoogleMobileAds.unitypackage");
			}
			if (GUILayout.Button ("Help", new GUILayoutOption[] { GUILayout.Width (80) })) {
				Application.OpenURL ("https://firebase.google.com/docs/admob/unity/start#basic_interstitial_request");
			}

			GUILayout.EndHorizontal ();

			GUILayout.Space (10);
			if (oldenableGoogleMobileAds != adsDB.enableGoogleMobileAds) {

				SetScriptingDefineSymbols ();
			}
			if (adsDB.enableGoogleMobileAds) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (20);
				adsDB.admobUIDAndroid = EditorGUILayout.TextField ("Admob Interstitial ID Android ", adsDB.admobUIDAndroid, new GUILayoutOption[] {
					GUILayout.Width (220),
					GUILayout.MaxWidth (220)
				});
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Space (20);
				adsDB.admobUIDIOS = EditorGUILayout.TextField ("Admob Interstitial ID iOS", adsDB.admobUIDIOS, new GUILayoutOption[] {
					GUILayout.Width (220),
					GUILayout.MaxWidth (220)
				});
				GUILayout.EndHorizontal ();
				GUILayout.Space (10);
			}

			if (GUI.changed) {
				EditorUtility.SetDirty (adsDB);
			}
		}


		void SetScriptingDefineSymbols (){
			string defines = "";
			if (adsDB.enableUnityAds) {
				defines = defines + "; UNITY_ADS";
			}
			if (adsDB.enableGoogleMobileAds) {
				defines = defines + "; GOOGLE_MOBILE_ADS";
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, defines);
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, defines);
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.WSA, defines);

		}
			
	}
}
