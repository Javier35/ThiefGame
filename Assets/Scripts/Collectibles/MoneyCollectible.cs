using UnityEngine;
using System.Collections;

public class MoneyCollectible : Collectible {
	
	[SerializeField] private double moneyAmmount = 100;

	void Start(){
		SetLevelManager ();
		spawned = true;
	}

	void OnTriggerEnter2D (Collider2D col){

		if (col.gameObject.tag == "Player") {
			Inventory.GainMoney(moneyAmmount);
			DestroySelf ();
		}
	}
}
