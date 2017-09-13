using UnityEngine;
using System.Collections;

public class PitEffects : MonoBehaviour {

	private LevelManager levelManager;
	private bool respawning = false;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player") {

			levelManager.RespawnPlayer ();

		} else {
			var destroyable = other.GetComponent<Destroyable> ();
			if (destroyable != null) {
				destroyable.DestroySelf ();
			}
		}

	}

	void ToggleRespawnFlag(){
		respawning = !respawning;
	}

}
