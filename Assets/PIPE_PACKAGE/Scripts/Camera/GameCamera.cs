using UnityEngine;
using System.Collections;

using PIPE_PACKAGE;
namespace PIPE_PACKAGE{
	public class GameCamera : MonoBehaviour {

		//design for ipad 2048x1536
		public float designHeight = 20.48f;
		public float designWidth = 15.36f;

		void Awake () {
            
			float aspectRatio = (float)Screen.height / (float)Screen.width;
			this.GetComponent<Camera> ().orthographicSize = (designWidth / 2.0f) * aspectRatio;
            
		}

		// Update is called once per frame
		void Update () {

		}
	}
}