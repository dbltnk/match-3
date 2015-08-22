﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// define tile: x, y, color(r,g,b), status(normal, marked for delete, empty)

	public enum Flavour {RED, GREEN, BLUE};
	public enum Status {NORMAL, DELETE, EMPTY};

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

		int x = 3;
		int y = 5;

		CreateNewTileOfRandomFlavour (x, y);

		// setup playfield
			// create rows
				// create columns
		// for each tile random color
	}

	Tile CreateNewTileOfRandomFlavour(int x, int y) {

		Status s = Status.NORMAL;
		Flavour f;

		int r = Random.Range (1, 3);
		if (r == 1) {
			f = Flavour.RED;
		} else if (r == 2) {
			f = Flavour.GREEN;
		} else {
			f = Flavour.BLUE;
		}

		Tile tile = new Tile (x, y, f, s);
		return tile;
	}
	
	void Update () {
		// go through tiles from tl to br
			// for each tile count neighbours with same color
				// if n >= 2 mark all for delete
		// delete marked tiles
		// from br to tl go through each tile
			// if empty then move tile above down
			// if no tile above then spawn new random tile here

		// GetNeighbours
	}
}
