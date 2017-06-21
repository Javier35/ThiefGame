using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ProjectileBehavior {

	// Use this for initialization
	void Update () {
		rbody.velocity = new Vector2 (speed * direction, rbody.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D col){
		DeactivateSelf ();
	}
}
