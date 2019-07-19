using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class Tile : MonoBehaviour {

		public int row;
		public int col;

		public Item item;

		//Sound
		public AudioClip selectedSound;

		public event EventHandler TileClicked;
		public event EventHandler TileHighlighted;
		public event EventHandler TileDehighlighted;

		protected virtual void OnMouseEnter(){
			if (TileHighlighted != null) {
				TileHighlighted.Invoke (this, new EventArgs ());
			}
		}

		protected virtual void OnMouseExit(){    
			if (TileDehighlighted != null) {
				TileDehighlighted.Invoke (this, new EventArgs ());
			}
		}

		void OnMouseDown(){
			if (TileClicked != null) {
				TileClicked.Invoke (this, new EventArgs ());
			}
		}

		//Get Neighbor tile in different directions
		public Tile GetNeighborLeft() {
			return TileManager.instance.GetTile(row, (col - 1));
		}

		public Tile GetNeighborRight() {
			return TileManager.instance.GetTile(row, (col + 1));
		}

		public Tile GetNeighborTop() {
			return TileManager.instance.GetTile((row - 1), col);
		}

		public Tile GetNeighborBottom() {
			return TileManager.instance.GetTile((row + 1), col);
		}

	}
}