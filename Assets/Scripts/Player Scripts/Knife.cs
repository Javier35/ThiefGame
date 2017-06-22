using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ProjectileBehavior {

	private List<int> objectsHit = new List<int>();
	[SerializeField] int damageStrength = 1;

	void Update () {
		rbody.velocity = new Vector2 (speed * direction, rbody.velocity.y);
	}

	override protected void DeactivateSelf(){
		ClearHittedList ();
		this.gameObject.SetActive (false);
	}

	public void ClearHittedList (){
		objectsHit.Clear ();
	}

	void OnCollisionEnter2D(Collision2D col){
		DeactivateSelf ();
	}

	void OnTriggerEnter2D(Collider2D col){

		var hitHandler = col.gameObject.GetComponent<HitHandler> ();
		if (hitHandler != null) {

			int hittedObjectId = col.gameObject.GetInstanceID ();

			if (!objectsHit.Contains (hittedObjectId)) {
				hitHandler.HitEvent (damageStrength);
				objectsHit.Add (hittedObjectId);
				DeactivateSelf ();
			}
		}
	}
}
