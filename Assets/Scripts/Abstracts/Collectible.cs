using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class Collectible : Destroyable {

	public LevelManager levelManager;
	private Collider2D thisCollider;

	[SerializeField] GameObject messageIndicator;
	private GameObject message;

	void Start(){
		spriteEffector = GetComponent<SpriteEffector> ();
	}

	public void SetLevelManager(){
		levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
	}

	override public void DestroySelf(){
		if (!spawned) {
			levelManager.respawnables.Add (this.gameObject);
			gameObject.SetActive (false);
		} else {
			Destroy (gameObject);
		}

		if (messageIndicator != null) {
			Vector3 spawnPos = this.transform.position;
			Instantiate (messageIndicator, spawnPos, this.transform.rotation); 
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
