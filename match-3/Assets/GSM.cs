using UnityEngine;
using System.Collections;

public class GSM : MonoBehaviour {

	public enum GameState {SETUP, CLEANUP, INPUT, MATCHING, DELETE, REFILL};
	public GameState State;
	GameManager GameManager;

	void Awake () {
		GameManager = GetComponent<GameManager> ();
	}

	void Start () {
		State = GameState.SETUP;
	}
	
	void Update () {
		if (State == GameState.SETUP) {
			GameManager.Setup ();
		} 
		else if (State == GameState.CLEANUP) {
			bool cleanUpNeeded = GameManager.CheckForMatches();
			if (cleanUpNeeded) {
				State = GameState.DELETE;
			}
			else {
				State = GameState.INPUT;
			}
		}
		else if (State == GameState.DELETE) {
			GameManager.DeleteMatches();
		}
		else if (State == GameState.REFILL) {
			GameManager.Refill();
		}
	}

	public void ChangeStateTo (GameState targetState) {
		State = targetState;
	}
}
