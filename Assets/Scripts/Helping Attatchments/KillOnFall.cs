using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnFall : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collision){

		ContactPoint2D contact = collision.contacts [0];

		//if it colides from below and the object is destroyable, then destroy it.
		if (Vector3.Dot (contact.normal, Vector3.up) > 0.5) {
			var destroyable = collision.gameObject.GetComponent<Destroyable> ();
			if (destroyable != null) {
				destroyable.DestroySelf ();
			}
		}
	}
}
