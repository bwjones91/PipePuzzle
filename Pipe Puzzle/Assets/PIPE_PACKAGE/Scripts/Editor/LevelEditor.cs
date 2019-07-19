using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{

	public class LevelEditor : EditorWindow{
		int levelNumber = 0;
		private int selectedID = -1;
		protected static ItemDB itemDB;
		protected static LevelDB levelDB;
		private static LevelEditor window;
		private Vector2 scrollPos;

		public static void Init (){
			itemDB = ItemDB.LoadDB ();
			levelDB = LevelDB.LoadDB ();
			window = (LevelEditor)EditorWindow.GetWindow (typeof(LevelEditor), false, "Level Editor");
			window.Show ();
		}
			
		void OnFocus (){

		}
			
		void OnGUI (){
			if (window == null) {
				Init ();
			}

			if (0 == GetLevelCount ()) {
				AddLevel ();
			}
			LevelData levelData = levelDB.levelList [levelNumber];

			scrollPos = GUI.BeginScrollView (new Rect (25, 15, position.width - 30, position.height), scrollPos, new Rect (0, 0, 400, 800 + levelData.maxRows*55));

			GUILevelSelector ();
			GUILayout.Space (10);

			GUILevelBackGround ();
			GUILayout.Space (10);

			GUILevelSize ();
			GUILayout.Space (10);

			GUILimit ();
			GUILayout.Space (10);

			GUIStars ();
			GUILayout.Space (10);

			GUIItems ();
			GUILayout.Space (10);

			GUIGameMap ();

			GUI.EndScrollView ();

			if (GUI.changed) {
				EditorUtility.SetDirty(levelDB);
			}
		}
			
		void GUILevelSelector (){
			GUILayout.BeginVertical ();
			GUILayout.Label ("Level:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width (50) });

			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);
			if (GUILayout.Button ("<", new GUILayoutOption[] { GUILayout.Width (60) })) {
				PreviousLevel ();
			}
			string changeLevel = GUILayout.TextField (" " + (levelNumber + 1), new GUILayoutOption[] { GUILayout.Width (50) });
			try {
				if (int.Parse (changeLevel) != levelNumber + 1) {
					levelNumber = int.Parse (changeLevel) - 1;
					if (levelNumber >= GetLevelCount()) {
						levelNumber = GetLevelCount() - 1;
					}else if (levelNumber < 0) {
						levelNumber = 0;
					}
				}
			} catch (Exception) {
				throw;
			}

			if (GUILayout.Button (">", new GUILayoutOption[] { GUILayout.Width (60) })) {
				NextLevel ();
			}

			if (GUILayout.Button ("New level", new GUILayoutOption[] { GUILayout.Width (65) })) {
				AddLevel ();
			}
			if (GUILayout.Button ("Insert level", new GUILayoutOption[] { GUILayout.Width (80) })) {
				InsertLevel ();
			}
			if (GUILayout.Button ("Delete level", new GUILayoutOption[] { GUILayout.Width (80) })) {
				DeleteLevel ();
			}

			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();

		}

		void GUILevelBackGround (){
			LevelData levelData = levelDB.levelList [levelNumber];

			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);

			GUILayout.Label ("Ground1:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (55) });
			levelData.ground1 = (Sprite)EditorGUILayout.ObjectField (levelData.ground1, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width (70) });
			GUILayout.Space (10);

			GUILayout.Label ("Ground2:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (55) });
			levelData.ground2 = (Sprite)EditorGUILayout.ObjectField (levelData.ground2, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width (70) });
			GUILayout.Space (10);

			GUILayout.Label ("BG:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (20) });
			levelData.background = (Sprite)EditorGUILayout.ObjectField (levelData.background, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width (70) });

			GUILayout.EndHorizontal ();

		}

		void GUILevelSize (){
			LevelData levelData = levelDB.levelList [levelNumber];

			int oldValue = levelData.maxRows + levelData.maxCols;
			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Rows:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (60) });
			levelData.maxRows = EditorGUILayout.IntField (levelData.maxRows, new GUILayoutOption[] { GUILayout.Width (50) });
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Columns:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (60) });
			levelData.maxCols = EditorGUILayout.IntField (levelData.maxCols, new GUILayoutOption[] { GUILayout.Width (50) });
			GUILayout.EndHorizontal ();

			if (levelData.maxRows < 2) {
				levelData.maxRows = 2;
			}
			if (levelData.maxCols < 2) {
				levelData.maxCols = 2;
			}

			if (oldValue != levelData.maxRows + levelData.maxCols) {
				levelData.InitItemList ();
			}
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}

		void GUILimit (){
			LevelData levelData = levelDB.levelList [levelNumber];

			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);

			GUILayout.Label ("Time:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (60) });
			levelData.limit = EditorGUILayout.IntField (levelData.limit, new GUILayoutOption[] { GUILayout.Width (50) });
			if (levelData.limit <= 0)
				levelData.limit = 1;
			GUILayout.EndHorizontal ();

		}


		void GUIStars (){
			LevelData levelData = levelDB.levelList [levelNumber];

			GUILayout.BeginVertical ();

			GUILayout.Label ("Stars:", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);
			GUILayout.Label ("Star1", new GUILayoutOption[] { GUILayout.Width (80) });
			GUILayout.Label ("Star2", new GUILayoutOption[] { GUILayout.Width (80) });
			GUILayout.Label ("Star3", new GUILayoutOption[] { GUILayout.Width (80) });
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Space (30);
			int s = 0;
			s = EditorGUILayout.IntField ("", levelData.star1, new GUILayoutOption[] { GUILayout.Width (80) });
			if (s != levelData.star1) {
				levelData.star1 = s;
			}
			if (levelData.star1 < 0)
				levelData.star1 = 1;
			s = EditorGUILayout.IntField ("", levelData.star2, new GUILayoutOption[] { GUILayout.Width (80) });
			if (s != levelData.star2) {
				levelData.star2 = s;
			}
			if (levelData.star2 < levelData.star1)
				levelData.star2 = levelData.star1 + 1;
			s = EditorGUILayout.IntField ("", levelData.star3, new GUILayoutOption[] { GUILayout.Width (80) });
			if (s != levelData.star3) {
				levelData.star3 = s;
			}
			if (levelData.star3 < levelData.star2)
				levelData.star3 = levelData.star2 + 1;
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();

		}

		void GUIItems (){
			LevelData levelData = levelDB.levelList [levelNumber];

			GUILayout.BeginVertical ();

			GUILayout.Label ("Items:", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal ();
			GUI.color = Color.white;
			if (GUILayout.Button ("Clear All", new GUILayoutOption[] { GUILayout.Width (104), GUILayout.Height (50) })) {
				levelData.InitItemList ();
			}
			if (selectedID == -1) {
				GUI.color = Color.gray;
			} else {
				GUI.color = Color.white;
			}
			if (GUILayout.Button ("Clear", new GUILayoutOption[] { GUILayout.Width (50), GUILayout.Height (50) })) {
				selectedID = -1;
			}
			GUILayout.EndHorizontal ();

			for (int row = 0; row < itemDB.itemList.Count/levelData.maxCols + 1; row++) {
				GUILayout.BeginHorizontal ();
				for (int i = row*levelData.maxCols; i < (row+1)*levelData.maxCols; i++) {
					if (i >= itemDB.itemList.Count) {
						break;
					}
					if (selectedID == itemDB.itemList [i].ID) {
						GUI.color = Color.gray;
					} else {
						GUI.color = Color.white;
					}

					if (GUILayout.Button (itemDB.itemList [i].gameObject.GetComponent<SpriteRenderer> ().sprite.texture, new GUILayoutOption[] {
						GUILayout.Width (50),
						GUILayout.Height (50)
					})) {
						selectedID = itemDB.itemList [i].ID;
					}
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}

		void GUIGameMap (){
			LevelData levelData = levelDB.levelList [levelNumber];
			List<ItemData> itemList = levelData.itemList;

			GUI.color = Color.gray;

			GUILayout.BeginVertical ();

			for (int row = 0; row < levelData.maxRows; row++) {
				GUILayout.BeginHorizontal ();
				for (int col = 0; col < levelData.maxCols; col++) {
					var imageButton = new object ();

					Item itemPrefab = GetItemPrefabByID (itemList [row * levelData.maxCols + col].ID);
					if (itemPrefab) {
						Texture2D itemTexture = itemPrefab.gameObject.GetComponent<SpriteRenderer> ().sprite.texture;

						int width = itemTexture.width;
						int height = itemTexture.height;

						if (itemList [row * levelData.maxCols + col].rotation == Rotation.Rotation0) {
							imageButton = itemTexture;
						} else {
							int t = 0;
							Texture2D newTexture;

							if (itemList [row * levelData.maxCols + col].rotation == Rotation.Rotation90) {
								newTexture = new Texture2D(height, width);
								while (t < width) {
									newTexture.SetPixels (0, t, newTexture.width, 1, itemTexture.GetPixels (width - t - 1, 0, 1, height));
									t++;
								}
							} else if (itemList [row * levelData.maxCols + col].rotation == Rotation.Rotation180) {
								newTexture = new Texture2D(width, height);
								Texture2D tmpTexture = new Texture2D(width, height);
								while (t < height)
								{
									tmpTexture.SetPixels(0, t, tmpTexture.width, 1, itemTexture.GetPixels (0,height - t - 1, width, 1));
									t++;            
								}
								t = 0;
								while (t < width)
								{
									newTexture.SetPixels(t, 0, 1, newTexture.height, tmpTexture.GetPixels(width - t - 1, 0, 1, height));
									t++;
								}
							} else {
								newTexture = new Texture2D(height, width);
								while (t < height) {
									newTexture.SetPixels (t, 0, 1, newTexture.height, itemTexture.GetPixels (0, height - t - 1, width, 1));
									t++;
								}
							}

							newTexture.Apply ();
							imageButton = newTexture;
						}
					}
					if (GUILayout.Button (imageButton as Texture, new GUILayoutOption[] {
						GUILayout.Width (50),
						GUILayout.Height (50),
					})) {
						SetType (row, col);
					}
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}

		Item GetItemPrefabByID (int ID){
			for (int i = 0; i < itemDB.itemList.Count; i++) {
				if (ID == itemDB.itemList [i].ID) {
					return itemDB.itemList [i];
				}
			}
			return null;
		}

		void SetType (int row, int col){
			LevelData levelData = levelDB.levelList [levelNumber];
			List<ItemData> itemList = levelData.itemList;
			if (itemList [row * levelData.maxCols + col].ID == selectedID) {
				if (itemList [row * levelData.maxCols + col].rotation == Rotation.Rotation270) {
					itemList [row * levelData.maxCols + col].rotation = Rotation.Rotation0;
				} else {
					itemList [row * levelData.maxCols + col].rotation++;
				}
			} else {
				itemList [row * levelData.maxCols + col].ID = selectedID;
			}

			EditorUtility.SetDirty(levelDB);
		}

		int GetLevelCount (){
			return levelDB.levelList.Count;
		}

		void AddLevel (){
			LevelData newLevel = new LevelData(); 
			newLevel.InitItemList ();
			levelDB.levelList.Add(newLevel);
			EditorUtility.SetDirty(levelDB);

			levelNumber = GetLevelCount () - 1;
		}

		void InsertLevel (){
			LevelData newLevel = new LevelData(); 
			newLevel.InitItemList ();
			levelDB.levelList.Insert(levelNumber, newLevel);
			EditorUtility.SetDirty(levelDB);

			//levelNumber++;
		}

		void DeleteLevel (){
			levelDB.levelList.RemoveAt(levelNumber);
			levelNumber--;
			if (levelNumber < 0) {
				levelNumber = 0;
			}
			if (0 == GetLevelCount ()) {
				AddLevel ();
			}
		}

		void NextLevel (){
			levelNumber++;
			if (levelNumber >= GetLevelCount()) {
				levelNumber = GetLevelCount() - 1;
			}
			EditorUtility.SetDirty(levelDB);
		}

		void PreviousLevel (){
			levelNumber--;
			if (levelNumber < 0) {
				levelNumber = 0;
			}
			EditorUtility.SetDirty(levelDB);
		}
	}
}