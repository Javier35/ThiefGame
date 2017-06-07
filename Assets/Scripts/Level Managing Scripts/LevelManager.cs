using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private GameObject player;
	public Fading fader;

	public ArrayList respawnables = new ArrayList ();
	public ArrayList movables = new ArrayList ();

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Moxi");
		fader = gameObject.GetComponent<Fading> ();
	}

	public void RespawnPlayer(){

		Inventory.LoseALife ();
		if (checkGameOver()) {
			//game over logic
			SceneManager.LoadScene("LT1");
			return;
		}

		player.transform.position = currentCheckpoint.transform.position;

		player.transform.position = new Vector3 (
			currentCheckpoint.transform.position.x, 
			currentCheckpoint.transform.position.y - 0.35f,
			player.transform.position.z);
		
		player.GetComponent<PlayerDamageManager> ().Heal (99);
		player.GetComponent<PlayerDamageManager> ().BecomeInvincible ();

		HealAllEnemies ();
		RespawnRespawnables ();
		ResetMovables ();

		fader.BeginFade (-1);
	}

	public void RespawnRespawnables(){
		foreach (GameObject respawnable in respawnables) {
			respawnable.GetComponent<Destroyable> ().Respawn ();
		}
		respawnables.Clear ();
	}

	public void ResetMovables(){
		var movableObjects = GameObject.FindObjectsOfType<Movable> ();
		foreach (Movable movableObject in movableObjects) {
			movableObject.ResetPosition ();
		}
	}

	public void HealAllEnemies(){
		var allEnemies = GameObject.FindObjectsOfType<EnemyDamageManager> ();

		foreach (EnemyDamageManager enemy in allEnemies) {
			enemy.Heal (999);
		}
	}

	public bool checkGameOver(){
		if (Inventory.GetLives () <= 0)
			return true;
		return false;
	}
}
