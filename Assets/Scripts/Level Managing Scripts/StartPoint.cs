using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

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
