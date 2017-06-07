using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnTop : MonoBehaviour {

	public float bounceForce = 200f;
	private Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();

	}

	void OnCollisionEnter2D(Collision2D collision){
		
		ContactPoint2D contact = collision.contacts [0];
		if (Vector3.Dot (contact.normal, Vector3.down) > 0.5) {
			var destroyable = collision.gameObject.GetComponent<Destroyable> ();
			if (destroyable != null) {

				if(anim != null)
					anim.SetTrigger ("Bounce");
				
				if (collision.gameObject.tag == "Player") {
					var playerBehavior = collision.gameObject.GetComponent<PlatformerCharacter2D> ();
					playerBehavior.DoJump (bounceForce);

				} else {
					if(collision.rigidbody != null)
						collision.rigidbody.AddForce (new Vector2 (0f, bounceForce));
				}
			}
		}
	}

}
