using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE {

	public class MenuMain : MonoBehaviour {

		public GameObject levelManager;
		public GameObject menuSetting;
		void Awake(){

		}

		void Start () {
			MusicManager.instance.Play (MusicManager.instance.musicList[0]);
            Play();
		}

		public void Play(){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			this.gameObject.SetActive (false);
			levelManager.SetActive (true);
		}

		public void Setting(){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			menuSetting.SetActive (true);
		}

		public void CloseSetting(){
			SoundManager.instance.Play (SoundManager.instance.buttonClick);

			menuSetting.SetActive (false);
		}
			
	}


}