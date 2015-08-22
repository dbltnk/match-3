using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// define tile: x, y, color(r,g,b), status(normal, marked for delete, empty)

	public enum Flavour {RED, GREEN, BLUE, PINK, YELLOW};
	public enum Status {NEW, NORMAL, DELETE, EMPTY};
	public List<Tile> Tiles = new List<Tile>();
	public List<GameObject> GameObjects = new List<GameObject>();
	public int PlayfieldWidth = 5;
	public int PlayfieldHeight = 7;
	public GameObject ObjectTileRED;
	public GameObject ObjectTileGREEN;
	public GameObject ObjectTileBLUE;
	public GameObject ObjectTilePINK;
	public GameObject ObjectTileYELLOW;
	public GameObject RootObject;

	public class Tile
	{
		public int X;
		public int Y;
		public Flavour Flavour;
		public Status Status;
		
		public Tile(int x, int y, Flavour f, Status s)
		{
			X = x;
			Y = y;
			Flavour = f;
			Status = s;
		}
	}


	void Start () {
		// setup playfield
		// create rows
		// create columns
		// for each tile random color

		for (int row = 1; row <= PlayfieldWidth; row++)
		{
			for (int line = 1; line <= PlayfieldHeight; line++)
			{
				Tile t = CreateNewTileOfRandomFlavour (row, line);
				Tiles.Add(t);
			}
		}
	}

	Tile CreateNewTileOfRandomFlavour(int x, int y) {

		Status s = Status.NEW;
		Flavour f;

		int r = Random.Range (0, 5);
		if (r == 1) {
			f = Flavour.RED;
		} else if (r == 2) {
			f = Flavour.GREEN;
		} else if (r == 3) {
			f = Flavour.BLUE;
		} else if (r == 4) {
			f = Flavour.YELLOW;
		} else {
			f = Flavour.PINK;
		}

		Tile tile = new Tile (x, y, f, s);
		return tile;
	}
	
	void Update () {
		// display playfield

		foreach (Tile tile in Tiles) {
			if (tile.Status == Status.NEW) {
				GameObject o;
				if (tile.Flavour == Flavour.RED) {
					o = GameObject.Instantiate(ObjectTileRED, new Vector3(tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				}
				else if (tile.Flavour == Flavour.GREEN){
					o = GameObject.Instantiate(ObjectTileGREEN, new Vector3(tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				}
				else if (tile.Flavour == Flavour.BLUE){
					o = GameObject.Instantiate(ObjectTileBLUE, new Vector3(tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				}
				else if (tile.Flavour == Flavour.PINK){
					o = GameObject.Instantiate(ObjectTilePINK, new Vector3(tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				}
				else {
					o = GameObject.Instantiate(ObjectTileYELLOW, new Vector3(tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				}
				GameObjects.Add(o);
				o.transform.SetParent(RootObject.transform);
				o.name = string.Concat("tile-", tile.X, "-", tile.Y);
				tile.Status = Status.NORMAL;
			}
		}

		foreach (Tile queryTile in Tiles) {
			QueryNeighbours(queryTile);
		}

		// go through tiles from tl to br
			// for each tile count neighbours with same color
				// if n >= 2 mark all for delete
		// delete marked tiles
		// from br to tl go through each tile
			// if empty then move tile above down
			// if no tile above then spawn new random tile here

		// GetNeighbours
	}

	Tile GetTile (int x, int y) 
	{ 
		if (x <= 0 || x >= PlayfieldWidth || y <= 0 || y >= PlayfieldHeight)
			return null;

		foreach (Tile tile in Tiles) {
			if (tile.X == x && tile.Y == y) {
				return tile;
			}
		}

		return null;
	}

	void QueryNeighbours (Tile tile) 
	{
		if (tile.X <= 0 || tile.X >= PlayfieldWidth || tile.Y <= 0 || tile.Y >= PlayfieldHeight)
			return;

		int horizontalCount = 0;

		Tile leftNeighbour = GetTile (tile.X - 1, tile.Y);
		if (IsFlavourEqual(tile, leftNeighbour)) {
			horizontalCount += 1;
		}

	    Tile rightNeighbour = GetTile (tile.X + 1, tile.Y);
		if (IsFlavourEqual(tile, rightNeighbour)) {
			horizontalCount += 1; 
		}

		if (horizontalCount == 2) {
			MarkForDeletion(tile);
			MarkForDeletion(leftNeighbour);
			MarkForDeletion(rightNeighbour);
		}
	}

	bool IsFlavourEqual (Tile t1, Tile t2) {
		if (t1.Flavour == t2.Flavour) {
			return true;
		} else {
			return false;
		}
	}

	void MarkForDeletion (Tile tile) {
		tile.Status = Status.DELETE;
	}
}
