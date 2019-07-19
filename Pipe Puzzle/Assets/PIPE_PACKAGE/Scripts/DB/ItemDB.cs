using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{
	
	public class ItemDB : MonoBehaviour {

		public List<Item> itemList=new List<Item>();
		public int primaryKey = 0;

		public static ItemDB LoadDB(){
			GameObject obj = Resources.Load ("ItemDB", typeof(GameObject)) as GameObject;

			#if UNITY_EDITOR
			if(obj == null) obj = CreatePrefab();
			#endif
			return obj.GetComponent<ItemDB> ();
		}

		public static List<Item> Load(){
			GameObject obj = Resources.Load ("ItemDB", typeof(GameObject)) as GameObject;
			print (obj);
			#if UNITY_EDITOR
			if(obj == null) obj = CreatePrefab();
			#endif

			ItemDB instance = obj.GetComponent<ItemDB> ();
			return instance.itemList;
		}


		#if UNITY_EDITOR
		private static GameObject CreatePrefab(){
			GameObject obj = new GameObject ();
			obj.AddComponent<ItemDB> ();
			GameObject prefab = PrefabUtility.CreatePrefab ("Assets/PIPE_PACKAGE/Resources/ItemDB.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
			DestroyImmediate (obj);
			AssetDatabase.Refresh ();
			return prefab;
		}
		#endif

	}
}