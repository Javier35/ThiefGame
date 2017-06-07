using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropModule : MonoBehaviour {

	[SerializeField] private GameObject[] items;
	[SerializeField] private int[] dropRates;


	public GameObject RollForItem(){

		for (int i = 0; i < items.Length; i ++) {
			var currentRoll = Random.Range (1, 101);
			if (currentRoll <= dropRates[i]) {
				return (GameObject)items [i];
			}
		}
		return null;
	}
	
	public void DropItem(int knockbackDir){
		
		var item = RollForItem ();

		if (item != null) {
			GameObject spawnedItem = (GameObject)Instantiate (item, this.transform.position, this.transform.rotation);
			spawnedItem.GetComponent<Collectible> ().configureSpawnedBehavior();
			var itemScript = spawnedItem.GetComponent<Collectible> ();
			itemScript.startSpawnedBehavior();
			var rbody = spawnedItem.GetComponent<Rigidbody2D> ();
			rbody.AddForce (new Vector2 (35 * knockbackDir, 100));
		}
	}

	public void SpawnItem(){
		var item = (GameObject)items [0];

		GameObject spawnedItem = (GameObject)Instantiate (item, this.transform.position, this.transform.rotation);
		var itemScript = spawnedItem.GetComponent<Collectible> ();
		itemScript.startSpawnedBehavior();
	}
}
