using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class ProgressBar : MonoBehaviour {
		Image slider;
		public static ProgressBar Instance;
		public int starsCount = 0;
		public float limitTimeMax = 0;
		public float limitTime = 0;
		public float starTime1 = 0;
		public float starTime2 = 0;
		public float starTime3 = 0;
		public float currentTime = 0;
		public float pauseTime = 0; //Record pause time
		public bool isPaused = false;

		public GameObject star1;
		public GameObject star2;
		public GameObject star3;

		public event EventHandler Timeout;

		void Awake () {
			Instance = this;
			slider = GetComponent<Image>();
			ResetBar();
		}

		public void SetLimit(float limit, float time1, float time2, float time3){
			limitTimeMax = limit;
			limitTime = limit;
			starTime1 = time1;
			starTime2 = time2;
			starTime3 = time3;
			limitTime = limitTime + Time.time;
			ResetBar ();
			isPaused = false;
		}

		public void SetValue(float w){
			slider.fillAmount = (w/limitTimeMax);

			if (currentTime < starTime3) {
				star3.SetActive (false);
				starsCount = 2;
			} 
			if (currentTime < starTime2) {
				star2.SetActive (false);
				starsCount = 1;
			} 
			if (currentTime < starTime1) {
				star1.SetActive (false);
				starsCount = 0;
			} 
		}

		public void AddValue (float w) {
			currentTime += w;
		}

		public void SubValue (float w) {
			currentTime -= w;
		}

		public void Pause () {
			isPaused = true;
			pauseTime = Time.time;
		}

		public void Resume () {
			isPaused = false;
			limitTime += Time.time - pauseTime;
		}

		void FixedUpdate(){
			if (isPaused) {
				return;
			}

			currentTime = limitTime - Time.time;

			if (currentTime <= 0)
			{
				currentTime = 0;
				if (Timeout != null)
					Timeout.Invoke(this, new EventArgs());
			}
			SetValue (currentTime);
		}

		public void ResetBar(){
			slider.fillAmount = 1.0f;
			star1.SetActive (true);
			star2.SetActive (true);
			star3.SetActive (true);
			starsCount = 3;
			if (limitTimeMax > 0) {
				RectTransform rectTransform = this.GetComponent<RectTransform> ();
				star1.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 ((starTime1 / limitTimeMax) * rectTransform.sizeDelta.x, star1.transform.localPosition.y, star1.transform.localPosition.z);
				star2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 ((starTime2 / limitTimeMax) * rectTransform.sizeDelta.x, star2.transform.localPosition.y, star2.transform.localPosition.z);
				star3.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 ((starTime3 / limitTimeMax) * rectTransform.sizeDelta.x, star3.transform.localPosition.y, star3.transform.localPosition.z);
			}
		}
	}

}