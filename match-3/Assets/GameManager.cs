using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GSM GSM;

	public enum Flavour {RED, GREEN, BLUE, PINK, YELLOW, NONE};
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
	public GameObject ObjectTileBLACK;
	public GameObject RootObject;

	public class Tile
	{
		public int X;
		public int Y;
		public Flavour Flavour;
		public Status Status;
		public GameObject ObjectReference;
		
		public Tile(int x, int y, Flavour f, Status s, GameObject o)
		{
			X = x;
			Y = y;
			Flavour = f;
			Status = s;
			ObjectReference = o;
		}
	}

	void Awake () {
		GSM = GetComponent<GSM> ();
		GSM.ChangeStateTo (GSM.GameState.SETUP);
	}

	public void Setup () {
		for (int row = 1; row <= PlayfieldWidth; row++)
		{
			for (int line = 1; line <= PlayfieldHeight; line++)
			{
				Tile t = CreateNewTileOfRandomFlavour (row, line);
				Tiles.Add(t);
			}
		}
		BuildPlayfield ();
		GSM.ChangeStateTo (GSM.GameState.CLEANUP);
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

		Tile tile = new Tile (x, y, f, s, null);
		return tile;
	}

	void BuildPlayfield () {
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
				tile.ObjectReference = o;
				tile.Status = Status.NORMAL;
			}
		}
	}

	void UpdatePlayfield () {
		foreach (Tile tile in Tiles) {
			if (tile.Status == Status.DELETE) {
				GameObject.DestroyImmediate (tile.ObjectReference);
				GameObject o = GameObject.Instantiate (ObjectTileBLACK, new Vector3 (tile.X, tile.Y, 0f), Quaternion.identity) as GameObject;
				GameObjects.Add (o);
				o.transform.SetParent (RootObject.transform);
				o.name = string.Concat ("tile-", tile.X, "-", tile.Y, "-DEL");
				tile.ObjectReference = o;
				tile.Status = Status.EMPTY;
			}
		}
	}

	public bool CheckForMatches () {
		bool matchFound = false;

		foreach (Tile tile in Tiles) {
			int horizontalCount = 0;
			
			Tile leftNeighbour = GetTile (tile.X - 1, tile.Y);
			if (leftNeighbour != null) {
				if (IsFlavourEqual (tile, leftNeighbour)) {
					horizontalCount += 1;
				}
			}
			
			Tile rightNeighbour = GetTile (tile.X + 1, tile.Y);
			if (rightNeighbour != null) {
				if (IsFlavourEqual(tile, rightNeighbour)) {
					horizontalCount += 1; 
				}
			}

			int verticalCount = 0;
			
			Tile topNeighbour = GetTile (tile.X, tile.Y + 1);
			if (topNeighbour != null) {
				if (IsFlavourEqual (tile, topNeighbour)) {
					verticalCount += 1;
				}
			}
			
			Tile bottomNeighbour = GetTile (tile.X, tile.Y - 1);
			if (bottomNeighbour != null) {
				if (IsFlavourEqual(tile, bottomNeighbour)) {
					verticalCount += 1; 
				}
			}

			if (verticalCount >= 2 || horizontalCount >= 2) {
				matchFound = true;
			}
		}

		if (matchFound == true) {
			return true;
		}
		else {
			return false;
		}
	}

	public void Cleanup () {
		string m = CheckForMatches().ToString();
		m = string.Concat ("Matches found: ", m);
		Debug.Log (m);
	}

	Tile GetTile (int x, int y) 
	{ 
		if (x < 1 || x > PlayfieldWidth || y < 1 || y > PlayfieldHeight)
			return null;

		foreach (Tile tile in Tiles) {
			if (tile.X == x && tile.Y == y) {
				return tile;
			}
		}

		return null;
	}

	void HasNeighbours (Tile tile) {
		Tile leftNeighbour = GetTile (tile.X - 1, tile.Y);
		Tile rightNeighbour = GetTile (tile.X + 1, tile.Y);

		string t;
		if (leftNeighbour == null && rightNeighbour == null) {
			t = string.Concat ("tile ", tile.X, tile.Y, " has no neighbours");
		} else if (leftNeighbour == null) {
			t = string.Concat ("tile ", tile.X, tile.Y, " has no left neighbour");
		} else if (rightNeighbour == null) {
			t = string.Concat ("tile ", tile.X, tile.Y, " has no right neighbour");
		} 
		else {
			t = string.Concat(tile.X,tile.Y,leftNeighbour.X,leftNeighbour.Y,rightNeighbour.X,rightNeighbour.Y);
		}
		Debug.Log(t);
	}

	void MarkMatchedTilesForDeletion (Tile tile) 
	{
		int horizontalCount = 0;

		Tile leftNeighbour = GetTile (tile.X - 1, tile.Y);
		if (leftNeighbour != null) {
			if (IsFlavourEqual (tile, leftNeighbour)) {
				horizontalCount += 1;
			}
		}

	    Tile rightNeighbour = GetTile (tile.X + 1, tile.Y);
		if (rightNeighbour != null) {
			if (IsFlavourEqual(tile, rightNeighbour)) {
				horizontalCount += 1; 
			}
		}

		if (horizontalCount >= 2) {
			MarkForDeletion(tile);
			MarkForDeletion(leftNeighbour);
			MarkForDeletion(rightNeighbour);
		}

		int verticalCount = 0;
		
		Tile topNeighbour = GetTile (tile.X, tile.Y + 1);
		if (topNeighbour != null) {
			if (IsFlavourEqual (tile, topNeighbour)) {
				verticalCount += 1;
			}
		}
		
		Tile bottomNeighbour = GetTile (tile.X, tile.Y - 1);
		if (bottomNeighbour != null) {
			if (IsFlavourEqual(tile, bottomNeighbour)) {
				verticalCount += 1; 
			}
		}
		
		if (verticalCount >= 2) {
			MarkForDeletion(tile);
			MarkForDeletion(topNeighbour);
			MarkForDeletion(bottomNeighbour);
		}
	}

	public void DeleteMatches () {
		foreach (Tile queryTile in Tiles) {
			MarkMatchedTilesForDeletion(queryTile);
		}

		UpdatePlayfield ();

		foreach (Tile queryTile in Tiles) {
			DeleteMarkedTiles(queryTile);
		}

		GSM.ChangeStateTo (GSM.GameState.REFILL);
	}

	void DeleteMarkedTiles (Tile tile) {
		if (tile.Status == Status.DELETE) {
			tile.Flavour = Flavour.NONE;
			tile.Status = Status.EMPTY;
			DestroyImmediate (tile.ObjectReference);
			tile.ObjectReference = null;
		}
	}

	public void Refill() {
		foreach (Tile queryTile in Tiles) {
			if (queryTile.Status == Status.EMPTY) {
				LogTile(queryTile);
				// refill here
			}
		}
	}

	void LogTile (Tile tile) {
		string t = string.Concat ("Tile ", tile.X, "-", tile.Y, " is ", tile.Flavour, " has status ", tile.Status, " ref = ", tile.ObjectReference);
		Debug.Log (t);
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
