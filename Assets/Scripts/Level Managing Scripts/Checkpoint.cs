using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private LevelManager levelManager;
	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			levelManager.currentCheckpoint = gameObject;
			//this.GetComponent<BoxCollider2D> ().enabled = false; //in case you wanna make it impossible to revisit old checkpoints
		}
	}
}
