using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageManager : Destroyable {

	[SerializeField] private int damage = 1;
	private Collider2D thisCollider;


	void Start(){
		thisCollider = gameObject.GetComponent<BoxCollider2D> ();
	}

	void OnCollisionEnter2D(Collision2D col){
		
		if (col.gameObject.tag == "Platform" || col.gameObject.tag == "Player") {

			if(col.gameObject.tag == "Player"){
				var playersScript = col.gameObject.GetComponent<PlayerDamageManager> ();
				playersScript.ReceiveDamage (damage);
			}

			DestroySelf ();
		}else{
			Physics2D.IgnoreCollision (col.collider, thisCollider);
		}
	}

	override public void DestroySelf(){
		Destroy (gameObject);
	}

	override public void Respawn (){}
}
