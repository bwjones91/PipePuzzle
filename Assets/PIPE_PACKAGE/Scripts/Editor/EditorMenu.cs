using UnityEngine;
using System.Collections;
using UnityEditor; 
using PIPE_PACKAGE;
namespace PIPE_PACKAGE{
	public class EditorMenu : MonoBehaviour {

		[MenuItem("Pipe Game Kit/Item Editor", false, 10)] 
		static void OpenItemEditorWindow() { 
			ItemEditor.Init ();
		} 


		[MenuItem("Pipe Game Kit/Level Editor", false, 10)] 
		static void OpenLeverEditorWindow() { 
			LevelEditor.Init ();
		}

		[MenuItem("Pipe Game Kit/Ads Editor", false, 10)] 
		static void OpenAdsEditorWindow() { 
			AdsEditor.Init ();
		}
			
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
	}
}
