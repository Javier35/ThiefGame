using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCheckpoint : HitHandler {


	private LevelManager levelManager;
	private Animator animator;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		animator = GetComponent<Animator> ();
	}


	override public void HitEvent(int damage){
		animator.SetTrigger ("Hitted");
		levelManager.currentCheckpoint = gameObject;
	}
}
