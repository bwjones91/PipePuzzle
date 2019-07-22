using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class Level : MonoBehaviour {
		//Display level number
		public int number;
		public GameObject text;

		//Display the number of level stars
		public int starsCount;
		public Transform star1;
		public Transform star2;
		public Transform star3;

		//Level unlock status
		public bool isLocked;
		public Transform lockT;

		public event EventHandler LevelClicked;			// Event is invoked when user clicks the object
		public event EventHandler LevelHighlighted;		// Event is invoked when user moves cursor over the object
		public event EventHandler LevelDehighlighted;	// Event is invoked when user moves cursor out the object

		public void OnMouseEnter(){
			if (LevelHighlighted != null)
				LevelHighlighted.Invoke(this, new EventArgs());
		}

		public void OnMouseDown(){
			if (LevelClicked != null)
				LevelClicked.Invoke(this, new EventArgs());
		}

		public void OnMouseExit(){
			if (LevelDehighlighted != null)
				LevelDehighlighted.Invoke(this, new EventArgs());
		}

		//Set level number
		public void SetNumber(int num){
			number = num;
			name = "Level" + number;
			text.GetComponent<Text>().text = "" + num;
		}

		//Set level unlock
		public void SetLock(bool locked){
			isLocked = locked;
			lockT.gameObject.SetActive(isLocked);
		}

		//Set level stars
		public void SetStars(int count){
			starsCount = count;
			star1.gameObject.SetActive(starsCount >= 1);
			star2.gameObject.SetActive(starsCount >= 2);
			star3.gameObject.SetActive(starsCount >= 3);
		}
	}

}