using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class Collectible : Destroyable {

	public LevelManager levelManager;
	private Collider2D thisCollider;
	private bool bounced = false;

	void Start(){
		spriteEffector = GetComponent<SpriteEffector> ();
	}

	public void SetLevelManager(){
		levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
		thisCollider = gameObject.GetComponent<BoxCollider2D> ();
	}

	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag != "Platform") {
			var otherCollider = col.gameObject.GetComponent<Collider2D> ();
			Physics2D.IgnoreCollision (otherCollider, thisCollider);
		} else {

			if (!bounced) {
				bounced = true;
				Vector2 contactPoint = col.contacts[0].normal;

				GetComponent<Rigidbody2D> ().AddForce (col.contacts[0].normal * 1.5f, ForceMode2D.Impulse );
			}
		}
	}

	override public void DestroySelf(){
		if (!spawned) {
			levelManager.respawnables.Add (this.gameObject);
			gameObject.SetActive (false);
		} else {
			Destroy (gameObject);
		}
	}

	override public void Respawn(){
		gameObject.SetActive (true);
	}

	public void startSpawnedBehavior(){
		StartCoroutine (configureSpawnedBehavior());
	}

	public IEnumerator configureSpawnedBehavior(){

		setSpawned(true);
		GetComponent<Float> ().enabled = false;
		GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;

		yield return new WaitForSeconds (3.5f);
		StartCoroutine (spriteEffector.Flicker (2.2f));
		yield return new WaitForSeconds (2.2f);
		DestroySelf ();
	}
}
