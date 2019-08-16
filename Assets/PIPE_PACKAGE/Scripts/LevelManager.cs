using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class LevelManager : MonoBehaviour {
		
		public GameObject levelPrefab;
		public Transform levelField;
		public GameObject mainMenu;
		public GameObject gameManager;
		public GameObject buttonPre;
		public GameObject buttonNext;
		public GameObject background;
		public int levelPrePage = 20;
		public int levelPreRow = 5;
		public float levelWidth = 2.56f;
		public float levelHeight = 2.56f;
		private int currentPage = 0;

		public static LevelManager instance;

		void Awake(){
			instance = this;

		}	

		void Start () {
			MusicManager.instance.Play (MusicManager.instance.musicList[0]);

			int unlockLevel = PlayerPrefs.GetInt ("UnlockLevel", 0);
			currentPage = unlockLevel / levelPrePage;
			CreatePage ();

		}

		void Update () {

		}

		int GetLevelCount (){
			LevelDB levelDB = LevelDB.LoadDB ();
			return levelDB.levelList.Count;
		}

		public void MainMenu (int page){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			this.gameObject.SetActive (false);
			mainMenu.SetActive (true);
		}

		public void PreviousPage (int page){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			currentPage -= 1;
			if (currentPage < 0) {
				currentPage = 0;
			}
			CreatePage ();
		}

		public void NextPage (int page){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			currentPage += 1;
			int count = GetLevelCount ();
			if ((currentPage+1)*levelPrePage > count) {
				currentPage = count/levelPrePage;
			}
			CreatePage ();
		}

		void CreatePage (){
			levelField.transform.position = Vector3.zero;

			int count = GetLevelCount ();
			if (levelPrePage * currentPage > count) {
				return;
			}

			buttonPre.SetActive (currentPage > 0);
			buttonNext.SetActive (currentPage < (count - 1) / levelPrePage);


			for (int i = levelField.childCount - 1; i >= 0; i--) {  
				DestroyImmediate (levelField.GetChild (i).gameObject);  
			}  

			int maxLevel = (currentPage + 1) * levelPrePage + 1;
			if (maxLevel > count + 1) {
				maxLevel = count + 1;
			}

			for (int i = currentPage*levelPrePage + 1; i < maxLevel; i++) {
				CreateLevelButton (i);
			}

			levelField.transform.position = new Vector3 (-(levelPreRow-1)*levelWidth/2, (levelPrePage/levelPreRow-1)*levelHeight/2, 10);
		}

		void CreateLevelButton (int number){
			int unlockLevel = PlayerPrefs.GetInt ("UnlockLevel", 0);
			if (unlockLevel == 0) {
				unlockLevel = 1;
			}

			float posX = (((number-1)%levelPrePage) % levelPreRow) * levelWidth;
			float posY = (0 - ((number-1)%levelPrePage)/levelPreRow) * levelHeight;
			GameObject levelObject = Instantiate (levelPrefab, new Vector2 (posX, posY), Quaternion.identity) as GameObject;
			Level level = levelObject.GetComponent<Level> ();
			if (number > unlockLevel) {
				level.SetLock (true);
				level.SetStars (0);
			} else {
				level.SetLock (false);
				int starsCount = PlayerPrefs.GetInt (string.Format ("Level.{0:G}.StarsCount", number), 0);
				level.SetStars (starsCount);
			}
			level.SetNumber (number);

			level.LevelClicked += OnLevelClicked;
			level.LevelHighlighted += OnLevelHighlighted;
			level.LevelDehighlighted += OnLevelDehighlighted;

			levelObject.transform.SetParent (levelField);

		}

		private void OnLevelDehighlighted(object sender, EventArgs e){

		}

		private void OnLevelHighlighted(object sender, EventArgs e){

		} 

		private void OnLevelClicked(object sender, EventArgs e){
			Level level = sender as Level;
			if (!level.isLocked) {
				SoundManager.instance.Play (SoundManager.instance.buttonClick);

				this.gameObject.SetActive (false);
				gameManager.SetActive (true);
                //GameManager.instance.InitLevel (level.number);
                PipeLockManager.instance.InitLevel(level.number);
			}
		}
	}

}