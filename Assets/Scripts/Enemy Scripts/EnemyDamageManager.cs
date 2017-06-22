using UnityEngine;
using System.Collections;

public class EnemyDamageManager : DamageManager {

	public float deathTime = 1f;

	private ItemDropModule itemDropper;
	private LevelManager levelManager;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Cricket");
		levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
		itemDropper = gameObject.GetComponent<ItemDropModule> ();
	}

	public override void ReceiveDamage(int damage){
		Debug.Log ("ouch");
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
		//poof
	}

	void Deactivate(){
		gameObject.SetActive(false);
	}

	override public void Respawn(){

		gameObject.SetActive(true);
		gameObject.GetComponent<EnemyDamageManager> ().spriteEffector.stopFlicker ();
		//Debug.Log (transform.position);
	}
}
