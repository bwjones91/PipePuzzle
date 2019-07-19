using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE {

	public class SoundManager : MonoBehaviour {

		public AudioClip buttonClick;
		public AudioClip levelComplete;
		public AudioClip gameover;
		public static SoundManager instance;
		public List<GameObject> uiSoundButtons;

		void Awake(){
			instance = this;
			DontDestroyOnLoad (gameObject);
			SetSoundVolume(PlayerPrefs.GetInt ("Sound"));
		}

		public void Play(AudioClip sound){
			AudioSource source = this.gameObject.GetComponent<AudioSource> ();
			source.clip = sound;
			source.Play ();
		}

		public void SoundSwitch(){
			if (PlayerPrefs.GetInt ("Sound") == 0) {
				SetSoundVolume(1);
			} else {
				SetSoundVolume(0);
			}
		}

		public void SetSoundVolume(float volume){
			AudioSource source = this.gameObject.GetComponent<AudioSource> ();
			source.volume = volume;
			PlayerPrefs.SetInt ("Sound", (int)volume);
			PlayerPrefs.Save();

			//Canvas/MenuPause/ButtonSound/ButtonSoundOff .....
			foreach (GameObject gameObject in uiSoundButtons){
				gameObject.SetActive (volume != 1);
			}
		}

	}
}