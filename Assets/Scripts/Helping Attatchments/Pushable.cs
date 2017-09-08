using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Movable {

	PlatformerCharacter2D playerBehaviorReference;
	public float pushSpeed = 1.7f;

	void Start(){
		playerBehaviorReference = GameObject.Find ("Moxi").GetComponent<PlatformerCharacter2D> ();
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && playerBehaviorReference.grounded) {

			ContactPoint2D contact = collision.contacts [0];
			if (Vector3.Dot (contact.normal, Vector3.left) > 0.1 && !playerBehaviorReference.facingRight) {

				playerBehaviorReference.SetMaxSpeed (pushSpeed);
				rbody.velocity = new Vector2 (-pushSpeed, rbody.velocity.y);

			}else if(Vector3.Dot (contact.normal, Vector3.right) > 0.1 && playerBehaviorReference.facingRight){

				playerBehaviorReference.SetMaxSpeed (pushSpeed);
				rbody.velocity = new Vector2 (pushSpeed, rbody.velocity.y);
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision){
		if (collision.gameObject.tag == "Player") {
			playerBehaviorReference.RestoreMaxSpeed ();
			rbody.velocity = new Vector2 (0, rbody.velocity.y);
		}
	}
}
