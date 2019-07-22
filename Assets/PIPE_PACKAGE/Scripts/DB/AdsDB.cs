using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{
	
	public class AdsDB : MonoBehaviour {

		public bool enableUnityAds;
		public bool enableGoogleMobileAds;
		public string admobUIDAndroid;
		public string admobUIDIOS;

		public static AdsDB LoadDB(){
			GameObject obj = Resources.Load ("AdsDB", typeof(GameObject)) as GameObject;

			#if UNITY_EDITOR
			if(obj == null) obj = CreatePrefab();
			#endif
			return obj.GetComponent<AdsDB> ();
		}

		#if UNITY_EDITOR
		private static GameObject CreatePrefab(){
			GameObject obj = new GameObject ();
			obj.AddComponent<AdsDB> ();
			GameObject prefab = PrefabUtility.CreatePrefab ("Assets/PIPE_PACKAGE/Resources/AdsDB.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
			DestroyImmediate (obj);
			AssetDatabase.Refresh ();
			return prefab;
		}
		#endif

	}
}