﻿using UnityEngine;
using System.Collections;

public class EnemyDamageManager : DamageManager {

	// private ItemDropModule itemDropper;
	private LevelManager levelManager;
	private GameObject player;
	public GameObject deathPoof;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Cricket");
		levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
		// itemDropper = gameObject.GetComponent<ItemDropModule> ();
	}

	public override void ReceiveDamage(int damage){
		health -= damage;

		if (health > 0) {
			spriteEffector.FlashRedOnce ();
		}else{
			DestroySelf();
		}
	}

	private int GetKnockbackDir(){
		Vector2 playerPos = player.transform.position;
		if (playerPos.x <= transform.position.x) {
			//fall to the right
			return 1;

		} else {
			//fall to the left
			return -1;
		}
	}

	override public void DestroySelf(){

		if (!spawned) {
			levelManager.GetComponent<LevelManager> ().respawnables.Add (this.gameObject);
			//var knockbackDir = GetKnockbackDir ();
			//itemDropper.DropItem (knockbackDir);
			Deactivate();
		} else {
			Destroy (gameObject);
		}
		Instantiate(deathPoof, this.transform.position, this.transform.rotation);
	}

	void Deactivate(){
		gameObject.SetActive(false);
	}

	override public void Respawn(){

		gameObject.SetActive(true);
		Heal (99);
		gameObject.GetComponent<EnemyDamageManager> ().spriteEffector.stopFlicker ();
		//Debug.Log (transform.position);
	}
}
