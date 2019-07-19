using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class GameManager : MonoBehaviour {
	
		public static GameManager instance;
	
		public int currentLevel = 1;
		public int limit = 30;
		public int star1;
		public int star2;
		public int star3;
		public bool isPaused;

		public int totalLevel;
		public GameObject background;
		public GameObject levelManager;

		//ui
		public Text textLevel;
		public GameObject menuComplete;
		public GameObject menuFailed;
		public GameObject menuPause;
		public GameObject buttonNextLevel;
		public GameObject iconStar3;
		public GameObject iconStar2;
		public GameObject iconStar1;
		public Text menuCompleteLevel;
		public Text menuCompleteScore;
		public Text menuFailedLevel;
		public Text menuFailedScore;

		#region game

		void Awake(){
			if (instance == null) {
				instance = this;
			}
		}	

		void Start(){
			TileManager.instance.TileClicked += OnTileClicked;
			TileManager.instance.TileHighlighted += OnTileHighlighted;
			TileManager.instance.TileDehighlighted += OnTileDehighlighted;
        }	

		public void InitLevel (int level) {
			MusicManager.instance.Play (MusicManager.instance.musicList[1]);

			isPaused = false;
			currentLevel = level;
			textLevel.text = "" + level;

			LoadLevelData ();

			TileManager.instance.Init (currentLevel);

			ProgressBar.Instance.SetLimit (limit, star1, star2, star3);
			ProgressBar.Instance.Timeout += OnTimeout;
		}
			
		private void OnTimeout(object sender, EventArgs e){
			MenuGameFailed ();
		}

		private void OnTileDehighlighted(object sender, EventArgs e){
			if (isPaused) {
				return;
			}
		}

		private void OnTileHighlighted(object sender, EventArgs e){
			if (isPaused) {
				return;
			}
		} 

		private void OnTileClicked(object sender, EventArgs e){
			if (isPaused) {
				return;
			}
			Tile tile = sender as Tile;
			if (tile.item) {
				tile.item.Click();
			}
			if (CheckJoin ()) {
				MenuGameComplete ();
			}
		}

		private bool CheckJoin (){
			TileManager.instance.ResetAllItemCheckedInfo ();

			//Start with a key item
			List<Tile> joinList = new List<Tile> ();
			joinList.Add (TileManager.instance.keyTilesList [0]);

			int keyCount = 1;
			while (joinList.Count > 0) {
				Tile lastTile = joinList [joinList.Count - 1];

				if (!lastTile.item.isTopChecked) {
					lastTile.item.isTopChecked = true;
					Tile nextTile = lastTile.GetNeighborTop ();
					if (nextTile && nextTile.item && nextTile.item.isBottomJoin () && !nextTile.item.isBottomChecked) {
						nextTile.item.isBottomChecked = true;
						if (!joinList.Contains (nextTile)) {
							if (nextTile.item.isKeyItem) {
								keyCount++;
							}
							joinList.Add (nextTile);
							//start check nextTile
							continue;
						}
					}
				}

				if (!lastTile.item.isRightChecked) {
					lastTile.item.isRightChecked = true;
					Tile nextTile = lastTile.GetNeighborRight ();
					if (nextTile && nextTile.item && nextTile.item.isLeftJoin () && !nextTile.item.isLeftChecked) {
						nextTile.item.isLeftChecked = true;
						if (!joinList.Contains (nextTile)) {
							if (nextTile.item.isKeyItem) {
								keyCount++;
							}
							joinList.Add (nextTile);
							//start check nextTile
							continue;
						}
					}
				}

				if (!lastTile.item.isBottomChecked) {
					lastTile.item.isBottomChecked = true;
					Tile nextTile = lastTile.GetNeighborBottom ();
					if (nextTile && nextTile.item && nextTile.item.isTopJoin () && !nextTile.item.isTopChecked) {
						nextTile.item.isTopChecked = true;
						if (!joinList.Contains (nextTile)) {
							if (nextTile.item.isKeyItem) {
								keyCount++;
							}
							joinList.Add (nextTile);
							//start check nextTile
							continue;
						}
					}
				}

				if (!lastTile.item.isLeftChecked) {
					lastTile.item.isLeftChecked = true;
					Tile nextTile = lastTile.GetNeighborLeft ();
					if (nextTile && nextTile.item && nextTile.item.isRightJoin () && !nextTile.item.isRightChecked) {
						nextTile.item.isRightChecked = true;
						if (!joinList.Contains (nextTile)) {
							if (nextTile.item.isKeyItem) {
								keyCount++;
							}
							joinList.Add (nextTile);
							//start check nextTile
							continue;
						}
					}
				}
				//All the neighbors have checked
				joinList.Remove (lastTile);
			}
		
			//Check if all the key items are connected
			if (keyCount >= TileManager.instance.keyTilesList.Count) {
				return true;
			} else {
				return false;
			}
		}
			
		private void LoadLevelData (){
			LevelDB levelDB = LevelDB.LoadDB ();
			LevelData levelData = levelDB.levelList [currentLevel - 1];

			limit = levelData.limit;
			star1 = levelData.star1;
			star2 = levelData.star2;
			star3 = levelData.star3;
			totalLevel = levelDB.levelList.Count;
			if (levelData.background) {
				background.GetComponent<SpriteRenderer> ().sprite = levelData.background;
			}
		}

		public void ClearLevelObject (){
			menuComplete.SetActive (false);
			menuFailed.SetActive (false);
			menuPause.SetActive (false);

			TileManager.instance.ClearTileObject ();
		}

		#endregion


		#region Menu

		public void MenuGameComplete (){
			SoundManager.instance.Play (SoundManager.instance.levelComplete);

			PauseGame ();

			int starsCount = ProgressBar.Instance.starsCount;
			if (PlayerPrefs.GetInt (string.Format ("Level.{0:G}.StarsCount", currentLevel), 0) < starsCount) {
				PlayerPrefs.SetInt (string.Format ("Level.{0:G}.StarsCount", currentLevel), starsCount);
			}

			if (PlayerPrefs.GetInt ("UnlockLevel", 0) < currentLevel + 1) {
				PlayerPrefs.SetInt ("UnlockLevel", currentLevel + 1);
			}

			menuComplete.SetActive (true);

			buttonNextLevel.SetActive (currentLevel < totalLevel);
			menuCompleteLevel.text = "Lv" + currentLevel;
			menuCompleteScore.text = string.Format ("{0:0}", ProgressBar.Instance.currentTime * 100);
			iconStar3.SetActive (starsCount >= 3);
			iconStar2.SetActive (starsCount >= 2);
			iconStar1.SetActive (starsCount >= 1);

		}

		public void MenuGameFailed (){
			SoundManager.instance.Play (SoundManager.instance.gameover);

			if (menuFailed.activeInHierarchy == true) {
				return;
			}
			menuFailed.SetActive (true);
			menuFailedLevel.text = "Lv" + currentLevel;
			menuFailedScore.text = string.Format ("{0:0}", ProgressBar.Instance.currentTime * 100);
			PauseGame ();
		}

		public void MenuGamePaused (){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);
			menuPause.SetActive (true);
			PauseGame ();
		}

		public void MenuLevel (){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			ClearLevelObject ();
			this.gameObject.SetActive (false);
			levelManager.SetActive (true);
		}


		public void NextLevel (){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			ClearLevelObject ();
			InitLevel (currentLevel + 1);
		}

		public void RestartGame (){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			ClearLevelObject ();
			InitLevel (currentLevel);
		}

		public void ResumeGame (){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			isPaused = false;
			ProgressBar.Instance.Resume();

			menuPause.SetActive (false);
		}

		public void PauseGame (){
			isPaused = true;
			ProgressBar.Instance.Pause();
		}

		#endregion
			
	}

}