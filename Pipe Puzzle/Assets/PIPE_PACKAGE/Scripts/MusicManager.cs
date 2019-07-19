using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE {

	public class MusicManager : MonoBehaviour {

		public AudioClip[] musicList;
		public static MusicManager instance;
		public List<GameObject> uiMusicButtons;
		void Awake(){
			instance = this;
			DontDestroyOnLoad (gameObject);
			SetMusicVolume(PlayerPrefs.GetInt ("Music"));
		}

		public void Play(AudioClip music){
			AudioSource source = this.gameObject.GetComponent<AudioSource> ();
			source.Stop ();
			source.loop = true;
			source.clip = music;
			source.Play ();
		}

		public void MusicSwitch(){
			if (PlayerPrefs.GetInt ("Music") == 0) {
				SetMusicVolume(1);
			} else {
				SetMusicVolume(0);
			}
		}

		public void SetMusicVolume(int volume){
			AudioSource source = this.gameObject.GetComponent<AudioSource> ();
			source.volume = volume;
			PlayerPrefs.SetInt ("Music", (int)volume);
			PlayerPrefs.Save();

			//Canvas/MenuPause/ButtonMusic/ButtonMusicOff .....
			foreach (GameObject gameObject in uiMusicButtons){
				gameObject.SetActive (volume != 1);
			}
		}

	}
}