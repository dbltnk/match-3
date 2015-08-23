using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

	public enum GameState {SETUP, CLEANUP, INPUT, MATCHING, DELETE, REFILL, WAITING};
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
		} else if (State == GameState.CLEANUP) {
			GameManager.Cleanup ();
		} else if (State == GameState.DELETE) {
			GameManager.DeleteMatches ();
		} else if (State == GameState.REFILL) {
			GameManager.Refill ();
		} else {
			// nothing happens		
		}
	}

	public void ChangeStateTo (GameState targetState) {
		string s = string.Concat ("Changing state from ", State.ToString (), " to ", targetState.ToString ());
		Debug.Log (s);
		State = GameState.WAITING;
		StartCoroutine(DoChangeState(targetState));
	}

	IEnumerator DoChangeState(GameState targetState) {
		yield return new WaitForSeconds(2f);
		State = targetState;
		string t = string.Concat("Switched state to ", State.ToString());
		Debug.Log (t);
	}
}
