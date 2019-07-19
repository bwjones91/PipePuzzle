using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	[System.Serializable]
	public enum Rotation{
		Rotation0 = 0,
		Rotation90,
		Rotation180,
		Rotation270,
	}

	[System.Serializable]
	public class ItemData{
		public int ID;
		public Rotation rotation;
	}

	public class Item : MonoBehaviour {

		public int ID=-1;

		public Rotation rotation = Rotation.Rotation0;

		//Which direction of the item can be connected
		public bool enableTopJoin = true;
		public bool enableBottomJoin = true;
		public bool enableLeftJoin = true;
		public bool enableRightJoin = true;

		//Fixed item cannot be rotated
		public bool isFixed = false;

		//Key items must be connected in order to pass
		public bool isKeyItem = false;

		//Sound
		public AudioClip selectedSound;
		public AudioClip selectedSoundWorng;

		//Record whether the direction has been checked for each operation
		[HideInInspector] public bool isTopChecked = false;
		[HideInInspector] public bool isBottomChecked  = false;
		[HideInInspector] public bool isLeftChecked  = false;
		[HideInInspector] public bool isRightChecked  = false;

		void Awake(){
			name = gameObject.name;

		}
			
		public void Click () {
			//Return if the item cannot be rotated
			if (isFixed) {
				SoundManager.instance.Play(selectedSoundWorng);
				return;
			}
			SoundManager.instance.Play(selectedSound);

			//Calculate rotation angle
			if (rotation == Rotation.Rotation270) {
				rotation = Rotation.Rotation0;
			} else {
				rotation++;
			}

			SetRotation (rotation);
		}

		public void SetRotation (Rotation r) {
			rotation = r;
			this.transform.rotation = Quaternion.Euler(0, 0, -90*(int)rotation);
		}

		//According to the rotation angle to determine whether the top can be connected
		public bool isTopJoin () {
			if (rotation == Rotation.Rotation0) {
				return enableTopJoin;
			} else if (rotation == Rotation.Rotation90) {
				return enableLeftJoin;
			} else if (rotation == Rotation.Rotation180) {
				return enableBottomJoin;
			} else {
				return enableRightJoin;
			}
		}

		//According to the rotation angle to determine whether the right can be connected
		public bool isRightJoin () {
			if (rotation == Rotation.Rotation0) {
				return enableRightJoin;
			} else if (rotation == Rotation.Rotation90) {
				return enableTopJoin;
			} else if (rotation == Rotation.Rotation180) {
				return enableLeftJoin;
			} else {
				return enableBottomJoin;
			}
		}

		//According to the rotation angle to determine whether the bottom can be connected
		public bool isBottomJoin () {
			if (rotation == Rotation.Rotation0) {
				return enableBottomJoin;
			} else if (rotation == Rotation.Rotation90) {
				return enableRightJoin;
			} else if (rotation == Rotation.Rotation180) {
				return enableTopJoin;
			} else {
				return enableLeftJoin;
			}
		}

		//According to the rotation angle to determine whether the left can be connected
		public bool isLeftJoin () {
			if (rotation == Rotation.Rotation0) {
				return enableLeftJoin;
			} else if (rotation == Rotation.Rotation90) {
				return enableBottomJoin;
			} else if (rotation == Rotation.Rotation180) {
				return enableRightJoin;
			} else {
				return enableTopJoin;
			}
		}

		//Reset check records for each direction
		public void resetCheckedInfo () {
			isTopChecked = !isTopJoin ();
			isRightChecked  = !isRightJoin ();
			isBottomChecked  = !isBottomJoin ();
			isLeftChecked  = !isLeftJoin ();
		}
	}
}