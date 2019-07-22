using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using PIPE_PACKAGE;

namespace PIPE_PACKAGE{

	public class TileManager : MonoBehaviour {

		public static TileManager instance;

		public float itemWidth = 2.56f;
		public float itemHeight = 2.56f;
		public int maxRows = 9;
		public int maxCols = 9;
		public Camera gameCamera;

		protected static ItemDB itemDB;
		public Tile[,] tilesArray;
		public List<Tile> keyTilesList = new List<Tile> ();

		public Sprite ground1;
		public Sprite ground2;

		public event EventHandler TileClicked;
		public event EventHandler TileHighlighted;
		public event EventHandler TileDehighlighted;

		void Awake(){
			if(instance==null) instance = this;
			itemDB = ItemDB.LoadDB ();
		}	


		public void Init (int currentLevel){
			this.transform.position = Vector3.zero;
			this.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			LevelDB levelDB = LevelDB.LoadDB ();
			LevelData levelData = levelDB.levelList [currentLevel - 1];
			List<ItemData> itemList = levelData.itemList;
			ground1 = levelData.ground1;
			ground2 = levelData.ground2;

			maxRows = levelData.maxRows;
			maxCols = levelData.maxCols;
			tilesArray = new Tile[maxRows, maxCols];

			for (int row = 0; row < maxRows; row++) {
				for (int col = 0; col < maxCols; col++) {
					CreateTile (row, col, itemList [row * maxCols + col]);
				}
			}

			//scale to fit screen
			float gameZoneWidth = 6.0f * 2.56f - 1.0f;
			float gameZoneHeight = gameCamera.orthographicSize * 2.0f - 4.0f;
			float scaleX = gameZoneWidth / (TileManager.instance.maxCols * itemWidth);
			float scaleY = gameZoneHeight / (TileManager.instance.maxRows * itemHeight);
			float scale = scaleX < scaleY ? scaleX : scaleY;
			this.transform.position = new Vector3 (-(maxCols - 1) * itemWidth * scale / 2, gameZoneHeight/2 - 2.0f, 10);
			this.transform.localScale = new Vector3 (scale, scale, 1.0f);
		}
			
		private void CreateTile (int row, int col, ItemData itemData){

			GameObject tileObject = new GameObject();
			tileObject.AddComponent<SpriteRenderer>();
			SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.sortingOrder = 1;
			if ((row + col) % 2 == 0) {
				spriteRenderer.sprite = ground1;
			} else {
				spriteRenderer.sprite = ground2;
			}

			tileObject.AddComponent<BoxCollider2D>();
			tileObject.GetComponent<BoxCollider2D>().size = new Vector2 (itemWidth, itemHeight);


			tileObject.AddComponent<Tile>();
			Tile tile = tileObject.GetComponent<Tile> ();
			tile.row = row;
			tile.col = col;
			tile.item = null;
			tile.name = "Tile_"+row+"_"+col;

			tile.TileClicked += OnTileClicked;
			tile.TileHighlighted += OnTileHighlighted;
			tile.TileDehighlighted += OnTileDehighlighted;

			tilesArray [row,col] = tile;

			tileObject.transform.SetParent (this.transform);
			tileObject.transform.position = new Vector2 (col * itemWidth, -row * itemHeight);

			if (itemData.ID >= 0) {
				tile.item = CreateItem (row, col, itemData);
				if (tile.item.isKeyItem) {
					keyTilesList.Add (tile);
				}
			} else {
				tile.item = null;
			}
		}

		private Item CreateItem (int row, int col, ItemData itemData){
			GameObject gameObject = GetItemPrefabByID(itemData.ID);
			if(gameObject){
				GameObject itemObject = Instantiate (gameObject, new Vector2 (col * itemWidth, -row * itemHeight), Quaternion.identity) as GameObject;
				itemObject.transform.SetParent (this.transform);
				Item item = itemObject.GetComponent<Item> ();
				item.SetRotation (itemData.rotation);
				return item;
			}
			return null;	
		}
			
		private GameObject GetItemPrefabByID (int ID){
			for (int i = 0; i < itemDB.itemList.Count; i++) {
				if (ID == itemDB.itemList [i].ID) {
					return itemDB.itemList [i].gameObject;
				}
			}
			return null;
		}

		private void OnTileDehighlighted(object sender, EventArgs e){
			if (TileHighlighted != null) {
				TileHighlighted.Invoke (sender, new EventArgs ());
			}
		}

		private void OnTileHighlighted(object sender, EventArgs e){
			if (TileDehighlighted != null) {
				TileDehighlighted.Invoke (sender, new EventArgs ());
			}
		} 

		private void OnTileClicked(object sender, EventArgs e){
			if (TileClicked != null) {
				TileClicked.Invoke (sender, new EventArgs ());
			}
		}

		public Tile GetTile (int row, int col){
			if (row >= maxRows || col >= maxCols || row < 0 || col < 0) {
				return null;
			}
			return tilesArray [row,col];
		}

		public Item GetItem (int row, int col){
			if (row >= maxRows || col >= maxCols || row < 0 || col < 0) {
				return null;
			}
			return tilesArray [row,col].item;
		}
			
		//Reset all items check records
		public void ResetAllItemCheckedInfo (){
			for (int row = 0; row < maxRows; row++) {
				for (int col = 0; col < maxCols; col++) {
					Item item = GetItem (row, col);
					if (item) {
						item.resetCheckedInfo ();
					}
				}
			}
		}

		public void ClearTileObject (){
			keyTilesList.Clear ();
			for (int row = 0; row < maxRows; row++) {
				for (int col = 0; col < maxCols; col++) {
					tilesArray [row, col] = null;
				}
			}
			for (int i = this.transform.childCount - 1; i >= 0; i--) {  
				DestroyImmediate (this.transform.GetChild (i).gameObject);  
			}  
		}

	}
}