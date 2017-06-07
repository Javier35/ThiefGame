using UnityEngine;
using System.Collections;

public class LifeCollectible : Collectible {

	void Start(){
		SetLevelManager ();
	}


	void OnTriggerEnter2D (Collider2D col){

		if (col.gameObject.tag == "Player") {
			Inventory.GainALife();
			DestroySelf ();
		}
	}
}
