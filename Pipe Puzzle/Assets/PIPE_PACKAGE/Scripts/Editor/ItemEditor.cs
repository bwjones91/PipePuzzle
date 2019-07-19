using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{
	public class ItemEditor : EditorWindow {

		private static ItemEditor window;

		protected Vector2 scrollPos;

		public int selectID = 0;

		protected static ItemDB itemDB;

		public static void Init () {
			itemDB = ItemDB.LoadDB ();

			window = (ItemEditor)EditorWindow.GetWindow(typeof (ItemEditor), false, "Item Editor");
			window.minSize = new Vector2 (550, 500);
		}

		protected void Select(int ID){
			if (selectID == ID) {
				return;
			}
			selectID = ID;
		}
			

		public void OnGUI (){
			if (window == null) {
				Init ();
			}
			List<Item> itemList = itemDB.itemList;

			GUILayout.BeginVertical ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Create New", new GUILayoutOption[] { GUILayout.Width (100), GUILayout.Height (30) })) {
				Select(NewItem());
			}

			if (GUILayout.Button ("Remove", new GUILayoutOption[] { GUILayout.Width (100), GUILayout.Height (30) })) {
				this.RemoveItem ();
				Select(Mathf.Max(0, selectID-1));
			}

			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			if (itemList.Count <= 0) {
				return;
			}

			GUILayout.BeginHorizontal ();

			DrawItemList(itemList);	

			GUILayout.Space (50);

			DrawItemInfo(itemList[selectID]);

			GUILayout.EndHorizontal ();

			if (GUI.changed) {
				EditorUtility.SetDirty(itemDB);
				for (int i = 0; i < itemList.Count; i++) {
					EditorUtility.SetDirty (itemList [i]);
				}
			}

		}


		void DrawItemInfo(Item item){
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Name:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (40) });
			item.name = GUILayout.TextField (item.name, new GUILayoutOption[] { GUILayout.Width (100) });
			GUILayout.EndHorizontal ();

			GUILayout.Space (10);

			GUILayout.Label ("The icon will be displayed in the game", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (250) });

			GUILayout.BeginHorizontal ();
			GUILayout.Space (140);
			item.enableTopJoin = GUILayout.Toggle (item.enableTopJoin, "Enable Top Join", new GUILayoutOption[] { GUILayout.Width (150) });
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();
			GUILayout.Space (30);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Enable Left Join", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (85) });
			item.enableLeftJoin = GUILayout.Toggle (item.enableLeftJoin, "", new GUILayoutOption[] { GUILayout.Width (15) });
			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			item.gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)EditorGUILayout.ObjectField (item.gameObject.GetComponent<SpriteRenderer> ().sprite, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width (70), GUILayout.Height (70) });

			GUILayout.BeginVertical ();
			GUILayout.Space (30);
			item.enableRightJoin = GUILayout.Toggle (item.enableRightJoin, "Enable Right Join", new GUILayoutOption[] { GUILayout.Width (150) });
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Space (140);
			item.enableBottomJoin = GUILayout.Toggle (item.enableBottomJoin, "Enable Bottom Join", new GUILayoutOption[] { GUILayout.Width (150) });
			GUILayout.EndHorizontal ();

			GUILayout.Space (10);
			item.isFixed = GUILayout.Toggle (item.isFixed, "Enable Fixed, Unable to rotate", new GUILayoutOption[] { GUILayout.Width (250) });
			item.isKeyItem = GUILayout.Toggle (item.isKeyItem, "Enable Key Unit, The item must be joined to pass the level", new GUILayoutOption[] { GUILayout.Width (350) });


			//Set sprite readable,so that the level editor can rotate the sprite.
			if (item.gameObject.GetComponent<SpriteRenderer> ().sprite) {
				string path = AssetDatabase.GetAssetPath (item.gameObject.GetComponent<SpriteRenderer> ().sprite);
				TextureImporter import = AssetImporter.GetAtPath (path) as TextureImporter;
				if (!import.isReadable) {
					import.isReadable = true;
					AssetDatabase.ImportAsset(path);
				}
//				if (import.textureType != TextureImporterType.Default) {
//					import.textureType = TextureImporterType.Default;
//					import.isReadable = true;
//					AssetDatabase.ImportAsset(path);
//				}
			}

			GUILayout.EndVertical ();
		}

		protected void DrawItemList(List<Item> list, bool drawRemove=true){

			Rect visibleRectList = new Rect (10, 40, 200, window.position.height - 45);
			Rect contentRectList = new Rect (10, 40, 190, list.Count * 35 + 5);

			GUI.color = new Color (.8f, .8f, .8f, 1f);
			GUI.Box (visibleRectList, "");
			GUI.color = Color.white;

			scrollPos = GUI.BeginScrollView (visibleRectList, scrollPos, contentRectList);

			GUILayout.BeginVertical ();
			GUILayout.Space (10);

			for(int i=0; i<list.Count; i++){
				GUILayout.BeginHorizontal ();
				GUILayout.Space (15);

				if (selectID == i) {
					GUI.color = Color.gray;
				} else {
					GUI.color = Color.white;
				}

				Sprite sprite = list [i].gameObject.GetComponent<SpriteRenderer> ().sprite;
				if (sprite) {
					if (GUILayout.Button (sprite.texture, new GUILayoutOption[] { GUILayout.Width (30), GUILayout.Height (30) })) {
						Select (i);
					}
				} else {
					GUILayout.Box ("", new GUILayoutOption[] { GUILayout.Width (30), GUILayout.Height (30) });
				}

				if (GUILayout.Button (list [i].name, new GUILayoutOption[] { GUILayout.Width (120), GUILayout.Height (30) })) {
					Select (i);
				}
				GUI.color = Color.white;
				GUILayout.EndHorizontal ();

			}
			GUILayout.EndVertical ();

			GUI.EndScrollView();

			selectID = Mathf.Clamp (selectID, 0, list.Count - 1);
		}

		int NewItem(int cloneID=-1){
			 
			itemDB.primaryKey++;

			GameObject obj=Resources.Load("ItemBase", typeof(GameObject)) as GameObject;

			String fileName = "Assets/PIPE_PACKAGE/Resources/Item" + itemDB.primaryKey + ".prefab";
			GameObject prefab=PrefabUtility.CreatePrefab(fileName, obj, ReplacePrefabOptions.ConnectToPrefab);

			AssetDatabase.Refresh ();

			Item item = prefab.GetComponent<Item>();
			item.ID = itemDB.primaryKey;
			itemDB.itemList.Add(item);

			return itemDB.itemList.Count-1;
		}

		void RemoveItem(){

			String fileName = "Assets/PIPE_PACKAGE/Resources/Item" + itemDB.itemList[selectID].ID + ".prefab";
			FileUtil.DeleteFileOrDirectory (fileName);
			AssetDatabase.Refresh ();

			itemDB.itemList.RemoveAt(selectID);
		}

	}
}