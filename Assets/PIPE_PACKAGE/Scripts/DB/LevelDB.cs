using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{

	[System.Serializable]
	public class LevelData {
		public Sprite ground1;
		public Sprite ground2;
		public Sprite background;
		public int limit = 30;
		public int maxRows = 9;
		public int maxCols = 9;
		public int star1;
		public int star2;
		public int star3;
		public List<ItemData> itemList = new List<ItemData>();

		public void InitItemList(){
			itemList.Clear ();
			for (int i = 0; i < maxCols * maxRows; i++) {
				ItemData itemData = new ItemData ();
				itemData.ID = -1;
				itemData.rotation = Rotation.Rotation0;
				itemList.Add (itemData);
			}
		}
	}

	public class LevelDB : MonoBehaviour {

		public List<LevelData> levelList = new List<LevelData>();

		public static LevelDB LoadDB(){
			GameObject obj = Resources.Load ("LevelDB", typeof(GameObject)) as GameObject;

			#if UNITY_EDITOR
			if(obj == null) obj = CreatePrefab();
			#endif
			return obj.GetComponent<LevelDB> ();
		}

		public static List<LevelData> Load(){
			GameObject obj = Resources.Load ("LevelDB", typeof(GameObject)) as GameObject;
			print (obj);
			#if UNITY_EDITOR
			if(obj == null) obj = CreatePrefab();
			#endif

			LevelDB instance = obj.GetComponent<LevelDB> ();
			return instance.levelList;
		}


		#if UNITY_EDITOR
		private static GameObject CreatePrefab(){
			GameObject obj = new GameObject ();
			obj.AddComponent<LevelDB> ();
			GameObject prefab = PrefabUtility.CreatePrefab ("Assets/PIPE_PACKAGE/Resources/LevelDB.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
			DestroyImmediate (obj);
			AssetDatabase.Refresh ();
			return prefab;
		}
		#endif

	}
}